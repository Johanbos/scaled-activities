using System.Collections.Generic;
using System.Threading.Tasks;
using JohanBos.ScaledActivities.Function.Models;

namespace JohanBos.ScaledActivities.Function.Interfaces
{
    public interface IActivityStore
    {
        Task Add(IEnumerable<Activity> activities);
    }
}
