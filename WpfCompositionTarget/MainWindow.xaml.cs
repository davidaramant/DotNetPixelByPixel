using System;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WriteableBitmap _canvas;
        private readonly GameState _gameState;
        private readonly KeysPressed _input = new KeysPressed();

        public MainWindow()
        {
            InitializeComponent();

            RenderOptions.SetBitmapScalingMode(PlaygroundImage, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(PlaygroundImage, EdgeMode.Aliased);

            var width = (int)Width;
            var height = (int)Height;
            _canvas = BitmapFactory.New(pixelWidth: width, pixelHeight: height);
            _gameState = new GameState(DrawPixelOnBitmap, new Size(width,height));

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
            _gameState.Update(_input);
            _gameState.Render();
        }

        public void DrawPixelOnBitmap(Point p, Color c)
        {
            try
            {
                _canvas.Lock();

                _canvas.SetPixel(x: p.X, y: p.Y, r: c.R, g: c.G, b:c.B);
                _canvas.AddDirtyRect(new Int32Rect(x: p.X, y: p.Y, width: 1, height: 1));
            }
            finally
            {
                _canvas.Unlock();
            }
        }
    }
}
