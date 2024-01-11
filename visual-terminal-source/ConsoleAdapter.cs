namespace DeepTek.Visual
{
    public interface IMemento<TMemento>
    {
        TMemento ToMemento();
        void LoadFromMemento(TMemento memento);
    }

    public interface IConsoleWriter
    {
        void Write(string text);
        void WriteLine(string text);
        void SetCursorPosition(int left, int top);
    }

    public interface IConsoleConfigurator
    {
        void SetForegroundColor(ConsoleColor color);
        void SetBackgroundColor(ConsoleColor color);
        void Clear();
        void ResetColor();
        void SetTitle(string title);
        void SetWindowSize(int width, int height);
        ConsoleKeyInfo ReadKey();
        ConsoleKeyInfo ReadKey(bool intercept);
        void Beep();
        void Beep(int frequency, int duration);
        void HideCursor();
        void ShowCursor();
        void Flush();
    }

    public interface IConfigurableConsole<TMemento> : IConsoleConfigurator, IMemento<TMemento> { }
    public interface IConsole<TMemento> : IConfigurableConsole<TMemento>, IConsoleWriter { }

    public class NativeConsoleMemento(ConsoleColor foregroundColor, ConsoleColor backgroundColor, string title, int windowWidth, int windowHeight, bool cursorVisible)
    {
        public ConsoleColor ForegroundColor { get; init; } = foregroundColor;
        public ConsoleColor BackgroundColor { get; init; } = backgroundColor;
        public string Title { get; init; } = title;
        public int WindowWidth { get; init; } = windowWidth;
        public int WindowHeight { get; init; } = windowHeight;
        public bool CursorVisible { get; init; } = cursorVisible;
    }

    public class NativeConsoleAdapter : IConsole<NativeConsoleMemento>
    {
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

        public NativeConsoleMemento ToMemento()
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

        public void LoadFromMemento(NativeConsoleMemento memento)
        {
            Console.ForegroundColor = memento.ForegroundColor;
            Console.BackgroundColor = memento.BackgroundColor;
            Console.Title = memento.Title;
            Console.WindowWidth = memento.WindowWidth;
            Console.WindowHeight = memento.WindowHeight;
            Console.CursorVisible = memento.CursorVisible;
        }

        public void Flush()
        {
            Console.Out.Flush();
        }
    }
}
