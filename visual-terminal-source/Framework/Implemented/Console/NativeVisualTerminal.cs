using DeepTek.VisualTerminalFramework.Console.Graphics;

namespace DeepTek.VisualTerminalFramework.Console
{
    public class NativeVisualTerminal : VisualTerminal<CanvasPosition, PixelInfo>
    {
        public NativeVisualTerminal() : base(new ConsoleWindow())
        {
            OnStop += (sender, args) =>
            {
                System.Console.ResetColor();
                System.Console.Clear();
                System.Console.CursorVisible = true;
            };

            OnRefresh += (sender, args) =>
            {
                Objects.ForEach(o =>
                {
                    if (o is IReceiveEngineStats engineStats)
                    {
                        var canvas = (ConsoleCanvas)((ConsoleGraphics)Graphics).Canvas;
                        engineStats.UpdateWithEngineStats(canvas.ActualUpdateCount, canvas.BufferUpdateCount, canvas.WriteFrameTime);
                    }
                });
            };
        }
    }
}