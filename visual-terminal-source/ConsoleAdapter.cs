namespace DeepTek.Visual
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
        void SetForegroundColor(ConsoleColor color);
        void SetBackgroundColor(ConsoleColor color);
        void ResetColor();
        void SetTitle(string title);
        void SetWindowSize(int width, int height);
        (int width, int height) GetWindowSize();
        void HideCursor();
        void ShowCursor();
    }

    public interface IStatefulConsoleConfigurator : IConsoleConfigurator, IMemento { }
    public interface IConsole : IStatefulConsoleConfigurator, IConsoleWriter, IInput { }

    public class NativeConsoleMemento(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string title, int windowWidth, int windowHeight, bool cursorVisible)
    {
        public ConsoleColor ForegroundColor { get; init; } = foregroundColor;
        public ConsoleColor BackgroundColor { get; init; } = backgroundColor;
        public string Title { get; init; } = title;
        public int WindowWidth { get; init; } = windowWidth;
        public int WindowHeight { get; init; } = windowHeight;
        public bool CursorVisible { get; init; } = cursorVisible;
    }

    public class NativeConsoleAdapter : IConsole
    {
        public uint Width => (uint)Console.WindowWidth;
        public uint Height => (uint)Console.WindowHeight;

        public void Write(string text)
        {
            Console.Write(text);
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }

        public void SetCursorPosition(int left, int top)
        {
            Console.SetCursorPosition(left, top);
        }

        public void SetForegroundColor(ConsoleColor color)
        {
            Console.ForegroundColor = color;
        }

        public void SetBackgroundColor(ConsoleColor color)
        {
            Console.BackgroundColor = color;
        }

        public void Clear()
        {
            Console.Clear();
        }

        public void ResetColor()
        {
            Console.ResetColor();
        }

        public void SetTitle(string title)
        {
            Console.Title = title;
        }

        public void SetWindowSize(int width, int height)
        {
            Console.WindowWidth = width;
            Console.WindowHeight = height;
        }

        public (int width, int height) GetWindowSize()
        {
            return (Console.WindowWidth, Console.WindowHeight);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey();
        }

        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Console.ReadKey(intercept);
        }

        public void Beep()
        {
            Console.Beep();
        }

        public void Beep(int frequency, int duration)
        {
            if (OperatingSystem.IsWindows())
            {
                Console.Beep(frequency, duration);
            }
        }

        public void HideCursor()
        {
            Console.CursorVisible = false;
        }

        public void ShowCursor()
        {
            Console.CursorVisible = true;
        }

        public object ToMemento()
        {
            if (OperatingSystem.IsWindows())
            {
                return new NativeConsoleMemento(Console.ForegroundColor, Console.BackgroundColor, Console.Title, Console.WindowWidth, Console.WindowHeight, Console.CursorVisible);
            }
            else
            {
                return new NativeConsoleMemento(Console.ForegroundColor, Console.BackgroundColor, string.Empty, Console.WindowWidth, Console.WindowHeight, false);
            }
        }

        public void LoadFromMemento(object memento)
        {
            if (memento is NativeConsoleMemento m)
            {
                Console.ForegroundColor = m.ForegroundColor;
                Console.BackgroundColor = m.BackgroundColor;
                Console.Title = m.Title;
                Console.WindowWidth = m.WindowWidth;
                Console.WindowHeight = m.WindowHeight;
                Console.CursorVisible = m.CursorVisible;
            }
        }

        public void Flush()
        {
            Console.Out.Flush();
        }
    }
}
