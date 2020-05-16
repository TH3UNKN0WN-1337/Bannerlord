﻿using KNTLibrary.Components.Events;
using Revolutions.Components.Base.Characters;
using TaleWorlds.CampaignSystem;

namespace Revolutions.Components.CivilWars.Events.War
{
    internal class WarEventOptionTwo : EventOption
    {
        public WarEventOptionTwo() : base()
        {

        }

        public WarEventOptionTwo(string id, string text) : base(id, text)
        {

        }

        public override void Invoke()
        {
            Managers.Character.GetInfo(Hero.MainHero.CharacterObject).PlotState = PlotState.Considering;
        }
    }
}