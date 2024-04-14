using CaveCrawler.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Managers
{
    public class AnimationManager : ICloneable
    {
        private Animation _animation; // The animation to play

        private float _timer;

        public Animation CurrentAnimation
        {
            get
            {
                return _animation;
            }
        }

        public float Layer { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public Color Colour { get; set; } // Get and Set methods for the Animation spritesheet's colour
        public int ColourA // Get method for an attribute which is used to either return the Alpha value of the Animation's colour, or change that Alpha value
        {
            get { return Colour.A; }
            set { Colour = new Color(Colour.R, Colour.G, Colour.B, Colour.A + value); }
        }

        /// <summary>
        /// Constructor for AnimationManager
        /// </summary>
        /// <param name="animation"> The animation to store and play with this Manager </param>
        public AnimationManager(Animation animation)
        {
            _animation = animation;
            Scale = 1f;
            Colour = Color.White;
        }

        /// <summary>
        /// The Draw Method
        /// 
        /// Uses a Rectangle parameter to specify the specific section of the animation spritesheet to display each frame
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
              _animation.Texture,
              Position,
              new Rectangle(
                _animation.CurrentFrame * _animation.FrameWidth,
                0,
                _animation.FrameWidth,
                _animation.FrameHeight
                ),
              Colour,
              Rotation,
              Origin,
              Scale,
              SpriteEffects.None,
              Layer
              );
        }

        /// <summary>
        /// Play Method
        /// 
        /// Used to set a new animation to play with this Manager
        /// </summary>
        /// <param name="animation"></param>
        public void Play(Animation animation)
        {
            if (_animation == animation)
                return;

            _animation = animation;

            _animation.CurrentFrame = 0;

            _timer = 0;
        }

        /// <summary>
        /// Reset the current animation
        /// </summary>
        public void Stop()
        {
            _timer = 0f;

            _animation.CurrentFrame = 0;
        }

        /// <summary>
        /// Update()
        /// 
        /// Increments _timer with the elapsed game time in seconds
        /// If the _timer exceeds the FrameSpeed, then increment the Frame to be displayed, and if that exceeds the total frame count then reset it back to 0
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;

                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }

        /// <summary>
        /// Clone() method for the ICloneable Interface
        /// 
        /// First clones this instance of the animationManager
        /// Then clones the animation object stored in _animation
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            var animationManager = this.MemberwiseClone() as AnimationManager;

            animationManager._animation = animationManager._animation.Clone() as Animation;

            return animationManager;
        }
    }
}