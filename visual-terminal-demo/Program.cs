using DeepTek.VisualTerminalFramework.Console;
using DeepTek.VisualTerminalFramework.Console.Graphics.Objects;

namespace VisualTerminalDemo
{
    public class PressCounterText : Text
    {
        private uint Counter = 0;
        public PressCounterText(NativeVisualTerminal terminal)
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
            NativeVisualTerminal terminal = new();
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