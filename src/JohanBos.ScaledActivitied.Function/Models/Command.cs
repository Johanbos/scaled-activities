﻿using System;

namespace JohanBos.ScaledActivities.Function.Models
{
    public class Command
    {
        public string Id { get; set; }

        public int ActivitiesCount { get; set; }

        public DateTime StartUtc { get; internal set; }

        public int ActivitiesGroup { get; internal set; }
    }
}
