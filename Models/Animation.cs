using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Models
{
    public class Animation : ICloneable
    {
        public int CurrentFrame { get; set; } // Current Frame of the Animation to play
        public int FrameCount { get; private set; } // Total number of frames in a spritesheet
        public int FrameHeight { get { return Texture.Height; } } // Height of each frame
        public float FrameSpeed { get; set; } // The speed at which to go through frames
        public int FrameWidth { get { return Texture.Width / FrameCount; } } // Width of each frame
        public bool IsLooping { get; set; } // Bool to determine if the animation loops or not
        public Texture2D Texture { get; private set; } // Sprite texture to load

        /// <summary>
        /// Constructor for Animation
        /// 
        /// By default, the animation speed is 0.2f and loops
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="frameCount"></param>
        public Animation(Texture2D texture, int frameCount)
        {
            Texture = texture;

            FrameCount = frameCount;

            IsLooping = true;

            FrameSpeed = 0.1f;
        }

        /// <summary>
        /// Clone() method from the ICloneable interface
        /// 
        /// Used so that the Animation object can be cloned so that different sprites can be animated at the same time
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}