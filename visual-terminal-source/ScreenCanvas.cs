namespace DeepTek.Visual
{
    public class ScreenCanvas(uint width, uint height)
    {
        private char[,] ShownCanvas { get; set; } = new char[width, height];
        private char[,] QueuedForDrawingCanvas { get; set; } = new char[width, height];

        public void Resize(uint width, uint height)
        {
            ShownCanvas = new char[width, height];
            QueuedForDrawingCanvas = new char[width, height];
        }

        public void SetPixel(uint x, uint y, char pixel)
        {
            if (x >= 0 && x < QueuedForDrawingCanvas.GetLength(0) && y >= 0 && y < QueuedForDrawingCanvas.GetLength(1))
            {
                QueuedForDrawingCanvas[x, y] = pixel;
            }
        }

        public char GetPixel(uint x, uint y)
        {
            return QueuedForDrawingCanvas[x, y];
        }

        public void Clear()
        {
            for (uint x = 0; x < QueuedForDrawingCanvas.GetLength(0); x++)
            {
                for (uint y = 0; y < QueuedForDrawingCanvas.GetLength(1); y++)
                {
                    QueuedForDrawingCanvas[x, y] = ' ';
                }
            }
        }

        public (uint x, uint y, char pixel)[] GetDifferences()
        {
            List<(uint x, uint y, char pixel)> differences = [];

            for (uint x = 0; x < QueuedForDrawingCanvas.GetLength(0); x++)
            {
                for (uint y = 0; y < QueuedForDrawingCanvas.GetLength(1); y++)
                {
                    if (QueuedForDrawingCanvas[x, y] != ShownCanvas[x, y])
                    {
                        differences.Add((x, y, QueuedForDrawingCanvas[x, y]));
                    }
                }
            }

            return [.. differences];
        }

        public void Commit()
        {
            for (uint x = 0; x < QueuedForDrawingCanvas.GetLength(0); x++)
            {
                for (uint y = 0; y < QueuedForDrawingCanvas.GetLength(1); y++)
                {
                    ShownCanvas[x, y] = QueuedForDrawingCanvas[x, y];
                }
            }
        }

        public void Draw(IConsoleWriter console)
        {
            (uint x, uint y, char pixel)[] differences = GetDifferences();

            foreach ((uint x, uint y, char pixel) in differences)
            {
                console.SetCursorPosition((int)x, (int)y);
                console.Write(pixel.ToString());
            }

            Commit();
        }
    }
}