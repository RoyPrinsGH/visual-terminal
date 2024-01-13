using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics
{
    public class ConsoleWindow : IMatrixWindow<CanvasPosition, PixelInfo>
    {
        private NativeConsoleAdapter Console { get; init; }
        public IMatrixGraphics<CanvasPosition, PixelInfo> Graphics { get; init; }

        public ConsoleWindow()
        {
            Console = new NativeConsoleAdapter();
            Graphics = new ConsoleGraphics(new ConsoleCanvas(Console));
        }

        public uint Width => Console.Width;
        public uint Height => Console.Height;

        public void SetTitle(string title)
        {
            Console.SetTitle(title);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey(true);
        }

        public void ShowCursor()
        {
            Console.ShowCursor();
        }

        public void HideCursor()
        {
            Console.HideCursor();
        }
    }
}