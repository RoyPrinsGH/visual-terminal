namespace DeepTek.VisualTerminalFramework.Abstractions
{
    public interface IMatrixCanvas<TPosition, TPixelData>
    {
        void SetDefaultColor(TPixelData pixelData);
        void SetPixel(TPosition position, TPixelData pixelData);
        void Clear();
        void RenderToWriter();
    }
}