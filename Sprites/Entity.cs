using CaveCrawler.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Sprites
{
    public class Entity : Sprite, ICollidable
    {
        private BoundingBox _boundingBox;
        public int Health { get; set; }
        public float Speed { get; set; }
        public Vector2 PrevVelocity { get; set; }
        public BoundingBox BoundingBox
        {
            get { return _boundingBox; }
            set { if (_boundingBox != value) _boundingBox = value; }
        }
        public Sprite Shadow { get; set; }

        /// <summary>
        /// Constructor for Entity
        /// All Entities will have some animation associated with them, so we implement the animation Sprite constructor
        /// Since nothing else is needed, we implement the base constructor
        /// </summary>
        /// <param name="animations"></param>
        public Entity(Dictionary<string, Animation> animations) : base(animations)
        {
            PrevVelocity = new Vector2(0, 1);
        }

        /// <summary>
        /// A method used to determine the animation to play for a Sprite depending on their velocity
        /// </summary>
        /// <param name="velocity"></param>
        protected virtual void DetermineAnimations(Vector2 velocity)
        {
            if (_animationManager == null)
                return;

            if (velocity.X > 0)
            {
                _animationManager.Play(_animations["WalkRight"]);
            }
            else if (velocity.X < 0)
            {

                _animationManager.Play(_animations["WalkLeft"]);

            }
            
            else if (velocity.Y > 0)
            {

                _animationManager.Play(_animations["WalkDown"]);
            }
            /*
            else if (velocity.Y < 0)
            {
                _animationManager.Play(_animations["WalkUp"]);
            }
            */
            else if (PrevVelocity.X > 0)
            {
                _animationManager.Play(_animations["IdleRight"]);
            }
            else if (PrevVelocity.X < 0)
            {

                _animationManager.Play(_animations["IdleLeft"]);

            }

            else if (PrevVelocity.Y > 0)
            {

                _animationManager.Play(_animations["IdleDown"]);
            }
            else if (PrevVelocity.Y < 0)
            {
                _animationManager.Play(_animations["IdleUp"]);
            }
        }

        public virtual void OnCollide(BoundingBox boundingBox)
        {
            throw new NotImplementedException();
        }
    }
}
