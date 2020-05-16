﻿using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Engine.Screens;
using TaleWorlds.Localization;

namespace KNTLibrary.Components.Events
{
    [Serializable]
    public class Event
    {
        public Event() { }

        public Event(int id, TextObject description)
        {
            Id = id;
            Description = description;
        }

        public readonly int Id;
        public readonly TextObject Description;
        public readonly List<Option> Options = new List<Option>();

        public void AddOption(string id, TextObject text)
        {
            Options.Add(new Option(id, text));
        }
        
        public void AddOption(Option option)
        {
            Options.Add(option);
        }

        public void Call()
        {
            ScreenManager.PushScreen(new EventScreen(Options, Description));
        }
    }

    [Serializable]
    public class Option
    {
        public Option() { }

        public Option(string id, TextObject text)
        {
            Id = id;
            Text = text;
        }
        
        public string Id;
        public TextObject Text;

        public virtual void Result()
        {
            
        }
    }
}