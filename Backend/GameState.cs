using System;
using System.Drawing;

namespace Backend
{
    public delegate void DrawPixel(Point p, Color c);

    public sealed class GameState
    {
        // Distance from center to side (perpendicularly)
        const int SquareRadius = 4;

        private readonly DrawPixel _drawPixel;
        private readonly Size _canvasSize;
        private Point _centerPosition;


        public GameState(DrawPixel drawPixel, Size canvasSize)
        {
            _drawPixel = drawPixel;
            _canvasSize = canvasSize;

            _centerPosition = new Point(canvasSize.Width / 2, canvasSize.Height / 2);
        }

        public void Update(KeysPressed input, TimeSpan elapsed)
        {
            const double pixelsPerMs = 0.5;

            int movement = (int)(pixelsPerMs * elapsed.TotalMilliseconds);

            Size delta = new Size();
            if (input.Up)
                delta.Height -= movement;
            else if (input.Down)
                delta.Height += movement;
            if (input.Left)
                delta.Width -= movement;
            else if (input.Right)
                delta.Width += movement;

            _centerPosition += delta;
            _centerPosition.X = Math.Max(SquareRadius, Math.Min(_canvasSize.Width - SquareRadius, _centerPosition.X));
            _centerPosition.Y = Math.Max(SquareRadius, Math.Min(_canvasSize.Height - SquareRadius, _centerPosition.Y));
        }

        public void Render()
        {
            for (int yOffset = -SquareRadius; yOffset < SquareRadius; yOffset++)
            {
                for (int xOffset = -SquareRadius; xOffset < SquareRadius; xOffset++)
                {
                    var delta = new Size(xOffset, yOffset);
                    _drawPixel(_centerPosition + delta, Color.Black);
                }
            }
        }
    }
}