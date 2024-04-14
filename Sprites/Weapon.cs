using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Sprites
{
    public class Weapon : BoundingBox
    { 
        public string AttackDirection { get; set; }
        public Weapon(Texture2D texture) : base(texture)
        {
            
        }
    }
}
