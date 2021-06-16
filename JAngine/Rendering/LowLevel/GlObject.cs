using System;

namespace JAngine.Rendering.LowLevel
{
    public abstract class GlObject<THandle> : IDisposable
    {
        internal THandle Handle { get; private set; }
        internal readonly Window Window;

        protected GlObject(Window window, Func<THandle> createMethod)
        {
            Window = window;
            Window.Queue(() => Handle = createMethod());
        }

        public abstract void Dispose();
    }
}