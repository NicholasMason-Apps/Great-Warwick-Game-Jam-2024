using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Sprites
{
    public class Pickaxe : Sprite
    {
        public string MineDirection {  get; set; }
        public Vector2 Offset {  get; set; }
        public Pickaxe(Texture2D texture) : base(texture)
        {

        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Parent != null)
                Position = Parent.Position + Offset;

            base.Update(gameTime);
        }
    }
}
