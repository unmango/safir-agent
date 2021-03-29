namespace Safir.Agent.Configuration
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal record AgentOptions
    {
        public static AgentOptions Default = new() {
            MaxDepth = 69
        };
        
        public string? Root { get; set; }
        
        public int MaxDepth { get; set; }
    }
}
