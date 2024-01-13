namespace DeepTek.VisualTerminalFramework.Abstractions
{
    public interface IMatrixWindow<TPixelPosition, TPixelData>
    {
        public IMatrixGraphics<TPixelPosition, TPixelData> Graphics { get; }
        public uint Width { get; }
        public uint Height { get; }
        public void SetTitle(string title);
        public ConsoleKeyInfo ReadKey();
        public void ShowCursor();
        public void HideCursor();
    }
}