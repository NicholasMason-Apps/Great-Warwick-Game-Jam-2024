using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Sprites
{
    /// <summary>
    /// An Interface called ICollidable
    /// Used to specify what Sprites are collidable, and if so then they are required to implement the OnCollide() method since we should do something if they are collidable
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// OnCollide Method
        /// Takes in a Sprite, which means that only Sprites can collide with other Sprites which inherit the ICollidable interface
        /// </summary>
        /// <param name="boundingBox"></param>
        void OnCollide(BoundingBox boundingBox);
    }
}
