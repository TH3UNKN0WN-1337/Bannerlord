﻿using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem;

namespace Revolutions.Components.CivilWars
{
    internal class CivilWarManager
    {
        #region Singleton

        static CivilWarManager()
        {
            Instance = new CivilWarManager();
        }

        internal static CivilWarManager Instance { get; private set; }

        #endregion

        internal HashSet<CivilWar> CivilWars = new HashSet<CivilWar>();

        internal CivilWar GetCivilWarByKingdomId(string id)
        {
            return this.CivilWars.FirstOrDefault(i => i.KingdomId == id);
        }

        internal CivilWar GetCivilWarByKingdom(Kingdom kingdom)
        {
            return this.GetCivilWarByKingdomId(kingdom.StringId);
        }

        internal CivilWar GetCivilWarByClanId(string id)
        {
            return this.CivilWars.FirstOrDefault(i => i.ClanIds.Any(cid => cid == id));
        }

        internal CivilWar GetCivilWarByClan(Clan clan)
        {
            return this.GetCivilWarByClanId(clan.StringId);
        }
    }
}