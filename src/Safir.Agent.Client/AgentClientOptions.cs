using JetBrains.Annotations;

namespace Safir.Agent.Client
{
    [PublicAPI]
    public class AgentClientOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
    }
}
