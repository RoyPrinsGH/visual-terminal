using DeepTek.VisualTerminalFramework.Abstractions;
using DeepTek.VisualTerminalFramework.Console.Graphics.Objects;

namespace DeepTek.VisualTerminalFramework.Console.Graphics
{
    public interface IReceiveEngineStats
    {
        public uint PreviousBufferUpdateCount { get; set; }
        public uint BufferUpdateCount { get; set; }
        public uint WriteFrameTime { get; set; }
    }

    public class EngineStats : IGraphicsObject<CanvasPosition, PixelInfo>, IReceiveEngineStats
    {
        public uint PreviousBufferUpdateCount { get; set; }
        public uint BufferUpdateCount { get; set; }
        public uint WriteFrameTime { get; set; }

        private bool Opened { get; set; } = false;
        public void ToggleOpened()
        {
            Opened = !Opened;
            UpdateText();
        }

        private Line BottomBorder { get; init; }
        private Square Background { get; init; }
        private Text Text { get; init; }

        private string GetStatusText()
        {
            return
                $" Actual Update Count: {PreviousBufferUpdateCount,-5} - " +
                $"Buffer Scheduled Updates Count: {BufferUpdateCount,-5} - " +
                $"Write Frame Time: {WriteFrameTime,-5}ms";
        }

        public EngineStats(IMatrixWindow<CanvasPosition, PixelInfo> window)
        {
            Background = new Square(0, 0, window.Width, 1, character: PixelInfo.NBSP, backgroundColor: ConsoleColor.Red);

            BottomBorder = new Line(
                new CanvasPosition(0, 1, 0),
                new CanvasPosition(window.Width - 1, 1, 0),
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

        public void UpdateText()
        {
            if (Opened)
            {
                Text.Value = GetStatusText();
                Text.TextColor = ConsoleColor.White;
                Text.BackgroundColor = ConsoleColor.Red;
            }
            else
            {
                Text.Value = "V [Engine Stats - Press S to view] V";
                Text.TextColor = ConsoleColor.Red;
                Text.BackgroundColor = ConsoleColor.Black;
            }
        }

        public void Render(IMatrixGraphics<CanvasPosition, PixelInfo> graphics)
        {
            if (Opened)
            {
                Background.Render(graphics);
                BottomBorder.Render(graphics);
            }

            Text.Render(graphics);
        }
    }
}