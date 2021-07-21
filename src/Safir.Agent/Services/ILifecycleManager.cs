using System.Collections.Generic;
using System.Threading.Tasks;

namespace Safir.Agent.Services
{
    public interface ILifecycleManager<T>
    {
        IEnumerable<string> Keys { get; }

        Task<T> GetOrCreateAsync(string name);
    }
}
