using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics.Objects
{
    public class Text(string value = "", uint x = 0, uint y = 0, uint layerIndex = 0, ConsoleColor color = ConsoleColor.White)
        : IGraphicsObject<CanvasPosition, PixelInfo>
    {
        public uint X = x;
        public uint Y = y;
        public uint LayerIndex = layerIndex;
        public string Value = value;
        public ConsoleColor Color = color;

        public void Render(IMatrixGraphics<CanvasPosition, PixelInfo> graphics)
        {
            graphics.DrawText(
                new CanvasPosition(X, Y, LayerIndex),
                Value,
                new PixelInfo(' ', foregroundColor: Color));
        }
    }
}