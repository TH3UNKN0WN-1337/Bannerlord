﻿using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using KNTLibrary;
using Revolutions.CampaignBehaviors;
using TaleWorlds.Engine;

namespace Revolutions
{
    public class SubModule : MBSubModuleBase
    {
        private DataStorage _dataStorage;

        internal static string BaseSavePath => System.IO.Path.Combine(Utilities.GetConfigsPath(), "Revolutions", "Saves");

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

            InformationManager.DisplayMessage(new InformationMessage("Revolutions: Loaded Mod.", ColorManager.Green));
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarter)
        {
            base.OnGameStart(game, gameStarter);

            if (!(game.GameType is Campaign))
            {
                return;
            }

            this.InitializeMod(gameStarter as CampaignGameStarter);
        }

        private void InitializeMod(CampaignGameStarter campaignGameStarter)
        {
            try
            {
                this._dataStorage = new DataStorage();
                this.AddBehaviours(campaignGameStarter);

                RevolutionsManagers.FactionManager.DebugMode = Settings.Instance.DebugMode;
                RevolutionsManagers.KingdomManager.DebugMode = Settings.Instance.DebugMode;
                RevolutionsManagers.ClanManager.DebugMode = Settings.Instance.DebugMode;
                RevolutionsManagers.PartyManager.DebugMode = Settings.Instance.DebugMode;
                RevolutionsManagers.CharacterManager.DebugMode = Settings.Instance.DebugMode;
                RevolutionsManagers.SettlementManager.DebugMode = Settings.Instance.DebugMode;
            }
            catch (Exception exception)
            {
                InformationManager.DisplayMessage(new InformationMessage("Revolutions: Failed to initialize!", ColorManager.Red));
                InformationManager.DisplayMessage(new InformationMessage(exception.ToString(), ColorManager.Red));
            }
        }

        private void AddBehaviours(CampaignGameStarter campaignGameStarter)
        {
            campaignGameStarter.AddBehavior(new LuckyNationBehaviour());
            campaignGameStarter.AddBehavior(new RevolutionBehavior(ref this._dataStorage));
            campaignGameStarter.AddBehavior(new RevolutionDailyBehavior(ref this._dataStorage));
            campaignGameStarter.AddBehavior(new GuiHandlersBehavior());
            campaignGameStarter.AddBehavior(new CleanupBehavior());

            campaignGameStarter.AddModel(new Models.SettlementLoyaltyModel(ref this._dataStorage));
        }
    }
}