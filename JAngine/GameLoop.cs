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
        private readonly Action<float> _loop;
        private readonly Action? _dispose;

        public GameLoop(IContainer<GameLoop> container, Action init, Action<float> loop, Action? dispose = null)
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
                do
                {
                    long ticks = stopwatch.ElapsedTicks;
                    DeltaTime = ticks / (float) Stopwatch.Frequency;
                } while (DeltaTime < 1 / 60f);
                stopwatch.Restart();
                
                _loop(DeltaTime);
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