using System.Collections.Concurrent;
using System.Diagnostics;
using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics
{
    public struct PixelInfo(char value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        public const char ClearChar = ' ';
        public char Value = value;
        public ConsoleColor? ForegroundColor = foregroundColor;
        public ConsoleColor? BackgroundColor = backgroundColor;
        public static PixelInfo Clear => new(ClearChar, ConsoleColor.White, ConsoleColor.Black);
        public static char NBSP => '\u00A0';
    }

    public struct CanvasPosition(uint x = 0, uint y = 0, uint layerIndex = 0)
    {
        public uint X = x;
        public uint Y = y;
        public uint LayerIndex = layerIndex;
    }

    public class ConsoleCanvas(IMatrixWriter matrix) : IMatrixCanvas<CanvasPosition, PixelInfo>
    {
        private readonly IMatrixWriter matrixWriter = matrix;

        private Dictionary<(uint x, uint y), PixelInfo> Buffer { get; set; } = [];
        private ConcurrentDictionary<CanvasPosition, PixelInfo> UpdateBuffer { get; set; } = [];

        private ConsoleColor DefaultForegroundColor { get; set; } = ConsoleColor.White;
        private ConsoleColor DefaultBackgroundColor { get; set; } = ConsoleColor.Black;

        public void SetDefaultColor(PixelInfo pixelInfo)
        {
            DefaultForegroundColor = pixelInfo.ForegroundColor ?? DefaultForegroundColor;
            DefaultBackgroundColor = pixelInfo.BackgroundColor ?? DefaultBackgroundColor;
        }

        public void SetPixel(CanvasPosition position, PixelInfo pixelInfo)
        {
            UpdateBuffer[position] = pixelInfo;
        }

        public void Clear()
        {
            UpdateBuffer.Clear();
            for (uint x = 0; x < matrixWriter.Width; x++)
            {
                for (uint y = 0; y < matrixWriter.Height; y++)
                {
                    UpdateBuffer[new CanvasPosition(x, y, 0)] = PixelInfo.Clear;
                }
            }
        }

        public void RenderToWriter()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            CommitBuffer();
            ConvertBufferToClearBuffer();
            stopwatch.Stop();
            WriteFrameTime = (uint)stopwatch.ElapsedMilliseconds;
        }

        public uint ActualUpdateCount { get; private set; } = 0;
        public uint BufferUpdateCount => (uint)UpdateBuffer.Count;
        public uint WriteFrameTime { get; private set; } = 0;

        private void CommitBuffer()
        {
            var sortedPositions = UpdateBuffer.OrderBy(kv => kv.Value.Value != PixelInfo.Clear.Value)
                                              .ThenBy(kv => kv.Key.LayerIndex)
                                              .ToArray()
                                              .GroupBy(kv => (kv.Key.X, kv.Key.Y))
                                              .Select(g => g.Last());

            ActualUpdateCount = 0;

            foreach ((CanvasPosition position, PixelInfo pixelInfo) in sortedPositions)
            {
                if (position.X >= matrixWriter.Width || position.Y >= matrixWriter.Height) continue;
                if (Buffer.TryGetValue((position.X, position.Y), out var existingPixelInfo)
                    && existingPixelInfo.Value == pixelInfo.Value
                    && existingPixelInfo.ForegroundColor == pixelInfo.ForegroundColor
                    && existingPixelInfo.BackgroundColor == pixelInfo.BackgroundColor)
                {
                    continue;
                }
                matrixWriter.SetCursorPosition((int)position.X, (int)position.Y);
                matrixWriter.SetForegroundColor(pixelInfo.ForegroundColor ?? DefaultForegroundColor);
                matrixWriter.SetBackgroundColor(pixelInfo.BackgroundColor ?? DefaultBackgroundColor);

                matrixWriter.Write(pixelInfo.Value.ToString());
                Buffer[(position.X, position.Y)] = pixelInfo;
                ActualUpdateCount++;
            }
        }

        private void ConvertBufferToClearBuffer()
        {
            foreach ((CanvasPosition position, PixelInfo pixelInfo) in UpdateBuffer)
            {
                if (pixelInfo.Value == PixelInfo.ClearChar)
                {
                    UpdateBuffer.Remove(position, out var _);
                }
                else
                {
                    UpdateBuffer[position] = PixelInfo.Clear;
                }
            }
        }
    }
}