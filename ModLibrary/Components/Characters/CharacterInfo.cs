﻿using System;
using TaleWorlds.CampaignSystem;

namespace ModLibrary.Components.Characters
{
    [Serializable]
    public class CharacterInfo : IGameComponent<CharacterInfo>
    {
        #region IGameComponent<InfoType>

        public bool Equals(CharacterInfo other)
        {
            return this.CharacterId == other.CharacterId;
        }

        public override bool Equals(object other)
        {
            if (other is CharacterInfo characterInfo)
            {
                return this.CharacterId == characterInfo.CharacterId;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.CharacterId.GetHashCode();
        }

        #endregion

        public CharacterInfo()
        {

        }

        public CharacterInfo(CharacterObject character)
        {
            this.CharacterId = character.Id.InternalValue;
        }

        #region Reference Properties

        public uint CharacterId { get; set; }

        #endregion

        #region Virtual Objects

        #region Reference Properties

        public CharacterObject Character => ModLibraryManagers.CharacterManager.GetGameObject(this.CharacterId);

        #endregion



        #endregion

        #region Normal Properties

        public bool CanJoinOtherKingdoms { get; set; } = true;

        #endregion
    }
}