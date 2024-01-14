namespace DeepTek.VisualTerminalFramework.Abstractions
{
    public interface IInstallableComponent<TPosition, TPixelData>
    {
        void Install(VisualTerminal<TPosition, TPixelData> terminal);
    }
}