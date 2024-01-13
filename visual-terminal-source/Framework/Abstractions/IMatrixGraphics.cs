namespace DeepTek.VisualTerminalFramework.Abstractions
{
    public interface IMatrixGraphics<in TPosition, in TPixelData>
    {
        void Reset();
        void Clear();
        void SetDefaultColor(TPixelData pixelData);
        void SetPixel(TPosition position, TPixelData pixelData);
        void DrawLine(TPosition start, TPosition end, TPixelData pixelData);
        void DrawRectangle(TPosition topright, TPosition bottomleft, TPixelData pixelData);
        void DrawText(TPosition position, string text, TPixelData pixelData);
        void RenderToScreen();
    }
}