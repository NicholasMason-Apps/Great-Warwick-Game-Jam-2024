using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Controls
{
    public class Button : Component
    {
        private MouseState _currentMouse;
        private SpriteFont _font;
        private bool _isHovering;
        private MouseState _previousMouse;
        private Texture2D _texture;

        public EventHandler Click;
        public bool Clicked { get; private set; }
        public float Layer { get; set; } // The layer of the Button sprite on the screen
        public Vector2 Origin // Returns a new centre point within the Button
        {
            get
            {
                return new Vector2(_texture.Width / 2, _texture.Height / 2);
            }
        }

        public Color PenColour { get; set; }
        public Vector2 Position { get; set; } // Position of the Button on the screen
        public Rectangle Rectangle // Rectangle attribute which is used to create a bounding rectangle around the button
        {
            get
            {
                return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
            }
        }

        public string Text;

        /// <summary>
        /// Contructor for Button
        /// </summary>
        /// <param name="texture">The loaded button texture</param>
        /// <param name="font">The font of the text</param>
        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
        }

        /// <summary>
        /// Method to draw the button
        /// 
        /// Draws the base box of the button first and then draws the text in the centre of the button if Text is not empty
        /// If the mouse is hovering over the button, then higlight it
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if (_isHovering)
            {
                colour = Color.Gray;
            }
            spriteBatch.Draw(_texture,
                             Position,
                             null,
                             colour,
                             0f,
                             Origin,
                             new Vector2(1, 1),
                             SpriteEffects.None,
                             Layer);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), PenColour, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, Layer + 0.01f);
            }
        }

        /// <summary>
        /// Update method for Button
        /// 
        /// Updates the MouseState and checks if the mouse is hovering over the button
        /// If the button is Clicked, then Invoke its click event
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }
    }
}
