using System;

namespace JAngine.Rendering.LowLevel
{
    public abstract class GlObject<THandle> : IDisposable
    {
        public delegate THandle CreateDelegate();
        public delegate void DeleteDelegate(in THandle handle);

        internal THandle Handle { get; private set; }
        internal readonly Window Window;
        private readonly DeleteDelegate DeleteMethod;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected GlObject(Window window, CreateDelegate createMethod, DeleteDelegate deleteMethod)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Window = window;
            Window.Queue(() => Handle = createMethod());
            DeleteMethod = deleteMethod;
        }

        public virtual void Dispose()
        {
            Window.Queue(() => DeleteMethod(Handle));
        }
    }
}