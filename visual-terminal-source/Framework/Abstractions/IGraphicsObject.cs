namespace DeepTek.VisualTerminalFramework.Abstractions
{
    public interface IGraphicsObject<TPosition, TPixelData>
    {
        void Render(IMatrixGraphics<TPosition, TPixelData> graphics);
    }
}