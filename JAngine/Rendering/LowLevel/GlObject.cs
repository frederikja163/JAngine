using System;

namespace JAngine.Rendering.LowLevel
{
    public abstract class GlObject : IDisposable
    {
        internal uint Handle { get; private set; }
        internal readonly Window Window;

        protected GlObject(Window window, Func<uint> createMethod)
        {
            Window = window;
            Window.Queue(() => Handle = createMethod());
        }

        public abstract void Dispose();
    }
}