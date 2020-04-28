﻿using System;
using Revolutions.CampaignBehaviours;
using TaleWorlds.Core;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace Revolutions.Screens.ViewModels
{
    public class ModOptionsViewModel : ViewModel
    {
        private readonly Action m_onClose;

        private ModOptionsData Data => ModOptions.OptionsData;

        #region Revolt Cool down

        [DataSourceProperty] public float SliderRevoltCooldownMinValue => 0f;

        [DataSourceProperty] public float SliderRevoltCooldownMaxValue => 250f;

        private string GetText(string id)
        {
            TextObject textObject = GameTexts.FindText(id);
            return textObject.ToString();
        }

        private float m_revolt_cooldown;

        [DataSourceProperty] public string RevoltCooldownString { get; private set; }

        [DataSourceProperty]
        public float RevoltCooldown
        {
            get => this.m_revolt_cooldown;
            set
            {
                this.SetField(ref this.m_revolt_cooldown, value, nameof(this.RevoltCooldown));
                TextObject textObject = GameTexts.FindText("str_opt_RevoltCooldown");
                textObject.SetTextVariable("COOLDOWN", (int)this.m_revolt_cooldown);

                this.RevoltCooldownString = textObject.ToString();
                this.OnPropertyChanged(nameof(this.RevoltCooldownString));
                this.Data.RevoltCooldownTime = this.m_revolt_cooldown;
            }
        }

        private float m_daysUntilLoyaltyChange;

        [DataSourceProperty]
        public string DoneDesc
        {
            get { return this.GetText("str_rev_Done"); }
        }

        [DataSourceProperty]
        public string OverextensionAffectsPlayerDesc { get { return this.GetText("str_opt_OverextPlayerDesc"); } }

        [DataSourceProperty]
        public string OverextensionDesc { get { return this.GetText("str_opt_OverextDesc"); } }

        [DataSourceProperty]
        public string ImperialLoyaltyMechanicDesc { get { return this.GetText("str_opt_ImpLoyaltyMechDesc"); } }

        [DataSourceProperty]
        public string DaysUntilLoyaltyChangeString { get; private set; }

        [DataSourceProperty]
        public float SliderDaysUntilLoyaltyChangeMinValue => 0f;

        [DataSourceProperty]
        public float SliderDaysUntilLoyaltyChangeMaxValue => 250f;

        [DataSourceProperty]
        public string MinorFactionsEnabledDesc { get { return this.GetText("str_opt_MinFactEnabledDesc"); } }

        [DataSourceProperty]
        public string DebugModeDesc { get { return this.GetText("str_opt_DebugDesc"); } }

        [DataSourceProperty]
        public float DaysUntilLoyaltyChange
        {
            get => this.m_daysUntilLoyaltyChange;
            set
            {
                this.SetField(ref this.m_daysUntilLoyaltyChange, value, nameof(this.DaysUntilLoyaltyChange));
                TextObject textObject = GameTexts.FindText("str_opt_LoyaltyChangeDays");
                textObject.SetTextVariable("DAYS", (int)this.m_daysUntilLoyaltyChange);

                this.DaysUntilLoyaltyChangeString = textObject.ToString();
                this.OnPropertyChanged(nameof(this.DaysUntilLoyaltyChangeString));
                this.Data.DaysUntilLoyaltyChange = (int)this.m_daysUntilLoyaltyChange;
            }
        }

        #endregion

        #region Mechanics

        private bool m_EmpireLoyaltyMechanicsEnabled;
        private bool m_OverextensionMechanicsEnabled;
        private bool m_PlayerAffectedByOverextension;
        private bool m_MinorFactionsEnabled;
        private bool m_DebugModeEnabled;

        [DataSourceProperty]
        public bool EmpireLoyaltyMechanicsEnabled
        {
            get => this.m_EmpireLoyaltyMechanicsEnabled;
            set
            {
                this.SetField(ref this.m_EmpireLoyaltyMechanicsEnabled, value, nameof(this.EmpireLoyaltyMechanicsEnabled));
                ModOptions.OptionsData.EmpireLoyaltyMechanics = this.m_EmpireLoyaltyMechanicsEnabled;
            }
        }

        [DataSourceProperty]
        public bool OverextensionMechanicsEnabled
        {
            get => this.m_OverextensionMechanicsEnabled;
            set
            {
                this.SetField(ref this.m_OverextensionMechanicsEnabled, value, nameof(this.OverextensionMechanicsEnabled));
                ModOptions.OptionsData.OverextensionMechanics = this.m_OverextensionMechanicsEnabled;
            }
        }

        [DataSourceProperty]
        public bool MinorFactionsEnabled
        {
            get => this.m_MinorFactionsEnabled;
            set
            {
                this.SetField(ref this.m_MinorFactionsEnabled, value, nameof(this.MinorFactionsEnabled));
                ModOptions.OptionsData.AllowMinorFactions = this.m_MinorFactionsEnabled;
            }
        }

        [DataSourceProperty]
        public bool DebugModeEnabled
        {
            get => this.m_DebugModeEnabled;
            set
            {
                this.SetField(ref this.m_DebugModeEnabled, value, nameof(this.DebugModeEnabled));
                ModOptions.OptionsData.DebugMode = this.m_DebugModeEnabled;
            }
        }

        [DataSourceProperty]
        public bool PlayerAffectedByOverextension
        {
            get => this.m_PlayerAffectedByOverextension;
            set
            {
                this.SetField(ref this.m_PlayerAffectedByOverextension, value, nameof(this.PlayerAffectedByOverextension));
                ModOptions.OptionsData.PlayerAffectedByOverextension = this.m_PlayerAffectedByOverextension;
            }
        }

        #endregion

        public sealed override void RefreshValues()
        {
            base.RefreshValues();
            this.RevoltCooldown = this.Data.RevoltCooldownTime;
            this.EmpireLoyaltyMechanicsEnabled = this.Data.EmpireLoyaltyMechanics;
            this.OverextensionMechanicsEnabled = this.Data.OverextensionMechanics;
            this.PlayerAffectedByOverextension = this.Data.PlayerAffectedByOverextension;
            this.DaysUntilLoyaltyChange = this.Data.DaysUntilLoyaltyChange;
            this.MinorFactionsEnabled = this.Data.AllowMinorFactions;
            this.DebugModeEnabled = this.Data.DebugMode;
        }

        public ModOptionsViewModel(Action onClose)
        {
            this.m_onClose = onClose;
            this.RefreshValues();
        }

        private void ExitOptionsMenu()
        {
            this.m_onClose?.Invoke();
            ScreenManager.PopScreen();
        }

        private void ExecuteDone()
        {
            this.m_onClose?.Invoke();
        }
    }
}