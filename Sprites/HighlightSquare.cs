using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Sprites
{
    public class HighlightSquare : BoundingBox
    {
        private Vector2 _offset = new Vector2(16, -3);

        public HighlightSquare(Texture2D texture) : base(texture)
        {
        }

        public override void Update(GameTime gameTime)
        {
            var mouse = Mouse.GetState();
            var velocity = Vector2.Zero;
            /*
            var xDistance = Math.Abs(Math.Abs(Position.X) - Math.Abs(Parent.Position.X));
            var yDistance = Math.Abs(Math.Abs(Position.Y) - Math.Abs(Parent.Position.Y));

            if (Parent != null)
            {
                if (xDistance > 32)
                {
                    if (Position.X < Parent.Position.X)
                    {
                        velocity.X += 32;
                    } else
                    {
                        velocity.X -= 32;  
                    }
                }
                else if (yDistance > 32)
                {
                    if (Position.Y < Parent.Position.Y)
                    {
                        velocity.Y += 32;
                    }
                    else
                    {
                        velocity.Y -= 32;
                    }
                }
            }
            */
            Position = new Vector2(32 * (int)(Parent.Position.X / 32), 32 * (int)(Parent.Position.Y / 32));
            if (Parent.Position.X < 0)
            {
                Position -= new Vector2(32, 0);
            }
            if (Parent.Position.Y > 0)
            {
                Position += new Vector2(0, 32);
            }

            // Mouse is in the top section of the game screen
            if ((mouse.Y < (Game1.ScreenHeight / 3)) && ((mouse.X > (Game1.ScreenWidth / 4)) && (mouse.X < (Game1.ScreenWidth * ((float)3 / 4)))))
            {
                velocity.Y -= 32;
            } // Mouse is in the bottom section of the game screen
            else if ((mouse.Y > (Game1.ScreenHeight * ((float)2 / 3)) && (mouse.X > (Game1.ScreenWidth / 4)) && (mouse.X < (Game1.ScreenWidth * ((float)3 / 4)))))
            {
                velocity.Y += 32;
            } // Mouse is in the left section of the game screen
            else if (mouse.X < (Game1.ScreenWidth / 2))
            {
                velocity.X -= 32;
            } // Mouse is in the right section of the game screen
            else
            {
                velocity.X += 32;
            }
            Position += velocity;
            Position += _offset;
        }
    }
}
