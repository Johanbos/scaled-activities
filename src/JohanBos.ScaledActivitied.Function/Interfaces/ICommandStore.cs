using System.Collections.Generic;
using System.Threading.Tasks;

namespace JohanBos.ScaledActivities.Function.Interfaces
{
    public interface ICommandStore
    {
        Task Start(IEnumerable<string> commands);

        Task<IEnumerable<Models.Command>> View(int? maxMessages);

        Task Clear();
    }
}
