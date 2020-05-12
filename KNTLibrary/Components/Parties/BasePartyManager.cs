﻿using KNTLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ObjectSystem;

namespace KNTLibrary.Components.Parties
{
    public class BasePartyManager<InfoType> /*: IBaseManager<InfoType, PartyBase>*/ where InfoType : BasePartyInfo, new()
    {
        #region Singleton

        private BasePartyManager() { }

        static BasePartyManager()
        {
            BasePartyManager<InfoType>.Instance = new BasePartyManager<InfoType>();
        }

        public static BasePartyManager<InfoType> Instance { get; private set; }

        #endregion

        #region IManager

        public bool DebugMode { get; set; }

        public HashSet<InfoType> Infos { get; set; } = new HashSet<InfoType>();

        public void InitializeInfos()
        {
            if (this.Infos.Count() == Campaign.Current.Parties.Count())
            {
                return;
            }

            foreach (var gameObject in Campaign.Current.Parties)
            {
                this.GetInfo(gameObject);
            }
        }

        public InfoType GetInfo(PartyBase gameObject)
        {
            var infos = this.Infos.Where(i => i.PartyId == gameObject.Id);
            if (this.DebugMode && infos.Count() > 1)
            {
                InformationManager.DisplayMessage(new InformationMessage("Revolutions: Multiple Parties with same Id. Using first one.", ColorHelper.Orange));
                foreach (var duplicatedInfo in infos)
                {
                    InformationManager.DisplayMessage(new InformationMessage($"Name: {duplicatedInfo.Party.Name} | StringId: {duplicatedInfo.PartyId}", ColorHelper.Orange));
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
            var info = this.Infos.FirstOrDefault(i => i.PartyId == id);
            if (id == null)
            {
                return;
            }

            this.Infos.RemoveWhere(i => i.PartyId == id);
        }

        public PartyBase GetGameObject(string id)
        {
            return Campaign.Current.Parties.FirstOrDefault(go => go.Id == id);
        }

        public PartyBase GetGameObject(InfoType info)
        {
            return this.GetGameObject(info.PartyId);
        }

        public void UpdateInfos(bool onlyRemoving = false)
        {
            if (this.Infos.Count == Campaign.Current.Parties.Count)
            {
                return;
            }

            this.Infos.RemoveWhere(i => !Campaign.Current.Parties.Any(go => go.Id == i.PartyId));

            if (onlyRemoving)
            {
                return;
            }

            foreach (var gameObject in Campaign.Current.Parties.Where(go => !this.Infos.Any(i => i.PartyId == go.Id)))
            {
                this.GetInfo(gameObject);
            }
        }

        public void CleanupDuplicatedInfos()
        {
            this.Infos.Reverse();
            this.Infos = this.Infos.GroupBy(i => i.PartyId)
                                   .Select(i => i.First())
                                   .ToHashSet();
            this.Infos.Reverse();
        }

        #endregion

        public MobileParty CreateMobileParty(Hero leader, Vec2 spawnPosition, Settlement homeSettlement, bool addLeaderToRoster, bool addInitialFood = true)
        {
            var mobileParty = MBObjectManager.Instance.CreateObject<MobileParty>();
            mobileParty.Initialize();

            var memberRoster = new TroopRoster
            {
                IsPrisonRoster = false
            };
            var prisonerRoster = new TroopRoster
            {
                IsPrisonRoster = true
            };

            mobileParty.Party.Owner = leader;
            mobileParty.SetAsMainParty();

            if (addLeaderToRoster)
            {
                mobileParty.MemberRoster.AddToCounts(leader.CharacterObject, 1, false, 0, 0, true, -1);
            }

            mobileParty.InitializeMobileParty(new TextObject(leader.CharacterObject.GetName().ToString(), null), memberRoster, prisonerRoster, spawnPosition, 0.0f, 0.0f);

            if (addInitialFood)
            {
                var foodItem = Campaign.Current.Items.Where(item => item.IsFood).GetRandomElement();
                mobileParty.ItemRoster.AddToCounts(foodItem, new Random().Next(100, 300));
            }

            mobileParty.HomeSettlement = homeSettlement;
            mobileParty.Quartermaster = leader;

            this.GetInfo(mobileParty.Party).IsCustomParty = true;
            return mobileParty;
        }

        public TroopRoster GenerateBasicTroopRoster(Hero leader, int amount, bool withTier1 = true, bool withTier2 = true, bool withTier3 = true, bool withTier4 = true)
        {
            var basicUnits = new TroopRoster();

            var basicTroop = leader?.Culture?.BasicTroop;
            if (basicTroop == null)
            {
                return basicUnits;
            }

            basicUnits.AddToCounts(basicTroop, amount);

            foreach (var tier1 in basicTroop?.UpgradeTargets)
            {
                if (tier1 == null)
                {
                    continue;
                }

                if (withTier1)
                {
                    basicUnits.AddToCounts(tier1, amount / 2);
                }

                foreach (var tier2 in tier1?.UpgradeTargets)
                {
                    if (tier2 == null)
                    {
                        continue;
                    }

                    if (withTier2)
                    {
                        basicUnits.AddToCounts(tier2, amount / 4);
                    }

                    foreach (var tier3 in tier2?.UpgradeTargets)
                    {
                        if (tier3 == null)
                        {
                            continue;
                        }

                        if (withTier3)
                        {
                            basicUnits.AddToCounts(tier3, amount / 8);
                        }

                        foreach (var tier4 in tier3?.UpgradeTargets)
                        {
                            if (tier4 == null)
                            {
                                continue;
                            }

                            if (withTier4)
                            {
                                basicUnits.AddToCounts(tier4, amount / 16);
                            }
                        }
                    }
                }
            }

            return basicUnits;
        }

        public TroopRoster GenerateEliteTroopRoster(Hero leader, int amount, bool withTier1 = true, bool withTier2 = true, bool withTier3 = true, bool withTier4 = true)
        {
            var eliteUnits = new TroopRoster();

            var eliteBasicTroop = leader?.Culture?.EliteBasicTroop;
            if (eliteBasicTroop == null)
            {
                return eliteUnits;
            }

            eliteUnits.AddToCounts(leader.Culture.EliteBasicTroop, amount);

            foreach (var tier1 in eliteBasicTroop?.UpgradeTargets)
            {
                if (tier1 == null)
                {
                    continue;
                }

                if (withTier1)
                {
                    eliteUnits.AddToCounts(tier1, amount / 2);
                }

                foreach (var tier2 in tier1?.UpgradeTargets)
                {
                    if (tier2 == null)
                    {
                        continue;
                    }

                    if (withTier2)
                    {
                        eliteUnits.AddToCounts(tier2, amount / 2);
                    }

                    foreach (var tier3 in tier2?.UpgradeTargets)
                    {
                        if (tier3 == null)
                        {
                            continue;
                        }

                        if (withTier3)
                        {
                            eliteUnits.AddToCounts(tier3, amount / 4);
                        }

                        foreach (var tier4 in tier3?.UpgradeTargets)
                        {
                            if (tier4 == null)
                            {
                                continue;
                            }

                            if (withTier4)
                            {
                                eliteUnits.AddToCounts(tier4, amount / 8);
                            }
                        }
                    }
                }
            }

            return eliteUnits;
        }
    }
}