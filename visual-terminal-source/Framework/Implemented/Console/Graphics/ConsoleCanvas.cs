using System.Collections.Concurrent;
using DeepTek.VisualTerminalFramework.Abstractions;

namespace DeepTek.VisualTerminalFramework.Console.Graphics
{
    public struct PixelInfo(char value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
    {
        public char Value = value;
        public ConsoleColor? ForegroundColor = foregroundColor;
        public ConsoleColor? BackgroundColor = backgroundColor;
        public static PixelInfo Clear => new(' ');
    }

    public struct CanvasPosition(uint x = 0, uint y = 0, uint layerIndex = 0)
    {
        public uint X = x;
        public uint Y = y;
        public uint LayerIndex = layerIndex;
    }

    public interface IMatrixCanvas<TPosition, TPixelData>
    {
        void SetPixel(TPosition position, TPixelData pixelData);
        void Clear();
        void RenderToWriter();
    }

    public class ConsoleCanvas(IMatrixWriter matrix) : IMatrixCanvas<CanvasPosition, PixelInfo>
    {
        private readonly IMatrixWriter matrixWriter = matrix;
        private ConcurrentDictionary<CanvasPosition, PixelInfo> UpdateBuffer { get; set; } = [];

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
            CommitBuffer();
            ConvertBufferToClearBuffer();
        }

        private void CommitBuffer()
        {
            foreach ((CanvasPosition position, PixelInfo pixelInfo) in UpdateBuffer)
            {
                if (position.X >= matrixWriter.Width || position.Y >= matrixWriter.Height) continue;
                matrixWriter.SetCursorPosition((int)position.X, (int)position.Y);
                matrixWriter.SetForegroundColor(pixelInfo.ForegroundColor);
                matrixWriter.SetBackgroundColor(pixelInfo.BackgroundColor);

                matrixWriter.Write(pixelInfo.Value.ToString());
            }
        }

        private void ConvertBufferToClearBuffer()
        {
            foreach ((CanvasPosition position, PixelInfo pixelInfo) in UpdateBuffer)
            {
                if (pixelInfo.Value == ' ')
                {
                    UpdateBuffer.Remove(position, out var _);
                }
                else
                {
                    UpdateBuffer[position] = new PixelInfo(' ');
                }
            }
        }
    }
}