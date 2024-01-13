using System.Diagnostics;
using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework
{
    public class VisualTerminal<TPixelPosition, TPixelData>(IMatrixWindow<TPixelPosition, TPixelData> window)
    {
        public readonly IMatrixWindow<TPixelPosition, TPixelData> Window = window;
        public IMatrixGraphics<TPixelPosition, TPixelData> Graphics => Window.Graphics;

        public bool IsActive { get; private set; }

        private bool KeyListenerEventsActive { get; set; }
        private object MonitorPadLock { get; init; } = new object();

        public event EventHandler OnRefresh = delegate { };
        public event EventHandler OnStart = delegate { };
        public event EventHandler OnStop = delegate { };
        public event EventHandler<ConsoleKeyInfo> OnKeyPress = delegate { };

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
            StartPeriodicActionsRunnerThread();
            OnStart?.Invoke(this, EventArgs.Empty);
        }

        private Thread StartPeriodicActionsRunnerThread()
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
            OnStop?.Invoke(this, EventArgs.Empty);
        }

        private List<Thread> PeriodicActionThreads { get; init; } = new List<Thread>();

        public void RegisterPeriodicActionOnSeparateThread(int milliseconds, Action action)
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

        public void RegisterPeriodicAction(int milliseconds, Action action)
        {
            lock (PeriodicActions)
            {
                PeriodicActions.Add((Stopwatch.StartNew(), milliseconds, action));
            }
        }

        public List<IGraphicsObject<TPixelPosition, TPixelData>> Objects { get; init; } = [];

        public void Start()
        {
            if (IsActive) throw new InvalidOperationException("VisualTerminal is already active.");
            PrepareStart();
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                while (IsActive)
                {
                    OnRefresh?.Invoke(this, EventArgs.Empty);

                    Objects.ForEach(graphicsObject =>
                    {
                        graphicsObject.Render(Graphics);
                    });

                    Graphics.RenderToScreen();

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
            }
        }

        private List<(ConsoleKey, Action)> KeyPressActions { get; init; } = [];

        public void RegisterKeyPressAction(ConsoleKey key, Action action)
        {
            KeyPressActions.Add((key, action));
        }

        private Thread StartKeyListenerThread()
        {
            Thread keyListenerThread = new(() =>
            {
                ConsoleKeyInfo key = Window.ReadKey();
                while (KeyListenerEventsActive)
                {
                    OnKeyPress?.Invoke(this, key);
                    KeyPressActions.ForEach(((ConsoleKey key, Action action) keyAction) =>
                    {
                        if (keyAction.key == key.Key)
                        {
                            keyAction.action();
                        }
                    });
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