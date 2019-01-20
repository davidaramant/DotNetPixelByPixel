using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Color = System.Drawing.Color;
using Backend;

namespace WpfCompositionTarget
{
    public partial class MainWindow : Window
    {
        private readonly WriteableBitmap _canvas;
        private readonly GameState _gameState;
        private readonly KeysPressed _input = new KeysPressed();
        private readonly Stopwatch _timer = new Stopwatch();
        private TimeSpan _lastElapsed;

        public MainWindow()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(PlaygroundImage, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(PlaygroundImage, EdgeMode.Aliased);

            var width = (int)PlaygroundImage.Width;
            var height = (int)PlaygroundImage.Height;
            _canvas = BitmapFactory.New(pixelWidth: width, pixelHeight: height);
            _gameState = new GameState(DrawPixel, new Size(width,height));

            PlaygroundImage.Source = _canvas;

            PlaygroundImage.Stretch = Stretch.None;
            PlaygroundImage.HorizontalAlignment = HorizontalAlignment.Left;
            PlaygroundImage.VerticalAlignment = VerticalAlignment.Top;

            _canvas.Clear(Colors.White);

            CompositionTarget.Rendering += GameLoop;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;

            _timer.Start();
            _lastElapsed = _timer.Elapsed;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            // This method avoids any branching
            _input.Up |= e.Key == Key.Up;
            _input.Down |= e.Key == Key.Down;
            _input.Left |= e.Key == Key.Left;
            _input.Right |= e.Key == Key.Right;
        }
        
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            // This method avoids any branching
            _input.Up &= e.Key != Key.Up;
            _input.Down &= e.Key != Key.Down;
            _input.Left &= e.Key != Key.Left;
            _input.Right &= e.Key != Key.Right;
        }

        void GameLoop(object sender, EventArgs e)
        {
            var timeStamp = _timer.Elapsed;
            _gameState.Update(_input, timeStamp - _lastElapsed);
            _lastElapsed = timeStamp;

            try
            {
                _canvas.Lock();
                _canvas.Clear(Colors.White);

                _gameState.Render();
            }
            finally
            {
                _canvas.Unlock();
            }
        }

        public void DrawPixel(Point p, Color c)
        {
            _canvas.SetPixel(x: p.X, y: p.Y, r: c.R, g: c.G, b: c.B);
            // TODO: Is this necessary?
            //_canvas.AddDirtyRect(new Int32Rect(x: p.X, y: p.Y, width: 1, height: 1));
        }
    }
}
