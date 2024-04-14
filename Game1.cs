using CaveCrawler.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace CaveCrawler
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public static Random Random;
        public static int ScreenWidth = 1280;
        public static int ScreenHeight = 720;

        private State _currentState;
        private State _nextState;

        private Song _music;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Random = new Random();

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// A public method which can be called to change the state being updated
        /// </summary>
        /// <param name="state"> The state to switch to </param>
        public void ChangeState(State state)
        {
            _nextState = state;
        }

        /// <summary>
        /// Initialising the SpriteBatch
        /// Setting the currentState to the MenuState to start in the Main Menu
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, Content);
            _currentState.LoadContent();

            _music = Content.Load<Song>("Sounds/CaveCrawlerMusic");
            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(_music);
            MediaPlayer.IsRepeating = true;

            _nextState = null;
        }

        /// <summary>
        /// Checks if a new state is wanting to be switched to, and if so loads its content
        /// 
        /// Calls the Update and PostUpdate methods of the current state
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values. (Taken from MonoGame)</param>
        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;
                _currentState.LoadContent();
                _nextState = null;
            }
            if (MediaPlayer.State == MediaState.Stopped)
                MediaPlayer.Play(_music);
            else if (MediaPlayer.State == MediaState.Paused)
                MediaPlayer.Resume();

            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the content of the current state
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(50, 33, 37));

            _currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}
