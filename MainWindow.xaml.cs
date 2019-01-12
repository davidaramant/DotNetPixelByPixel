using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace WpfDrawingPlayground
{
    sealed class KeysPressed
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Right;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WriteableBitmap _canvas;
        private Point _position = new Point();
        private readonly KeysPressed _input = new KeysPressed();

        private readonly int _width;
        private readonly int _height;

        public MainWindow()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(PlaygroundImage, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(PlaygroundImage, EdgeMode.Aliased);

            _width = (int)Width;
            _height = (int)Height;
            _canvas = BitmapFactory.New(pixelWidth: _width, pixelHeight: _height);

            PlaygroundImage.Source = _canvas;

            PlaygroundImage.Stretch = Stretch.None;
            PlaygroundImage.HorizontalAlignment = HorizontalAlignment.Left;
            PlaygroundImage.VerticalAlignment = VerticalAlignment.Top;

            _canvas.Clear(Colors.White);

            CompositionTarget.Rendering += GameLoop;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _input.Up = false;
                    break;
                case Key.Down:
                    _input.Down = false;
                    break;
                case Key.Left:
                    _input.Left = false;
                    break;
                case Key.Right:
                    _input.Right = false;
                    break;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    _input.Up = true;
                    break;
                case Key.Down:
                    _input.Down = true;
                    break;
                case Key.Left:
                    _input.Left = true;
                    break;
                case Key.Right:
                    _input.Right = true;
                    break;
            }
        }

        void GameLoop(object sender, EventArgs e)
        {
            Size delta = new Size();
            if (_input.Up)
                delta.Height -= 5;
            else if (_input.Down)
                delta.Height += 5;
            if (_input.Left)
                delta.Width -= 5;
            else if (_input.Right)
                delta.Width += 5;

            _position += delta;
            _position.X = (_position.X + _width) % (int)_canvas.Width;
            _position.Y = (_position.Y + _height) % (int)_canvas.Height;

            DrawPixel(_position);
        }

        void DrawPixel(Point p)
        {
            try
            {
                _canvas.Lock();

                _canvas.SetPixel(x: p.X, y: p.Y, color: Colors.Black);
                _canvas.AddDirtyRect(new Int32Rect(x: p.X, y: p.Y, width: 1, height: 1));
            }
            finally
            {
                _canvas.Unlock();
            }
        }
    }
}
