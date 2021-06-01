namespace JAngine.Rendering.LowLevel
{
    internal interface IGlHandle
    {
        public uint Handle { get; }
    }
    
    internal class GlHandle : IGlHandle
    {
        public uint Handle { get; set; }
    }
}