using DeepTek.Visual;

namespace VisualTerminalDemo
{
    static class DemoVisualTerminal
    {
        public static void Run()
        {
            new DemoVisualTerminalInstance().Start();
        }
    }

    class DemoVisualTerminalInstance : VisualTerminalInstance<NativeConsoleMemento>
    {
        public DemoVisualTerminalInstance() : base(new NativeConsoleAdapter()) { }

        protected override void OnStart()
        {
            TerminalSettings.SetTitle("Demo Visual Terminal");
            TerminalSettings.SetWindowSize(80, 25);
            TerminalSettings.SetForegroundColor(ConsoleColor.Green);
            TerminalSettings.SetBackgroundColor(ConsoleColor.Black);

            RegisterPeriodicAction(1000, () => { Counter++; Refresh(); });
            RegisterPeriodicAction(1, () => { Graphics.DrawText(0, 1, $"Time: {DateTime.Now.Ticks}"); Refresh(); });
        }

        private int Counter = 0;
        protected override void OnRefresh()
        {
            Graphics.DrawText(0, 0, $"Press ESC to exit, or P to refresh ({Counter})\n");
        }

        protected override void OnKeyPress(ConsoleKeyInfo key)
        {
            if (key.Key == ConsoleKey.Escape)
            {
                Stop();
            }
            if (key.Key == ConsoleKey.P)
            {
                Counter++;
                Refresh();
            }
        }
    }

    class Program
    {
        static int Main(string[] args)
        {
            DemoVisualTerminal.Run();
            return 0;
        }
    }
}