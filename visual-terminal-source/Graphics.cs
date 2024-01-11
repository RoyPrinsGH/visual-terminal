namespace DeepTek.Visual
{
    public class Graphics(ScreenCanvas canvas)
    {
        private ScreenCanvas Canvas { get; init; } = canvas;

        public void DrawLine(uint x1, uint y1, uint x2, uint y2, char pixel)
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

        public void DrawRectangle(uint x, uint y, uint width, uint height, char pixel)
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
                Canvas.SetPixel(x + i, y, text[(int)i]);
            }
        }
    }
}