using System;

namespace JohanBos.ScaledActivities.Function.Models
{
    public class Activity
    {
        public string Id { get; set; }

        public int Group { get; internal set; }

        public DateTime StartUtc { get; internal set; }

        public DateTime CommandStartUtc { get; internal set; }
    }
}
