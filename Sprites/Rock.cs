using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Sprites
{
    public class Rock : Sprite
    {
        private BoundingBox _boundingBox;

        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
            set { if (_boundingBox != value) _boundingBox = value; }
        }

        public Rock(Texture2D texture) : base(texture)
        {

        }
    }
}
