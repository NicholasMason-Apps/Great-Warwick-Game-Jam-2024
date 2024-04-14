using CaveCrawler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.States
{
    public abstract class State
    {
        protected Game1 _game; // The Game1 instance
        protected ContentManager _content; // ContentManager instance

        /// <summary>
        /// Constructor for State()
        /// </summary>
        /// <param name="game"></param>
        /// <param name="content"></param>
        public State(Game1 game, ContentManager content)
        {
            _game = game;

            _content = content;
        }

        // The LoadContent method to be implemented by future state classes which allows for Assets and Content to be loaded into memory
        public abstract void LoadContent();

        // The Update method to be implemented by future state classes
        public abstract void Update(GameTime gameTime);

        // The PostUpdate method to be implemented by future state classes. This is ran after Update()
        public abstract void PostUpdate(GameTime gameTime);

        // The Draw method used to display sprites on the screen
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}