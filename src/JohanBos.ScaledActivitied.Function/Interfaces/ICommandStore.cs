﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JohanBos.ScaledActivities.Function.Models;

namespace JohanBos.ScaledActivities.Function.Interfaces
{
    public interface ICommandStore
    {
        Task Start(IEnumerable<Command> commands);

        Task<IEnumerable<Command>> View(int? maxMessages);

        Task Clear();
    }
}
