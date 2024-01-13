using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics.Objects
{
    public class Line(CanvasPosition start, CanvasPosition end, char character = ' ', ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black) : IGraphicsObject<CanvasPosition, PixelInfo>
    {
        public CanvasPosition Start { get; init; } = start;
        public CanvasPosition End { get; init; } = end;
        public char Character { get; init; } = character;
        public ConsoleColor ForegroundColor { get; init; } = foregroundColor;
        public ConsoleColor BackgroundColor { get; init; } = backgroundColor;

        public void Render(IMatrixGraphics<CanvasPosition, PixelInfo> graphics)
        {
            graphics.DrawLine(Start, End, new PixelInfo(Character, ForegroundColor, BackgroundColor));
        }
    }
}