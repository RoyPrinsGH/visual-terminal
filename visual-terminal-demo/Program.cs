using DeepTek.VisualTerminalFramework.Console;
using DeepTek.VisualTerminalFramework.Console.Graphics;
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

            PressCounterText pct = new(terminal)
            {
                X = 0,
                Y = 5,
                LayerIndex = 1,
                TextColor = ConsoleColor.Green
            };
            terminal.Objects.Add(pct);
            terminal.RegisterPeriodicAction(100, () => { pct.X = (pct.X + 1) % (terminal.Window.Width / 5); terminal.Refresh(); });

            Square sq = new()
            {
                X = 3,
                Y = 3,
                Width = 5,
                Height = 5,
                Character = 'X',
                ForegroundColor = ConsoleColor.Red,
                BackgroundColor = ConsoleColor.Blue,
                IsFilled = false
            };
            terminal.Objects.Add(sq);

            Square sq2 = new()
            {
                X = 5,
                Y = 5,
                Width = 12,
                Height = 6,
                Character = 'O',
                ForegroundColor = ConsoleColor.White,
                IsFilled = true
            };
            terminal.Objects.Add(sq2);

            int counter = 0;
            terminal.RegisterPeriodicAction(16, () => { counter++; sq2.Width = (uint)(12 + Math.Sin((float)counter / 10) * 6); terminal.Refresh(); });

            EngineStats es = new(terminal.Window);
            terminal.Objects.Add(es);
            terminal.OnRefresh += (sender, args) => es.UpdateText();
            terminal.RegisterKeyPressAction(ConsoleKey.S, es.ToggleOpened);

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