using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console
{
    public class NativeConsoleAdapter : IConsole
    {
        public uint Width => (uint)System.Console.WindowWidth;
        public uint Height => (uint)System.Console.WindowHeight;

        public void Write(string text)
        {
            System.Console.Write(text);
        }

        public void WriteLine(string text)
        {
            System.Console.WriteLine(text);
        }

        public void SetCursorPosition(int left, int top)
        {
            System.Console.SetCursorPosition(left, top);
        }

        public void SetForegroundColor(ConsoleColor? color)
        {
            if (color == null) return;
            System.Console.ForegroundColor = color.Value;
        }

        public void SetBackgroundColor(ConsoleColor? color)
        {
            if (color == null) return;
            System.Console.BackgroundColor = color.Value;
        }

        public void Clear()
        {
            System.Console.Clear();
        }

        public void ResetColor()
        {
            System.Console.ResetColor();
        }

        public void SetTitle(string title)
        {
            System.Console.Title = title;
        }

        public void SetWindowSize(int width, int height)
        {
            System.Console.WindowWidth = width;
            System.Console.WindowHeight = height;
        }

        public (int width, int height) GetWindowSize()
        {
            return (System.Console.WindowWidth, System.Console.WindowHeight);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey();
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return System.Console.ReadKey(intercept);
        }

        public void Beep()
        {
            System.Console.Beep();
        }

        public void Beep(int frequency, int duration)
        {
            if (OperatingSystem.IsWindows())
            {
                System.Console.Beep(frequency, duration);
            }
        }

        public void HideCursor()
        {
            System.Console.CursorVisible = false;
        }

        public void ShowCursor()
        {
            System.Console.CursorVisible = true;
        }

        public void Flush()
        {
            System.Console.Out.Flush();
        }
    }
}
