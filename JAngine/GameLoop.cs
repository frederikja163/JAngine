using System;
using System.Diagnostics;
using System.Threading;

namespace JAngine
{
    public sealed class GameLoop : IDisposable
    {
        private readonly IContainer<GameLoop> _container;
        public bool IsRunning { get; set; } = true;
        public float DeltaTime { get; private set; }
        private readonly Thread _thread;
        private readonly Action _init;
        private readonly Action _loop;
        private readonly Action? _dispose;

        public GameLoop(IContainer<GameLoop> container, Action init, Action loop, Action? dispose = null)
        {
            _container = container;
            _container.Add(this);

            _thread = new Thread(Run);
            _thread.Start();
            _init = init;
            _loop = loop;
            _dispose = dispose;
        }

        private void Run()
        {
            _init();
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (IsRunning)
            {
                long ticks = stopwatch.ElapsedTicks;
                stopwatch.Restart();
                DeltaTime = ticks / (float) Stopwatch.Frequency;
                
                _loop();
            }
            
            _dispose?.Invoke();
            Dispose();
        }

        public void Dispose()
        {
            if (IsRunning)
            {
                IsRunning = false;
            }
            _container.Remove(this);
        }
    }
}