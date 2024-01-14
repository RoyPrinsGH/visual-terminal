using DeepTek.VisualTerminalFramework.Abstractions;
using DeepTek.VisualTerminalFramework.Console.Graphics.Objects;

namespace DeepTek.VisualTerminalFramework.Console.Graphics
{
    public interface IReceiveEngineStats
    {
        public void UpdateWithEngineStats(uint ActualUpdateCount, uint BufferUpdateCount, uint WriteFrameTime);
    }

    public class EngineStats
        : IGraphicsObject<CanvasPosition, PixelInfo>,
        IReceiveEngineStats,
        IInstallableComponent<CanvasPosition, PixelInfo>
    {
        private VisualTerminal<CanvasPosition, PixelInfo>? Terminal { get; set; }

        private uint ActualUpdateCount { get; set; }
        private uint BufferUpdateCount { get; set; }
        private uint WriteFrameTime { get; set; }

        public bool Visible { get; set; } = true;

        public void UpdateWithEngineStats(uint actualUpdateCount, uint bufferUpdateCount, uint writeFrameTime)
        {
            ActualUpdateCount = actualUpdateCount;
            BufferUpdateCount = bufferUpdateCount;
            WriteFrameTime = writeFrameTime;
        }

        private bool Opened { get; set; } = false;
        public void ToggleOpened()
        {
            if (!Visible) return;
            Opened = !Opened;
            Update(null, EventArgs.Empty);
            Terminal?.Refresh();
        }

        public ConsoleColor Color
        {
            get => Background.BackgroundColor;
            set { Background.BackgroundColor = value; Text.BackgroundColor = value; BottomBorder.BackgroundColor = value; }
        }

        private Line BottomBorder { get; set; }
        private Square Background { get; set; }
        private Text Text { get; set; }

        private string GetStatusText()
        {
            return
                $" Actual Update Count: {ActualUpdateCount,-5} - " +
                $"Buffer Scheduled Updates Count: {BufferUpdateCount,-5} - " +
                $"Write Frame Time: {WriteFrameTime,-5}ms";
        }

        public EngineStats()
        {
            Background = new Square(0, 0, 1, 1, character: PixelInfo.NBSP, backgroundColor: ConsoleColor.Red);

            BottomBorder = new Line(
                new CanvasPosition(0, 1, 0),
                new CanvasPosition(1, 1, 0),
                '─',
                ConsoleColor.White,
                ConsoleColor.Red);

            Text = new Text(
                GetStatusText(),
                x: 0,
                y: 0,
                layerIndex: 0,
                ConsoleColor.White,
                ConsoleColor.Red);
        }

        public void Update(object? sender, EventArgs e)
        {
            if (Opened)
            {
                Text.Value = GetStatusText();
                Text.TextColor = ConsoleColor.White;
                Text.BackgroundColor = Color;
            }
            else
            {
                Text.Value = "V [Engine Stats - Press S to view] V";
                Text.TextColor = ConsoleColor.Red;
                Text.BackgroundColor = ConsoleColor.Black;
            }
        }

        public virtual void Resize(object? sender, (uint newWidth, uint newHeight) dim)
        {
            Background.Width = dim.newWidth;
            BottomBorder.End = new CanvasPosition(dim.newWidth - 1, 1, 0);
        }

        public void Render(IMatrixGraphics<CanvasPosition, PixelInfo> graphics)
        {
            if (!Visible) return;

            if (Opened)
            {
                Background.Render(graphics);
                BottomBorder.Render(graphics);
            }

            Text.Render(graphics);
        }

        public void Install(VisualTerminal<CanvasPosition, PixelInfo> terminal)
        {
            Terminal = terminal;

            Resize(terminal, (terminal.Window.Width, terminal.Window.Height));

            terminal.Objects.Add(this);

            terminal.OnResize += Resize;
            terminal.OnRefresh += Update;

            terminal.RegisterKeyPressAction(ConsoleKey.S, ToggleOpened);
        }
    }
}