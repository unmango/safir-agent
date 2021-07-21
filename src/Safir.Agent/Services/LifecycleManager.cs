using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Safir.Agent.Services
{
    public abstract class LifecycleManager<T> : ILifecycleManager<T>
    {
        private readonly ConcurrentDictionary<string, T> _cache = new();

        public IEnumerable<string> Keys => _cache.Keys;

        public async Task<T> GetOrCreateAsync(string name)
        {
            if (!_cache.TryGetValue(name, out var value))
            {
                _cache[name] = value = await CreateAsync(name);
            }
             
            PostGetOrCreate(value);

            return value;
        }

        protected abstract Task<T> CreateAsync(string name);
        
        protected virtual void PostGetOrCreate(T value) { }
    }
}
