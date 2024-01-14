using DeepTek.VisualTerminalFramework;
using DeepTek.VisualTerminalFramework.Abstractions;
using DeepTek.VisualTerminalFramework.Console;
using DeepTek.VisualTerminalFramework.Console.Graphics;
using DeepTek.VisualTerminalFramework.Console.Graphics.Objects;

namespace VisualTerminalDemo
{
    public class ScrollingPressCounterText : Text, IInstallableComponent<CanvasPosition, PixelInfo>
    {
        private uint Counter = 0;
        public void Install(VisualTerminal<CanvasPosition, PixelInfo> terminal)
        {
            terminal.Objects.Add(this);

            Value = "Press spacebar to increase counter";

            terminal.RegisterKeyPressAction(ConsoleKey.Spacebar, () =>
            {
                Counter++;
                Value = $"Spacebar pressed {Counter} times";
                terminal.Refresh();
            });

            terminal.RegisterPeriodicAction(100, () =>
            {
                X = (X + 1) % terminal.Window.Width;
                terminal.Refresh();
            });
        }
    }

    static class DemoVisualTerminal
    {
        public static IEnumerable<IInstallableComponent<CanvasPosition, PixelInfo>> GetObjects(NativeVisualTerminal terminal)
        {
            var es = new EngineStats() { Color = ConsoleColor.DarkBlue };
            yield return es;
            yield return new ScrollingPressCounterText() { X = 0, Y = 4, TextColor = ConsoleColor.Green };
            var menu = new Menu() { X = 0, Y = terminal.Window.Height - 4, Width = 25, Options = ["Toggle enginestats", "---", "---"], HasFocus = true };
            menu.OnSelectionConfirmed += (sender, index) =>
            {
                if (index == 0)
                {
                    es.Visible = !es.Visible;
                }
            };
            yield return menu;
        }

        public static void Run()
        {
            NativeVisualTerminal terminal = new();
            terminal.Window.SetTitle("Visual Terminal Demo");

            terminal.InstallAll(GetObjects(terminal));

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