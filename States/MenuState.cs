using CaveCrawler.Controls;
using CaveCrawler.Sprites;
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
    public class MenuState : State
    {
        private List<Component> _components;
        public MenuState(Game1 game, ContentManager content) : base(game, content)
        {

        }

        /// <summary>
        /// The LoadContent method
        /// 
        /// Uses _content from the base State class to load the Button texture and Font
        /// 
        /// Adds 3 Buttons to the Components List: Button to Play, Button to go to Credits, Button to Exit
        /// </summary>
        public override void LoadContent()
        {
            var buttonTexture = _content.Load<Texture2D>("Button");
            var buttonFont = _content.Load<SpriteFont>("Font");

            _components = new List<Component>()
            {
                new Sprite(_content.Load<Texture2D>("Backgrounds/MainMenu"))
                {
                    Layer = 0.0f,
                    Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Play",
                    Position = new Vector2(Game1.ScreenWidth / 2, 400),
                    Click = new EventHandler(Button_Play_Clicked),
                    Layer = 0.1f,
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Credits",
                    Position = new Vector2(Game1.ScreenWidth / 2, 500),
                    Click = new EventHandler(Button_Credits_Clicked),
                    Layer = 0.1f,
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Exit",
                    Position = new Vector2(Game1.ScreenWidth / 2, 600),
                    Click = new EventHandler(Button_Exit_Clicked),
                    Layer = 0.1f,
                },
            };
        }

        /// <summary>
        /// Three methods used for the Click Events for Buttons
        /// 
        /// Button_Play_Clicked - Changes the state to the GameState to start playing
        /// Button_Credits_Clicked - Changes to the CreditsState to display it
        /// Button_Exit_Clicked - Closes the application window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Button_Play_Clicked(object sender, EventArgs args)
        {
            _game.ChangeState(new GameState(_game, _content)
            {
                Health = 6,
                Score = 1,
            });
        }
        private void Button_Credits_Clicked(object sender, EventArgs args)
        {
            _game.ChangeState(new CreditsState(_game, _content));
        }
        private void Button_Exit_Clicked(object sender, EventArgs args)
        {
            _game.Exit();
        }

        /// <summary>
        /// The Update method
        /// 
        /// Loops through every Component (so button) in _components and calls their update method
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        /// <summary>
        /// The PostUpdate Method
        /// 
        /// Required since it is an abstract method in State, but does nothing for the MenuState
        /// </summary>
        /// <param name="gameTime"></param>
        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        /// <summary>
        /// The Draw Method
        /// 
        /// Simply just calls the draw method of each Component using the specified layering values
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack); // A call to the spriteBatch to start drawing using the layers we specify
            
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
