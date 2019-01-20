using Backend;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGameTexture
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Test2DGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        Color[] _textureData;
        Texture2D _outputTexture;

        GameState _gameState;

        readonly int _width = 640;
        readonly int _height = 480;


        public Test2DGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = _width,
                PreferredBackBufferHeight = _height,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = true,
            };
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            this.TargetElapsedTime = System.TimeSpan.FromSeconds(1 / 60.0);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _textureData = new Color[_width * _height];
            _outputTexture = new Texture2D(_graphics.GraphicsDevice, width: _width, height: _height);

            _gameState = new GameState(DrawPixel, _width, _height);
        }

        private void DrawPixel(int x, int y, byte r, byte g, byte b)
        {
            _textureData[y * _width + x] = new Color(r, g, b, (byte)0);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();
            var input = new KeysPressed
            {
                Up = keyboardState.IsKeyDown(Keys.Up),
                Down = keyboardState.IsKeyDown(Keys.Down),
                Left = keyboardState.IsKeyDown(Keys.Left),
                Right = keyboardState.IsKeyDown(Keys.Right)
            };

            _gameState.Update(input, gameTime.ElapsedGameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            for (int i = 0; i < _width * _height; i++)
            {
                _textureData[i] = Color.White;
            }

            _gameState.Render();

            _outputTexture.SetData(_textureData);

            GraphicsDevice.Textures[0] = null;

            _spriteBatch.Begin(
                sortMode: SpriteSortMode.Immediate,
                blendState: BlendState.Opaque,
                samplerState: SamplerState.PointWrap,
                depthStencilState: DepthStencilState.None,
                rasterizerState: RasterizerState.CullNone);

            _spriteBatch.Draw(
                texture: _outputTexture,
                destinationRectangle: new Rectangle(0, 0, _width, _height),
                color: Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
