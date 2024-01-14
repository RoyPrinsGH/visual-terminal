using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics.Objects
{
    public class Line(CanvasPosition start, CanvasPosition end, char character = ' ', ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black) : IGraphicsObject<CanvasPosition, PixelInfo>
    {
        public CanvasPosition Start { get; set; } = start;
        public CanvasPosition End { get; set; } = end;
        public char Character { get; set; } = character;
        public ConsoleColor ForegroundColor { get; set; } = foregroundColor;
        public ConsoleColor BackgroundColor { get; set; } = backgroundColor;
        public bool Visible { get; set; } = true;

        public void Render(IMatrixGraphics<CanvasPosition, PixelInfo> graphics)
        {
            if (!Visible) return;
            graphics.DrawLine(Start, End, new PixelInfo(Character, ForegroundColor, BackgroundColor));
        }
    }
}