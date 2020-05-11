﻿using KNTLibrary.Components.Factions;
using KNTLibrary.Components.Kingdoms;
using KNTLibrary.Components.Clans;
using KNTLibrary.Components.Parties;
using KNTLibrary.Components.Characters;
using KNTLibrary.Components.Settlements;
using Revolutions.Components.BaseComponents.Settlements;
using Revolutions.Components.BaseComponents.Parties;
using Revolutions.Components.BaseComponents.Kingdoms;
using Revolutions.Components.BaseComponents.Factions;
using Revolutions.Components.BaseComponents.Clans;
using Revolutions.Components.BaseComponents.Characters;
using Revolutions.Components.Revolts;

namespace Revolts
{
    public static class RevoltsManagers
    {
        public static BaseKingdomManager<KingdomInfo> Kingdom { get; } = BaseKingdomManager<KingdomInfo>.Instance;

        public static BaseFactionManager<FactionInfo> Faction { get; } = BaseFactionManager<FactionInfo>.Instance;

        public static BaseClanManager<ClanInfo> Clan { get; } = BaseClanManager<ClanInfo>.Instance;

        public static BasePartyManager<PartyInfo> Party { get; } = BasePartyManager<PartyInfo>.Instance;

        public static BaseCharacterManager<CharacterInfo> Character { get; } = BaseCharacterManager<CharacterInfo>.Instance;

        public static BaseSettlementManager<SettlementInfo> Settlement { get; } = BaseSettlementManager<SettlementInfo>.Instance;

        public static RevoltManager Revolt { get; } = RevoltManager.Instance;
    }
}