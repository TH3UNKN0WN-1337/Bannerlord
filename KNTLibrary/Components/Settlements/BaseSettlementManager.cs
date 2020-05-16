﻿using KNTLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace KNTLibrary.Components.Settlements
{
    public class BaseSettlementManager<InfoType> : IBaseManager<InfoType, Settlement> where InfoType : BaseSettlementInfo, new()
    {
        #region Singleton

        private BaseSettlementManager() { }

        static BaseSettlementManager()
        {
            BaseSettlementManager<InfoType>.Instance = new BaseSettlementManager<InfoType>();
        }

        public static BaseSettlementManager<InfoType> Instance { get; private set; }

        #endregion

        #region IManager

        public bool DebugMode { get; set; }

        public HashSet<InfoType> Infos { get; set; } = new HashSet<InfoType>();

        public void InitializeInfos()
        {
            if (this.Infos.Count() == Campaign.Current.Settlements.Count())
            {
                return;
            }

            foreach (var gameObject in Campaign.Current.Settlements)
            {
                this.GetInfo(gameObject);
            }
        }

        public InfoType GetInfo(Settlement gameObject)
        {
            var infos = this.Infos.ToList().Where(i => i.Id == gameObject.StringId);
            if (this.DebugMode && infos.Count() > 1)
            {
                InformationManager.DisplayMessage(new InformationMessage("Revolutions: Multiple Settlements with same Id. Using first one.", ColorHelper.Orange));
                foreach (var duplicatedInfo in infos)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"Name: {duplicatedInfo.Settlement.Name} | StringId: {duplicatedInfo.Id}", ColorHelper.Orange));
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
            var info = this.Infos.FirstOrDefault(i => i.Id == id);
            if (id == null)
            {
                return;
            }

            this.Infos.RemoveWhere(i => i.Id == id);
        }

        public Settlement GetGameObject(string id)
        {
            return Campaign.Current.Settlements.FirstOrDefault(go => go.StringId == id);
        }

        public Settlement GetGameObject(InfoType info)
        {
            return this.GetGameObject(info.Id);
        }

        public void UpdateInfos(bool onlyRemoving = false)
        {
            if (this.Infos.Count == Campaign.Current.Settlements.Count)
            {
                return;
            }

            this.Infos.RemoveWhere(i => !Campaign.Current.Settlements.Any(go => go.StringId == i.Id));

            if (onlyRemoving)
            {
                return;
            }

            foreach (var settlement in Campaign.Current.Settlements.Where(go => !this.Infos.Any(i => i.Id == go.StringId)))
            {
                this.GetInfo(settlement);
            }
        }

        public void CleanupDuplicatedInfos()
        {
            this.Infos.Reverse();
            this.Infos = this.Infos.GroupBy(i => i.Id)
                                   .Select(i => i.First())
                                   .ToHashSet();
            this.Infos.Reverse();
        }

        #endregion
    }
}