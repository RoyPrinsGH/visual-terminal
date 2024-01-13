using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics.Objects
{
    public class Text(string value = "", uint x = 0, uint y = 0, uint layerIndex = 0, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        : IGraphicsObject<CanvasPosition, PixelInfo>
    {
        public uint X = x;
        public uint Y = y;
        public uint LayerIndex = layerIndex;
        public string Value = value;
        public ConsoleColor TextColor { get; set; } = foregroundColor;
        public ConsoleColor BackgroundColor { get; set; } = backgroundColor;

        public virtual void Render(IMatrixGraphics<CanvasPosition, PixelInfo> graphics)
        {
            graphics.DrawText(
                new CanvasPosition(X, Y, LayerIndex),
                Value,
                new PixelInfo(' ', TextColor, BackgroundColor));
        }
    }
}