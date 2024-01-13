using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics
{
    public class ConsoleGraphics(IMatrixCanvas<CanvasPosition, PixelInfo> canvas)
        : IMatrixGraphics<CanvasPosition, PixelInfo>
    {
        public IMatrixCanvas<CanvasPosition, PixelInfo> Canvas { get; init; } = canvas;

        public void SetDefaultColor(PixelInfo pixelData)
        {
            Canvas.SetDefaultColor(pixelData);
        }

        public void Reset()
        {
            Canvas.Reset();
        }

        public void Clear()
        {
            Canvas.Clear();
        }

        public void SetPixel(CanvasPosition position, PixelInfo pixelData)
        {
            Canvas.SetPixel(position, pixelData);
        }

        public void DrawLine(CanvasPosition start, CanvasPosition end, PixelInfo pixelData)
        {
            int sx = start.X < end.X ? 1 : -1;
            int sy = start.Y < end.Y ? 1 : -1;
            int dx = (int)(start.X < end.X ? end.X - start.X : start.X - end.X);
            int dy = (int)(start.Y < end.Y ? end.Y - start.Y : start.Y - end.Y);
            int err = dx - dy;

            while (true)
            {
                SetPixel(start, pixelData);

                if (start.X == end.X && start.Y == end.Y)
                {
                    break;
                }

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    start.X = (uint)(start.X + sx);
                }
                if (e2 < dx)
                {
                    err += dx;
                    start.Y = (uint)(start.Y + sy);
                }
            }
        }

        public void DrawRectangle(CanvasPosition topright, CanvasPosition bottomleft, PixelInfo pixelData)
        {
            DrawLine(topright, new CanvasPosition(bottomleft.X, topright.Y, topright.LayerIndex), pixelData);
            DrawLine(new CanvasPosition(bottomleft.X, topright.Y, topright.LayerIndex), bottomleft, pixelData);
            DrawLine(bottomleft, new CanvasPosition(topright.X, bottomleft.Y, topright.LayerIndex), pixelData);
            DrawLine(new CanvasPosition(topright.X, bottomleft.Y, topright.LayerIndex), topright, pixelData);
        }

        public void DrawText(CanvasPosition position, string text, PixelInfo pixelData)
        {
            foreach (char c in text)
            {
                pixelData.Value = c == ' ' ? ' ' : c;
                SetPixel(position, pixelData);
                position.X++;
            }
        }

        public void RenderToScreen()
        {
            Canvas.RenderToWriter();
        }
    }
}