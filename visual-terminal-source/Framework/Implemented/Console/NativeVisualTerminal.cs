using DeepTek.VisualTerminalFramework.Console.Graphics;

namespace DeepTek.VisualTerminalFramework.Console
{
    public class NativeVisualTerminal : VisualTerminal<CanvasPosition, PixelInfo>
    {
        public NativeVisualTerminal() : base(new ConsoleWindow()) { }
    }
}