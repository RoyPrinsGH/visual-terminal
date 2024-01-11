namespace DeepTek.Visual
{
    public interface IMatrixGraphics
    {
        void Clear();
        void DrawLine(uint x1, uint y1, uint x2, uint y2, Pixel pixel);
        void DrawRectangle(uint x, uint y, uint width, uint height, Pixel pixel);
        void DrawText(uint x, uint y, string text);
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }
        void Update();
    }

    public class MatrixGraphics(IMatrixCanvas canvas) : IMatrixGraphics
    {
        private IMatrixCanvas Canvas { get; init; } = canvas;

        public void Clear()
        {
            Canvas.Clear();
        }

        public void DrawLine(uint x1, uint y1, uint x2, uint y2, Pixel pixel)
        {
            if (x1 == x2)
            {
                for (uint y = y1; y <= y2; y++)
                {
                    Canvas.SetPixel(x1, y, pixel);
                }
            }
            else if (y1 == y2)
            {
                for (uint x = x1; x <= x2; x++)
                {
                    Canvas.SetPixel(x, y1, pixel);
                }
            }
            else
            {
                throw new ArgumentException("Only horizontal and vertical lines are supported.");
            }
        }

        public void DrawRectangle(uint x, uint y, uint width, uint height, Pixel pixel)
        {
            DrawLine(x, y, x + width, y, pixel);
            DrawLine(x, y, x, y + height, pixel);
            DrawLine(x + width, y, x + width, y + height, pixel);
            DrawLine(x, y + height, x + width, y + height, pixel);
        }

        public void DrawText(uint x, uint y, string text)
        {
            for (uint i = 0; i < text.Length; i++)
            {
                Canvas.SetPixel(x + i, y, new Pixel(text[(int)i]));
            }
        }

        public ConsoleColor ForegroundColor
        {
            get => Console.ForegroundColor;
            set => Console.ForegroundColor = value;
        }

        public ConsoleColor BackgroundColor
        {
            get => Console.BackgroundColor;
            set => Console.BackgroundColor = value;
        }

        public void Update()
        {
            Canvas.Update();
        }
    }
}