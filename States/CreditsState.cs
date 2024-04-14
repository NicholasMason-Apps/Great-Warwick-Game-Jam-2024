using CaveCrawler.Controls;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaveCrawler.Sprites;

namespace CaveCrawler.States
{
    public class CreditsState : State
    {
        #region Attributes
        private SpriteFont _font;
        private List<Component> _components;
        #endregion

        #region Methods
        public CreditsState(Game1 game, ContentManager content) : base(game, content)
        {
        }

        public override void LoadContent()
        {
            _font = _content.Load<SpriteFont>("Font");
            var buttonTexture = _content.Load<Texture2D>("Button");
            var buttonFont = _content.Load<SpriteFont>("Font");

            _components = new List<Component>()
            {
                new Sprite(_content.Load<Texture2D>("Backgrounds/CreditsMenu"))
                {
                    Layer = 0f,
                    Position = new Vector2(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2),
                },
                new Button(buttonTexture, buttonFont)
                {
                    Text = "Main Menu",
                    Position = new Vector2(Game1.ScreenWidth / 2, 600),
                    Click = new EventHandler(Button_MainMenu_Clicked),
                    Layer = 0.1f
                },
            };
        }

        private void Button_MainMenu_Clicked(object sender, EventArgs args)
        {
            _game.ChangeState(new MenuState(_game, _content));
        }
        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Credits to:\n" +
                "For Tileset Atlas - https://opengameart.org/content/lpc-tile-atlas:\n      - Lanea Zimmerman\n      - Stephen Challener\n      - Charles Sanchez\n      - Manuel Riecke\n      - Daniel Armstrong\n" +
                "\n\nFor Bat - https://opengameart.org/content/bat-sprite:\n      - bagzie" +
                "\n\n\nFor Sound Effects:" +
                "\n - Footsteps - https://freesound.org/people/FallujahQc/sounds/403168/ \n      - FallujahQc" +
                "\n - Music - https://freesound.org/people/ZHR%C3%98/sounds/685349/\n      - ZHRO" +
                "\n - Sword Sound Effects - https://freesound.org/people/Sam300Productions/\n      - Sam300Productions", new Vector2(400, 100), Color.White);

            spriteBatch.End();
        }
        #endregion
    }
}
