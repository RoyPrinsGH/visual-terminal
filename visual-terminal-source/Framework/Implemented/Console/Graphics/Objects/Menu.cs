using DeepTek.VisualTerminalFramework.Abstractions;
using System;

namespace DeepTek.VisualTerminalFramework.Console.Graphics.Objects
{
    public class Menu(ConsoleKey confirmKey = ConsoleKey.Enter, ConsoleKey upKey = ConsoleKey.UpArrow, ConsoleKey downKey = ConsoleKey.DownArrow)
            : IGraphicsObject<CanvasPosition, PixelInfo>,
        IInstallableComponent<CanvasPosition, PixelInfo>
    {
        public uint X { get; set; } = 0;
        public uint Y { get; set; } = 0;
        public uint LayerIndex { get; set; } = 0;
        public uint Width { get; set; } = 5;
        public uint Height { get; set; } = 5;
        public List<string> Options = [];
        public ConsoleColor TextColor { get; set; } = ConsoleColor.White;
        public ConsoleColor BackgroundColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor SelectedTextColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor SelectedBackgroundColor { get; set; } = ConsoleColor.White;
        public string CursorPrefix { get; set; } = "> ";
        public string CursorSuffix { get; set; } = string.Empty;
        public uint SelectedIndex { get; set; } = 0;
        public bool HasFocus { get; set; } = false;
        public bool Visible { get; set; } = true;

        public ConsoleKey ConfirmKey { get; init; } = confirmKey;
        public ConsoleKey UpKey { get; init; } = upKey;
        public ConsoleKey DownKey { get; init; } = downKey;

        public event EventHandler<uint> OnSelectionChanged = delegate { };
        public event EventHandler<uint> OnSelectionConfirmed = delegate { };

        public void Render(IMatrixGraphics<CanvasPosition, PixelInfo> graphics)
        {
            if (!Visible) return;
            for (uint x = 0; x < Width; x++)
            {
                for (uint y = 0; y < Height; y++)
                {
                    graphics.SetPixel(
                        new CanvasPosition(X + x, Y + y, LayerIndex),
                        new PixelInfo(' ', TextColor, BackgroundColor));
                }
            }

            for (uint i = 0; i < Options.Count; i++)
            {
                var option = Options[(int)i];
                var isSelected = i == SelectedIndex;

                var textColor = isSelected ? SelectedTextColor : TextColor;
                var backgroundColor = isSelected ? SelectedBackgroundColor : BackgroundColor;

                var prefix = isSelected ? CursorPrefix : string.Empty;
                var suffix = isSelected ? CursorSuffix : string.Empty;

                var text = $"{prefix}{option}{suffix}";
                if (text.Length > Width)
                {
                    text = text[..(int)Width];
                }

                graphics.DrawText(
                    new CanvasPosition(X + 1, Y + i + 1, LayerIndex),
                    text,
                    new PixelInfo(' ', textColor, backgroundColor));
            }
        }

        public void Install(VisualTerminal<CanvasPosition, PixelInfo> terminal)
        {
            terminal.Objects.Add(this);

            terminal.RegisterKeyPressAction(UpKey, () =>
            {
                if (SelectedIndex > 0 && HasFocus && Visible)
                {
                    SelectedIndex--;
                    OnSelectionChanged?.Invoke(this, SelectedIndex);
                    terminal.Refresh();
                }
            });

            terminal.RegisterKeyPressAction(DownKey, () =>
            {
                if (SelectedIndex < Options.Count - 1 && HasFocus && Visible)
                {
                    SelectedIndex++;
                    OnSelectionChanged?.Invoke(this, SelectedIndex);
                    terminal.Refresh();
                }
            });

            terminal.RegisterKeyPressAction(ConfirmKey, () =>
            {
                if (HasFocus && Visible)
                {
                    OnSelectionConfirmed?.Invoke(this, SelectedIndex);
                    terminal.Refresh();
                }
            });
        }
    }
}