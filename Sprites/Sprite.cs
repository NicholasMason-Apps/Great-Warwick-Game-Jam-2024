using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaveCrawler;
using CaveCrawler.Managers;
using CaveCrawler.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaveCrawler.Sprites
{
    public class Sprite : Component, ICloneable
    {
        #region Fields
        protected Dictionary<string, Animation> _animations; // Dictionary of string and Animation which is for the file location of the spritesheet and the Animation object instance for it respectively
        protected AnimationManager _animationManager;
        protected float _layer { get; set; }
        protected Vector2 _origin { get; set; }
        protected Vector2 _position { get; set; }
        protected float _rotation { get; set; }
        protected float _scale { get; set; }
        protected Texture2D _texture;
        #endregion

        #region Properties
        public Sprite Parent { get; set; }
        public List<Sprite> Children { get; set; } // A list of all the sprites which are children of this sprite
        public Color Colour { get; set; }
        public int ColourA // Get method for an attribute which is used to either return the Alpha value of the Animation's colour, or change that Alpha value
        {
            get { return Colour.A; }
            set { Colour = new Color(Colour.R, Colour.G, Colour.B, Colour.A + value); }
        }
        public bool IsRemoved { get; set; } // A Boolean to check if a Sprite needs to be removed (i.e not displayed or stored in memory anymore)
        public bool DoDraw { get; set; } // A Boolean to determine whether a Sprite should be drawn or not
        public float Layer // Returns _layer when called, but if a value is wanting to be set it sets _layer to it and the AnimationManager's layer if the Sprite is animated
        {
            get { return _layer; }
            set
            {
                _layer = value;

                if (_animationManager != null)
                    _animationManager.Layer = _layer;
            }
        }

        public Vector2 Origin // Returns _origin, but if a value is wanting to be set it sets _origin to it and the origin of the AnimationManager if the sprite is animated
        {
            get { return _origin; }
            set
            {
                _origin = value;

                if (_animationManager != null)
                    _animationManager.Origin = _origin;
            }
        }

        public Vector2 Position // Returns _position, but if a new position is wanting to be set it sets _position to it and the position in AnimationManager if one exists
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;

                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }

        public Rectangle Rectangle // Returns a new rectangle, either of the static texture or of the animation frame being displayed
        {
            get
            {
                if (_texture != null)
                {
                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, _texture.Width, _texture.Height);
                }

                if (_animationManager != null)
                {
                    var animation = _animations.FirstOrDefault().Value;

                    return new Rectangle((int)Position.X - (int)Origin.X, (int)Position.Y - (int)Origin.Y, animation.FrameWidth, animation.FrameHeight);
                }

                throw new Exception("Unknown sprite");
            }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = value;

                if (_animationManager != null)
                    _animationManager.Rotation = value;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sprite Constructor which takes a static Texture2D
        /// </summary>
        /// <param name="texture"></param>
        public Sprite(Texture2D texture)
        {
            _texture = texture;

            Children = new List<Sprite>();

            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);

            Colour = Color.White;
        }

        /// <summary>
        /// Sprite Constructor for when an animation is wanting to be played
        /// </summary>
        /// <param name="animations"></param>
        public Sprite(Dictionary<string, Animation> animations)
        {
            _texture = null;

            Children = new List<Sprite>();

            Colour = Color.White;

            _animations = animations;

            var animation = _animations.FirstOrDefault().Value;

            _animationManager = new AnimationManager(animation);

            Origin = new Vector2(animation.FrameWidth / 2, animation.FrameHeight / 2);
        }

        /// <summary>
        /// The Update method is empty since every primitive Sprite is static and does nothing
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {

        }

        /// <summary>
        /// The Draw Method
        /// If a texture is loaded, draws the static texture
        /// If an animation is loaded, calls the Draw method of the animationManager
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, Position, null, Colour, _rotation, Origin, 1f, SpriteEffects.None, Layer);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
        }

        /// <summary>
        /// Method to clone the Sprite Object
        /// Also clones each instance of Animation and AnimationManager associated with the sprite
        /// </summary>
        /// <returns> The cloned sprite</returns>
        public object Clone()
        {
            var sprite = this.MemberwiseClone() as Sprite;

            if (_animations != null)
            {
                sprite._animations = this._animations.ToDictionary(c => c.Key, v => v.Value.Clone() as Animation);
                sprite._animationManager = sprite._animationManager.Clone() as AnimationManager;
            }

            return sprite;
        }
        #endregion
    }
}