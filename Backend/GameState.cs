using System;
using System.Drawing;

namespace Backend
{
    public delegate void DrawPixel(Point p, Color c);

    public sealed class GameState
    {
        private readonly DrawPixel _drawPixel;
        private readonly Size _canvasSize;
        private Point _position = new Point();

        public GameState(DrawPixel drawPixel, Size canvasSize)
        {
            _drawPixel = drawPixel;
            _canvasSize = canvasSize;
        }

        public void Update(KeysPressed input, TimeSpan elapsed)
        {
            const int movement = 1;

            Size delta = new Size();
            if (input.Up)
                delta.Height -= movement;
            else if (input.Down)
                delta.Height += movement;
            if (input.Left)
                delta.Width -= movement;
            else if (input.Right)
                delta.Width += movement;

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