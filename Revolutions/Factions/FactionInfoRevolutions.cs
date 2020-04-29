﻿using System;
using ModLibrary.Factions;

namespace Revolutions.Factions
{
    [Serializable]
    public class FactionInfoRevolutions : FactionInfo
    {
        public FactionInfoRevolutions()
        {

        }

        public FactionInfoRevolutions(FactionInfo factionInfo)
        {
            base.FactionId = factionInfo.FactionId;
            base.InitialTownsCount = factionInfo.InitialTownsCount;
            base.CurrentTownsCount = factionInfo.CurrentTownsCount;
        }

        public string RevoltedSettlementId { get; set; }

        public bool CanRevolt { get; set; } = false;

        public int DaysSinceLastRevolution { get; set; } = 0;

        public bool SuccessfullyRevolted { get; set; } = false;
    }
}