namespace DeepTek.Visual
{
    public interface IMemento
    {
        object ToMemento();
        void LoadFromMemento(object memento);
    }

    public interface IMatrixWindow : IMemento
    {
        public IMatrixGraphics Graphics { get; }
        public void SetTitle(string title);
        public ConsoleKeyInfo ReadKey();
        public void ShowCursor();
        public void HideCursor();
    }

    public class ConsoleColorSettingsMemento : IMemento
    {
        public ConsoleColor ForegroundColor { get; init; }
        public ConsoleColor BackgroundColor { get; init; }

        public object ToMemento()
        {
            return new ConsoleColorSettingsMemento
            {
                ForegroundColor = Console.ForegroundColor,
                BackgroundColor = Console.BackgroundColor,
            };
        }

        public void LoadFromMemento(object memento)
        {
            if (memento is not ConsoleColorSettingsMemento consoleWindowSettingsMemento)
            {
                throw new ArgumentException("Memento is not of type ConsoleWindowSettingsMemento.");
            }

            Console.ForegroundColor = consoleWindowSettingsMemento.ForegroundColor;
            Console.BackgroundColor = consoleWindowSettingsMemento.BackgroundColor;
        }
    }

    public class ConsoleWindow : IMatrixWindow
    {
        private IConsole Console { get; init; }
        public IMatrixGraphics Graphics { get; init; }

        public ConsoleWindow()
        {
            Console = new NativeConsoleAdapter();
            Graphics = new MatrixGraphics(new MatrixCanvas(Console));
        }

        public void SetTitle(string title)
        {
            Console.SetTitle(title);
        }

        public object ToMemento()
        {
            return new ConsoleColorSettingsMemento
            {
                ForegroundColor = Graphics.ForegroundColor,
                BackgroundColor = Graphics.BackgroundColor,
            };
        }

        public void LoadFromMemento(object memento)
        {
            if (memento is not ConsoleColorSettingsMemento consoleWindowSettingsMemento)
            {
                throw new ArgumentException("Memento is not of type ConsoleWindowSettingsMemento.");
            }

            Graphics.ForegroundColor = consoleWindowSettingsMemento.ForegroundColor;
            Graphics.BackgroundColor = consoleWindowSettingsMemento.BackgroundColor;
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey(true);
        }

        public void ShowCursor()
        {
            Console.ShowCursor();
        }

        public void HideCursor()
        {
            Console.HideCursor();
        }
    }
}