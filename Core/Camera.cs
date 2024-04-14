using CaveCrawler.Sprites;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Core
{
    public class Camera
    {
        public Matrix Transform { get; set; }
        public Vector2 Position { get; set; }

        /// <summary>
        /// A method which moves the camera so that the target Sprite is centred on the game screen
        /// </summary>
        /// <param name="target"></param>
        public void MoveTo(Sprite target)
        {
            Transform = Matrix.CreateTranslation(
                -target.Position.X - (target.Rectangle.Width / 2),
                -target.Position.Y - (target.Rectangle.Height / 2),
                0) * Matrix.CreateTranslation(Game1.ScreenWidth / 2, Game1.ScreenHeight / 2, 0);
        }
    }
}
