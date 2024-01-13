namespace DeepTek.VisualTerminalFramework.Abstractions
{
    public interface IMatrixWriter
    {
        uint Width { get; }
        uint Height { get; }
        void Write(string text);
        void WriteLine(string text);
        void SetCursorPosition(int left, int top);
        void SetForegroundColor(ConsoleColor? color);
        void SetBackgroundColor(ConsoleColor? color);
        void Clear();
    }
}