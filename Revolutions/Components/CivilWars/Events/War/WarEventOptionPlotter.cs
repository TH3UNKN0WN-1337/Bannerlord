﻿using KNTLibrary.Components.Events;
using KNTLibrary.Components.Plots;
using Revolutions.Components.Characters;
using TaleWorlds.CampaignSystem;

namespace Revolutions.Components.CivilWars.Events.War
{
    internal class WarEventOptionPlotter : EventOption
    {
        public WarEventOptionPlotter() : base()
        {

        }

        public WarEventOptionPlotter(string id, string text) : base(id, text)
        {

        }

        public override void Invoke()
        {
            var mainHeroInfo = Managers.Character.Get(Hero.MainHero.CharacterObject);
            mainHeroInfo.PlotState = PlotState.IsPlotting;
            mainHeroInfo.DecisionMade = DecisionMade.Yes;
        }
    }
}