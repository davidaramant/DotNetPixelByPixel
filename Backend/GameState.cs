using System;
using System.Drawing;

namespace Backend
{
    public delegate void DrawPixelOnCanvas(Point p, Color c);

    public sealed class GameState
    {
        private readonly DrawPixelOnCanvas _drawPixel;
        private readonly Size _canvasSize;
        private Point _position = new Point();

        public GameState(DrawPixelOnCanvas drawPixel, Size canvasSize)
        {
            _drawPixel = drawPixel;
            _canvasSize = canvasSize;
        }

        public void Update(KeysPressed input)
        {
            Size delta = new Size();
            if (input.Up)
                delta.Height -= 5;
            else if (input.Down)
                delta.Height += 5;
            if (input.Left)
                delta.Width -= 5;
            else if (input.Right)
                delta.Width += 5;

            _position += delta;
            _position.X = (_position.X + _canvasSize.Width) % _canvasSize.Width;
            _position.Y = (_position.Y + _canvasSize.Height) % _canvasSize.Height;
        }

        public void Render()
        {
            _drawPixel(_position, Color.Black);
        }
    }
}