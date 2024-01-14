using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics.Objects
{
    public class Text(string value = "", uint x = 0, uint y = 0, uint layerIndex = 0, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black)
        : IGraphicsObject<CanvasPosition, PixelInfo>
    {
        public uint X { get; set; } = x;
        public uint Y { get; set; } = y;
        public uint LayerIndex { get; set; } = layerIndex;
        public string Value { get; set; } = value;
        public ConsoleColor TextColor { get; set; } = foregroundColor;
        public ConsoleColor BackgroundColor { get; set; } = backgroundColor;
        public bool Visible { get; set; } = true;

        public virtual void Render(IMatrixGraphics<CanvasPosition, PixelInfo> graphics)
        {
            if (!Visible) return;
            graphics.DrawText(
                new CanvasPosition(X, Y, LayerIndex),
                Value,
                new PixelInfo(' ', TextColor, BackgroundColor));
        }
    }
}