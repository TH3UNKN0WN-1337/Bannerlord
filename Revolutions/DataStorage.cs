﻿using KNTLibrary.Components.Banners;
using KNTLibrary.Helpers;
using Revolutions.Components.Base.Characters;
using Revolutions.Components.Base.Clans;
using Revolutions.Components.Base.Factions;
using Revolutions.Components.Base.Kingdoms;
using Revolutions.Components.Base.Parties;
using Revolutions.Components.Base.Settlements;
using Revolutions.Components.CivilWars;
using Revolutions.Components.Revolts;
using Revolutions.Settings;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Revolutions
{
    internal class DataStorage
    {
        internal static string ActiveSaveSlotName { get; set; }

        internal static void LoadData()
        {
            DataStorage.LoadBaseData();

            if (RevolutionsSettings.Instance.EnableRevolts)
            {
                DataStorage.LoadRevoltData();
            }

            if (RevolutionsSettings.Instance.EnableCivilWars)
            {
                DataStorage.LoadCivilWarData();
            }
        }

        internal static void SaveData()
        {
            DataStorage.SaveBaseData();

            if (RevolutionsSettings.Instance.EnableRevolts)
            {
                DataStorage.SaveRevoltData();
            }

            if (RevolutionsSettings.Instance.EnableCivilWars)
            {
                DataStorage.SaveCivilWarData();
            }
        }

        private static void LoadBaseData()
        {
            var saveDirectory = DataStorage.GetSaveDirectory();
            if(string.IsNullOrEmpty(saveDirectory))
            {
                Managers.Faction.Infos.Clear();
                Managers.Kingdom.Infos.Clear();
                Managers.Clan.Infos.Clear();
                Managers.Party.Infos.Clear();
                Managers.Character.Infos.Clear();
                Managers.Settlement.Infos.Clear();
            }

            Managers.Faction.Infos = FileHelper.Load<List<FactionInfo>>(saveDirectory, "Factions").ToHashSet();
            if(Managers.Faction.Infos.Count == 0)
            {
                Managers.Faction.InitializeInfos();
            }
            Managers.Faction.CleanupDuplicatedInfos();

            Managers.Kingdom.Infos = FileHelper.Load<List<KingdomInfo>>(saveDirectory, "Kingdoms").ToHashSet();
            if (Managers.Kingdom.Infos.Count == 0)
            {
                Managers.Kingdom.InitializeInfos();
            }
            Managers.Kingdom.CleanupDuplicatedInfos();

            Managers.Clan.Infos = FileHelper.Load<List<ClanInfo>>(saveDirectory, "Clans").ToHashSet();
            if (Managers.Clan.Infos.Count == 0)
            {
                Managers.Clan.InitializeInfos();
            }
            Managers.Clan.CleanupDuplicatedInfos();

            Managers.Party.Infos = FileHelper.Load<List<PartyInfo>>(saveDirectory, "Parties").ToHashSet();
            if (Managers.Party.Infos.Count == 0)
            {
                Managers.Party.InitializeInfos();
            }
            Managers.Party.CleanupDuplicatedInfos();

            Managers.Character.Infos = FileHelper.Load<List<CharacterInfo>>(saveDirectory, "Characters").ToHashSet();
            if (Managers.Character.Infos.Count == 0)
            {
                Managers.Character.InitializeInfos();
            }
            Managers.Character.CleanupDuplicatedInfos();

            Managers.Settlement.Infos = FileHelper.Load<List<SettlementInfo>>(saveDirectory, "Settlements").ToHashSet();
            if (Managers.Settlement.Infos.Count == 0)
            {
                Managers.Settlement.InitializeInfos();
            }
            Managers.Settlement.CleanupDuplicatedInfos();

            Managers.Banner.Infos = FileHelper.Load<List<BaseBannerInfo>>(saveDirectory, "Banners").ToHashSet();
            if (Managers.Banner.Infos.Count == 0)
            {
                Managers.Banner.InitializeInfos();
            }
            Managers.Banner.CleanupDuplicatedInfos();
        }

        private static void SaveBaseData()
        {
            var saveDirectory = DataStorage.GetSaveDirectory();

            FileHelper.Save(Managers.Faction.Infos, saveDirectory, "Factions");
            FileHelper.Save(Managers.Kingdom.Infos, saveDirectory, "Kingdoms");
            FileHelper.Save(Managers.Clan.Infos, saveDirectory, "Clans");
            FileHelper.Save(Managers.Party.Infos, saveDirectory, "Parties");
            FileHelper.Save(Managers.Character.Infos, saveDirectory, "Characters");
            FileHelper.Save(Managers.Settlement.Infos, saveDirectory, "Settlements");
            FileHelper.Save(Managers.Banner.Infos, saveDirectory, "Banners");
        }

        private static void LoadRevoltData()
        {
            var saveDirectory = DataStorage.GetSaveDirectory();
            if(string.IsNullOrEmpty(saveDirectory))
            {
                Managers.Revolt.Revolts.Clear();
            }

            Managers.Revolt.Revolts = FileHelper.Load<List<Revolt>>(saveDirectory, "Revolts").ToHashSet();
        }

        private static void SaveRevoltData()
        {
            var saveDirectory = DataStorage.GetSaveDirectory();

            FileHelper.Save(Managers.Revolt.Revolts, saveDirectory, "Revolts");
        }

        private static void LoadCivilWarData()
        {
            var saveDirectory = DataStorage.GetSaveDirectory();
            if(string.IsNullOrEmpty(saveDirectory))
            {
                Managers.CivilWar.CivilWars.Clear();
            }

            Managers.CivilWar.CivilWars = FileHelper.Load<List<CivilWar>>(saveDirectory, "CivilWars").ToHashSet();
        }

        private static void SaveCivilWarData()
        {
            var saveDirectory = DataStorage.GetSaveDirectory();

            FileHelper.Save(Managers.CivilWar.CivilWars, saveDirectory, "CivilWars");
        }

        private static string GetSaveDirectory()
        {
            if (string.IsNullOrEmpty(DataStorage.ActiveSaveSlotName))
            {
                return string.Empty;
            }

            return Path.Combine(SubModule.BaseSavePath, DataStorage.ActiveSaveSlotName);
        }
    }
}