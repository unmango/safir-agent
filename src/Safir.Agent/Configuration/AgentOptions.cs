using System.IO;
using JetBrains.Annotations;

namespace Safir.Agent.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class AgentOptions
    {
        public string? DataDirectory { get; set; }

        public int MaxDepth { get; [UsedImplicitly] set; }
        
        public EnumerationOptions EnumerationOptions { get; set; }
    }
}
