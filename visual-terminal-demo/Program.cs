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

    class DemoVisualTerminalInstance : VisualTerminalInstance
    {
        public DemoVisualTerminalInstance() : base(new ConsoleWindow())
        { }

        protected override void OnStart()
        {
            Window.SetTitle("Demo Visual Terminal");

            Graphics.ForegroundColor = ConsoleColor.Green;
            Graphics.BackgroundColor = ConsoleColor.Black;

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