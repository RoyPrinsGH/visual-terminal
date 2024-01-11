using System.Diagnostics;

namespace DeepTek.Visual
{
    public abstract class VisualTerminalInstance(IMatrixWindow window)
    {
        protected readonly IMatrixWindow Window = window;
        protected IMatrixGraphics Graphics => Window.Graphics;

        private bool KeyListenerEventsActive { get; set; }
        public bool IsActive { get; protected set; }
        private object MonitorPadLock { get; init; } = new object();

        protected abstract void OnRefresh();

        protected virtual void OnStart() { }
        protected virtual void OnStop() { }
        protected virtual void OnKeyPress(ConsoleKeyInfo key) { }

        public void Refresh()
        {
            lock (MonitorPadLock)
            {
                Monitor.Pulse(MonitorPadLock);
            }
        }

        public void Stop()
        {
            IsActive = false;
            Refresh();
        }

        private void PrepareStart()
        {
            IsActive = true;
            KeyListenerEventsActive = true;
            Graphics.Clear();
            Window.HideCursor();
            StartKeyListenerThread();
            StartPeriodicBlockingActionsHandlerThread();
            OnStart();
        }

        private Thread StartPeriodicBlockingActionsHandlerThread()
        {
            Thread thread = new(() =>
            {
                while (IsActive)
                {
                    lock (PeriodicActions)
                    {
                        foreach ((Stopwatch stopwatch, int milliseconds, Action action) in PeriodicActions)
                        {
                            if (stopwatch.ElapsedMilliseconds >= milliseconds)
                            {
                                action();
                                stopwatch.Restart();
                            }
                        }
                    }
                }
            });
            thread.Start();
            return thread;
        }

        private void PrepareStop()
        {
            IsActive = false;
            KeyListenerEventsActive = false;
            Graphics.Clear();
            Window.ShowCursor();
            OnStop();
        }

        private List<Thread> PeriodicActionThreads { get; init; } = [];

        protected void RegisterPeriodicActionOnSeparateThread(int milliseconds, Action action)
        {
            Thread periodicActionThread = new(() =>
            {
                while (IsActive)
                {
                    Thread.Sleep(milliseconds);
                    action();
                }
            })
            {
                IsBackground = true
            };
            periodicActionThread.Start();
            PeriodicActionThreads.Add(periodicActionThread);
        }

        private List<(Stopwatch, int, Action)> PeriodicActions { get; init; } = [];

        protected void RegisterPeriodicAction(int milliseconds, Action action)
        {
            lock (PeriodicActions)
            {
                PeriodicActions.Add((Stopwatch.StartNew(), milliseconds, action));
            }
        }

        public void Start()
        {
            object preRunState = Window.ToMemento();
            PrepareStart();
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (IsActive)
                {
                    OnRefresh();
                    Graphics.Update();

                    if (!IsActive) break;

                    lock (MonitorPadLock)
                    {
                        Monitor.Wait(MonitorPadLock, -1);
                    }

                    stopwatch.Restart();
                }
                stopwatch.Stop();
            }
            finally
            {
                PrepareStop();
                Window.LoadFromMemento(preRunState);
            }
        }

        private Thread StartKeyListenerThread()
        {
            Thread keyListenerThread = new(() =>
            {
                ConsoleKeyInfo key = Window.ReadKey();
                while (KeyListenerEventsActive)
                {
                    OnKeyPress(key);
                    key = Window.ReadKey();
                }
            })
            {
                IsBackground = true
            };
            keyListenerThread.Start();
            return keyListenerThread;
        }
    }
}