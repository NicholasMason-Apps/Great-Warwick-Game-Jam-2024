using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Sprites
{
    public class BoundingBox : Sprite
    {
        public Vector2 Offset { get; set; }
        public new Entity Parent { get; set; }
        public Rock RockParent {  get; set; }
        public BoundingBox(Texture2D texture) : base(texture)
        {

        }

        /// <summary>
        /// Method which checks if two sprites are colliding
        /// 
        /// Checks if the Bounding Rectangles of both sprites are intersecting in any way
        /// This is done by checking for an intersection of the Left, Right, Top, and Bottom of the rectangles
        /// An intersection is checked by seeing if the positions of the respective sides overlap
        /// </summary>
        /// <param name="boundingBox"></param>
        /// <returns> A Boolean of if an intersection occurred or not </returns>
        public bool Intersects(BoundingBox boundingBox)
        {
            if ((this.Rectangle.Right > boundingBox.Rectangle.Left &&
                this.Rectangle.Left < boundingBox.Rectangle.Left &&
                this.Rectangle.Bottom > boundingBox.Rectangle.Top &&
                this.Rectangle.Top < boundingBox.Rectangle.Bottom) || //Checking Left of Sprite

                (this.Rectangle.Left < boundingBox.Rectangle.Right &&
                this.Rectangle.Right > boundingBox.Rectangle.Right &&
                this.Rectangle.Bottom > boundingBox.Rectangle.Top &&
                this.Rectangle.Top < boundingBox.Rectangle.Bottom) || //Checking Right of Sprite

                (this.Rectangle.Bottom > boundingBox.Rectangle.Top &&
                this.Rectangle.Top < boundingBox.Rectangle.Top &&
                this.Rectangle.Right > boundingBox.Rectangle.Left &&
                this.Rectangle.Left < boundingBox.Rectangle.Right) || //Checking Top of Sprite

                (this.Rectangle.Top < boundingBox.Rectangle.Bottom &&
                this.Rectangle.Bottom > boundingBox.Rectangle.Bottom &&
                this.Rectangle.Right > boundingBox.Rectangle.Left &&
                this.Rectangle.Left < boundingBox.Rectangle.Right)) //Checking Bottom of Sprite
                return true;
            //No intersection occurred
            return false;
        }

        public virtual bool IsTouchingLeft(BoundingBox boundingBox)
        {
            return this.Rectangle.Right > boundingBox.Rectangle.Left &&
                this.Rectangle.Left < boundingBox.Rectangle.Left &&
                this.Rectangle.Bottom > boundingBox.Rectangle.Top &&
                this.Rectangle.Top < boundingBox.Rectangle.Bottom;
        }
        public virtual bool IsTouchingRight(BoundingBox boundingBox)
        {
            return this.Rectangle.Left < boundingBox.Rectangle.Right &&
                this.Rectangle.Right > boundingBox.Rectangle.Right &&
                this.Rectangle.Bottom > boundingBox.Rectangle.Top &&
                this.Rectangle.Top < boundingBox.Rectangle.Bottom;
        }
        public virtual bool IsTouchingTop(BoundingBox boundingBox)
        {
            return this.Rectangle.Bottom > boundingBox.Rectangle.Top &&
                this.Rectangle.Top < boundingBox.Rectangle.Top &&
                this.Rectangle.Right > boundingBox.Rectangle.Left &&
                this.Rectangle.Left < boundingBox.Rectangle.Right;
        }
        public virtual bool IsTouchingBottom(BoundingBox boundingBox)
        {
            return this.Rectangle.Top < boundingBox.Rectangle.Bottom &&
                this.Rectangle.Bottom > boundingBox.Rectangle.Bottom &&
                this.Rectangle.Right > boundingBox.Rectangle.Left &&
                this.Rectangle.Left < boundingBox.Rectangle.Right;
        }

        /// <summary>
        /// The Update Method
        /// 
        /// Simply moves the Boundary Box to be in line with its Parent Sprite, and if some offset is defined then it is applied (such as making the boundary box be in the middle bottom of the parent sprite's texture size if the boundary box is smaller than the parent sprite's max size)
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Parent != null)
                Position = Parent.Position + Offset;

            base.Update(gameTime);
        }

    }
}
