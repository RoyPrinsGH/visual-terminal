namespace DeepTek.VisualTerminalFramework.Abstractions
{
    public interface IInput
    {
        ConsoleKeyInfo ReadKey();
        ConsoleKeyInfo ReadKey(bool intercept);
    }

    public interface IConsoleWriter : IMatrixWriter
    {
        void Flush();
        void Beep();
        void Beep(int frequency, int duration);
    }

    public interface IConsoleConfigurator
    {
        void ResetColor();
        void SetTitle(string title);
        void SetWindowSize(int width, int height);
        (int width, int height) GetWindowSize();
        void HideCursor();
        void ShowCursor();
    }

    public interface IStatefulConsoleConfigurator : IConsoleConfigurator { }
    public interface IConsole : IStatefulConsoleConfigurator, IConsoleWriter, IInput { }
}