﻿using KNTLibrary.Components.Events;
using KNTLibrary.Components.Plots;
using Revolutions.Components.Characters;
using TaleWorlds.CampaignSystem;

namespace Revolutions.Components.CivilWars.Events.Plotting
{
    internal class PlottingEventOptionUndecided : EventOption
    {
        public PlottingEventOptionUndecided() : base()
        {

        }

        public PlottingEventOptionUndecided(string id, string text) : base(id, text)
        {

        }

        public override void Invoke()
        {
            var mainHeroInfo = Managers.Character.Get(Hero.MainHero.CharacterObject);
            mainHeroInfo.PlotState = PlotState.Considering;
            mainHeroInfo.DecisionMade = DecisionMade.No;
        }
    }
}