namespace DeepTek.Visual
{
    public interface IMatrixWriter
    {
        uint Width { get; }
        uint Height { get; }
        void Write(string text);
        void WriteLine(string text);
        void SetCursorPosition(int left, int top);
        void SetForegroundColor(ConsoleColor color);
        void SetBackgroundColor(ConsoleColor color);
        void Clear();
    }

    public class Pixel
    {
        public char Value { get; set; }
        public ConsoleColor? ForegroundColor { get; set; }
        public ConsoleColor? BackgroundColor { get; set; }

        public Pixel(char value, ConsoleColor? foregroundColor = null, ConsoleColor? backgroundColor = null)
        {
            Value = value;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }
    }

    public interface IMatrixCanvas
    {
        void SetPixel(uint x, uint y, Pixel pixel, int layer = 0);
        Pixel? GetPixel(uint x, uint y, int layer = 0);
        void Clear(int layer = 0);
        void Update();
    }

    public class MatrixCanvas : IMatrixCanvas
    {
        private readonly IMatrixWriter matrixWriter;
        private Dictionary<int, Pixel[,]> Layers { get; set; } = new Dictionary<int, Pixel[,]>();
        private List<(uint x, uint y, int layer)> InvalidatedCoordinates { get; set; } = new List<(uint x, uint y, int layer)>();

        public MatrixCanvas(IMatrixWriter matrix)
        {
            matrixWriter = matrix;
        }

        public void SetPixel(uint x, uint y, Pixel pixel, int layer = 0)
        {
            if (!Layers.ContainsKey(layer))
            {
                Layers[layer] = new Pixel[matrixWriter.Width, matrixWriter.Height];
            }

            Layers[layer][x, y] = pixel;

            lock (InvalidatedCoordinates)
            {
                InvalidatedCoordinates.Add((x, y, layer));
            }
        }

        public Pixel? GetPixel(uint x, uint y, int layer = 0)
        {
            if (Layers.ContainsKey(layer))
            {
                return Layers[layer][x, y];
            }

            return null;
        }

        public void Clear(int layer = 0)
        {
            if (Layers.ContainsKey(layer))
            {
                Pixel[,] layerPixels = Layers[layer];

                for (uint x = 0; x < layerPixels.GetLength(0); x++)
                {
                    for (uint y = 0; y < layerPixels.GetLength(1); y++)
                    {
                        layerPixels[x, y] = new Pixel(' ');
                    }
                }
            }
        }

        private void Commit()
        {
            lock (InvalidatedCoordinates)
            {
                InvalidatedCoordinates.Clear();
            }
        }

        private (uint x, uint y, Pixel pixel)[] GetInvalidatedPixels()
        {
            (uint x, uint y, Pixel pixel)[] differences;

            lock (InvalidatedCoordinates)
            {
                differences = InvalidatedCoordinates.Select(coord =>
                {
                    int layer = Layers.Keys.Where(l => Layers[l][coord.x, coord.y] != null).Max();
                    return (coord.x, coord.y, Layers[layer][coord.x, coord.y]);
                }).ToArray();
            }

            return differences;
        }

        public void Update()
        {
            (uint x, uint y, Pixel pixel)[] updates = GetInvalidatedPixels();

            foreach ((uint x, uint y, Pixel pixel) in updates)
            {
                matrixWriter.SetCursorPosition((int)x, (int)y);

                if (pixel.ForegroundColor.HasValue)
                {
                    matrixWriter.SetForegroundColor(pixel.ForegroundColor.Value);
                }

                if (pixel.BackgroundColor.HasValue)
                {
                    matrixWriter.SetBackgroundColor(pixel.BackgroundColor.Value);
                }

                matrixWriter.Write(pixel.Value.ToString());
            }

            Commit();
        }
    }
}