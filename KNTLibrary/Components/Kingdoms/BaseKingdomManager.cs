﻿using HarmonyLib;
using KNTLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace KNTLibrary.Components.Kingdoms
{
    public class BaseKingdomManager<InfoType> : IBaseManager<InfoType, Kingdom> where InfoType : BaseKingdomInfo, new()
    {
        #region Singleton

        private BaseKingdomManager() { }

        static BaseKingdomManager()
        {
            BaseKingdomManager<InfoType>.Instance = new BaseKingdomManager<InfoType>();
        }

        public static BaseKingdomManager<InfoType> Instance { get; private set; }

        #endregion

        #region IManager

        public bool DebugMode { get; set; }

        public HashSet<InfoType> Infos { get; set; } = new HashSet<InfoType>();

        public void InitializeInfos()
        {
            if (this.Infos.Count() == Campaign.Current.Kingdoms.Count())
            {
                return;
            }

            foreach (var gameObject in Campaign.Current.Kingdoms)
            {
                this.GetInfo(gameObject);
            }
        }

        public InfoType GetInfo(Kingdom gameObject)
        {
            var infos = this.Infos.ToList().Where(i => i.KingdomId == gameObject.StringId);
            if (this.DebugMode && infos.Count() > 1)
            {
                InformationManager.DisplayMessage(new InformationMessage("Revolutions: Multiple Kingdoms with same Id. Using first one.", ColorHelper.Orange));
                foreach (var duplicatedInfo in infos)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"Name: {duplicatedInfo.Kingdom.Name} | StringId: {duplicatedInfo.KingdomId}", ColorHelper.Orange));
                }
            }

            var info = infos.FirstOrDefault();
            if (info != null)
            {
                return info;
            }

            info = (InfoType)Activator.CreateInstance(typeof(InfoType), gameObject);
            this.Infos.Add(info);

            return info;
        }

        public InfoType GetInfo(string id)
        {
            var gameObject = this.GetGameObject(id);
            if (gameObject == null)
            {
                return null;
            }

            return this.GetInfo(gameObject);
        }

        public void RemoveInfo(string id)
        {
            var info = this.Infos.FirstOrDefault(i => i.KingdomId == id);
            if (id == null)
            {
                return;
            }

            this.Infos.RemoveWhere(i => i.KingdomId == id);
        }

        public Kingdom GetGameObject(string id)
        {
            return Campaign.Current.Kingdoms.FirstOrDefault(go => go?.StringId == id);
        }

        public Kingdom GetGameObject(InfoType info)
        {
            return this.GetGameObject(info.KingdomId);
        }

        public void UpdateInfos(bool onlyRemoving = false)
        {
            if (this.Infos.Count == Campaign.Current.Kingdoms.Count)
            {
                return;
            }

            this.Infos.RemoveWhere(i => !Campaign.Current.Kingdoms.Any(go => go.StringId == i.KingdomId));

            if (onlyRemoving)
            {
                return;
            }

            foreach (var gameObject in Campaign.Current.Kingdoms.Where(go => !this.Infos.Any(i => i.KingdomId == go.StringId)))
            {
                this.GetInfo(gameObject);
            }
        }

        public void CleanupDuplicatedInfos()
        {
            this.Infos.Reverse();
            this.Infos = this.Infos.GroupBy(i => i.KingdomId)
                                   .Select(i => i.First())
                                   .ToHashSet();
            this.Infos.Reverse();
        }

        #endregion

        public void ModifyKingdomList(Func<List<Kingdom>, List<Kingdom>> modificator)
        {
            var kingdoms = new List<Kingdom>(Campaign.Current.Kingdoms.ToList());
            kingdoms = modificator(kingdoms);
            if (kingdoms != null)
            {
                AccessTools.Field(Campaign.Current.GetType(), "_kingdoms").SetValue(Campaign.Current, new MBReadOnlyList<Kingdom>(kingdoms));
            }
        }

        public void AddKingdom(Kingdom kingdom)
        {
            this.ModifyKingdomList(kingdoms =>
            {
                if (kingdoms.Contains(kingdom))
                {
                    return null;
                }

                kingdoms.Add(kingdom);
                return kingdoms;
            });

            this.GetInfo(kingdom).IsCustomKingdom = true;
        }

        public void RemoveKingdom(Kingdom kingdom)
        {
            this.ModifyKingdomList(kingdoms =>
            {
                if (kingdoms.RemoveAll(go => go == kingdom) > 0)
                {
                    return kingdoms;
                }

                return null;
            });

            this.RemoveInfo(kingdom.StringId);
        }

        public void DestroyKingdom(Kingdom kingdom)
        {
            DestroyKingdomAction.Apply(kingdom);
            this.RemoveKingdom(kingdom);
        }

        public Kingdom CreateKingdom(Hero leader, TextObject name, TextObject informalName, Banner banner = null, bool showNotification = true)
        {
            var kingdom = MBObjectManager.Instance.CreateObject<Kingdom>();
            kingdom.InitializeKingdom(name, informalName, leader.Culture, banner ?? Banner.CreateRandomClanBanner(leader.StringId.GetDeterministicHashCode()), leader.Clan.Color, leader.Clan.Color2, leader.Clan.InitialPosition);

            ChangeKingdomAction.ApplyByJoinToKingdom(leader.Clan, kingdom, showNotification);
            kingdom.RulingClan = leader.Clan;

            AccessTools.Property(typeof(Kingdom), "AlternativeColor").SetValue(kingdom, leader.Clan.Color);
            AccessTools.Property(typeof(Kingdom), "AlternativeColor2").SetValue(kingdom, leader.Clan.Color2);
            AccessTools.Property(typeof(Kingdom), "LabelColor").SetValue(kingdom, ColorHelper.Black.ToUnsignedInteger());

            this.AddKingdom(kingdom);
            return kingdom;
        }
    }
}