﻿using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using Victoria.Entities;

namespace PrincessBot.DataStructs
{
    public class AudioOptions
    {
        public bool Shuffle { get; set; }
        public bool RepeatTrack { get; set; }
        public IUser Summoner { get; set; }
    }
}
