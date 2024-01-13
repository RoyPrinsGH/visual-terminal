using DeepTek.VisualTerminalFramework;
using DeepTek.VisualTerminalFramework.Console.Graphics;
using DeepTek.VisualTerminalFramework.Console.Graphics.Objects;

namespace VisualTerminalDemo
{
    public class PressCounterText : Text
    {
        private uint Counter = 0;
        public PressCounterText(VisualTerminal<CanvasPosition, PixelInfo> terminal)
        {
            Value = "Press spacebar to increase counter";

            terminal.RegisterKeyPressAction(ConsoleKey.Spacebar, () =>
            {
                Counter++;
                Value = $"Spacebar pressed {Counter} times";
                X = (X + 1) % terminal.Window.Width;
                terminal.Refresh();
            });
        }
    }
    static class DemoVisualTerminal
    {
        public static void Run()
        {
            VisualTerminal<CanvasPosition, PixelInfo> terminal = new(new ConsoleWindow());
            terminal.Window.SetTitle("Visual Terminal Demo");

            PressCounterText pct = new(terminal);
            terminal.Objects.Add(pct);
            terminal.RegisterPeriodicAction(100, () => { pct.X = (pct.X + 1) % terminal.Window.Width; terminal.Refresh(); });

            terminal.RegisterKeyPressAction(ConsoleKey.Escape, terminal.Stop);
            terminal.Start();
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