﻿using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Library;

namespace ModLibrary.Components.Kingdoms
{
    public class KingdomManager<InfoType> : IManager<InfoType, Kingdom> where InfoType : KingdomInfo, new()
    {
        #region Singleton

        private KingdomManager() { }

        static KingdomManager()
        {
            KingdomManager<InfoType>.Instance = new KingdomManager<InfoType>();
        }

        public static KingdomManager<InfoType> Instance { get; private set; }

        #endregion

        #region IManager

        public List<InfoType> Infos { get; set; } = new List<InfoType>();

        public void InitializeInfos()
        {
            foreach (var kingdom in Campaign.Current.Kingdoms)
            {
                this.AddInfo(kingdom, true);
            }
        }

        public InfoType GetInfoById(string id, bool addIfNotFound = true)
        {
            var kingdomInfo = this.Infos.FirstOrDefault(info => info.KingdomId == id);

            if (kingdomInfo != null)
            {
                return kingdomInfo;
            }

            if (!addIfNotFound)
            {
                return null;
            }

            var missingKingdom = this.GetObjectById(id);
            return missingKingdom != null ? this.AddInfo(missingKingdom, true) : null;
        }

        public InfoType GetInfoByObject(Kingdom kingdom, bool addIfNotFound = true)
        {
            return this.GetInfoById(kingdom?.StringId, addIfNotFound);
        }

        public InfoType AddInfo(Kingdom kingdom, bool force = false)
        {
            if (!force)
            {
                var existingKingdomInfo = this.Infos.FirstOrDefault(info => info.KingdomId == kingdom?.StringId);
                if (existingKingdomInfo != null)
                {
                    return existingKingdomInfo;
                }
            }

            var kingdomInfo = (InfoType)Activator.CreateInstance(typeof(InfoType), kingdom);
            this.Infos.Add(kingdomInfo);

            return kingdomInfo;
        }

        public void RemoveInfo(string id)
        {
            this.Infos.RemoveAll(info => info.KingdomId == id);
        }

        public Kingdom GetObjectById(string id)
        {
            return Campaign.Current.Kingdoms.FirstOrDefault(kingdom => kingdom?.StringId == id);
        }

        public Kingdom GetObjectByInfo(InfoType info)
        {
            return this.GetObjectById(info.KingdomId);
        }

        public void UpdateInfos()
        {
            if (this.Infos.Count() == Campaign.Current.Kingdoms.Count())
            {
                return;
            }

            this.Infos.RemoveAll(info => !Campaign.Current.Kingdoms.Any(kingdom => kingdom?.StringId == info.KingdomId));

            foreach (var kingdom in Campaign.Current.Kingdoms.Where(kingdom => !this.Infos.Any(info => info.KingdomId == kingdom?.StringId)))
            {
                this.AddInfo(kingdom, true);
            }
        }

        #endregion

        public void ModifyKingdomList(Action<List<Kingdom>> modificator)
        {
            var kingdoms = new List<Kingdom>(Campaign.Current.Kingdoms.ToList());
            modificator(kingdoms);
            AccessTools.Field(Campaign.Current.GetType(), "_kingdoms").SetValue(Campaign.Current, new MBReadOnlyList<Kingdom>(kingdoms));
        }
    }
}