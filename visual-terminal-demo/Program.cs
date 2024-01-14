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
        public int ScrollDirection { get; set; } = 1;
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
                X = (uint)((terminal.Window.Width + (int)X + ScrollDirection) % terminal.Window.Width);
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

            var spct = new ScrollingPressCounterText() { X = 0, Y = 4, TextColor = ConsoleColor.Green };
            yield return spct;

            var menu = new Menu() { X = 0, Y = terminal.Window.Height - 4, Width = 25, Options = ["Toggle enginestats", "Change scroll direction", "Exit"], HasFocus = true };
            menu.OnSelectionConfirmed += (sender, index) =>
            {
                if (index == 0)
                {
                    es.Visible = !es.Visible;
                }
                else if (index == 1)
                {
                    spct.ScrollDirection *= -1;
                }
                else if (index == 2)
                {
                    terminal.Stop();
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