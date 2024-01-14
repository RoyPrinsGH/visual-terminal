using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics.Objects
{
    public class Square(
        uint x = 0,
        uint y = 0,
        uint width = 1,
        uint height = 1,
        uint layerIndex = 0,
        char character = '#',
        ConsoleColor foregroundColor = ConsoleColor.White,
        ConsoleColor backgroundColor = ConsoleColor.Black,
        bool isFilled = true) : IGraphicsObject<CanvasPosition, PixelInfo>
    {
        public uint X { get; set; } = x;
        public uint Y { get; set; } = y;
        public uint Width { get; set; } = width;
        public uint Height { get; set; } = height;
        public uint LayerIndex { get; set; } = layerIndex;
        public char Character { get; set; } = character;
        public ConsoleColor ForegroundColor { get; set; } = foregroundColor;
        public ConsoleColor BackgroundColor { get; set; } = backgroundColor;
        public bool IsFilled { get; set; } = isFilled;
        public bool Visible { get; set; } = true;

        public virtual void Render(IMatrixGraphics<CanvasPosition, PixelInfo> graphics)
        {
            if (!Visible) return;
            if (IsFilled)
            {
                for (uint x = 0; x < Width; x++)
                {
                    for (uint y = 0; y < Height; y++)
                    {
                        graphics.SetPixel(
                            new CanvasPosition(X + x, Y + y, LayerIndex),
                            new PixelInfo(Character, ForegroundColor, BackgroundColor));
                    }
                }
            }
            else
            {
                // Draw only the border
                for (uint x = 0; x < Width; x++)
                {
                    graphics.SetPixel(
                        new CanvasPosition(X + x, Y, LayerIndex),
                        new PixelInfo(Character, ForegroundColor, BackgroundColor));

                    graphics.SetPixel(
                        new CanvasPosition(X + x, Y + Height - 1, LayerIndex),
                        new PixelInfo(Character, ForegroundColor, BackgroundColor));
                }

                for (uint y = 0; y < Height; y++)
                {
                    graphics.SetPixel(
                        new CanvasPosition(X, Y + y, LayerIndex),
                        new PixelInfo(Character, ForegroundColor, BackgroundColor));

                    graphics.SetPixel(
                        new CanvasPosition(X + Width - 1, Y + y, LayerIndex),
                        new PixelInfo(Character, ForegroundColor, BackgroundColor));
                }
            }
        }
    }
}