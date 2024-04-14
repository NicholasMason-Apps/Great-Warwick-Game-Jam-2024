using CaveCrawler.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Sprites
{
    public class Enemy : Entity
    {
        protected Player _player;
        private float _timer = 1.5f;
        public float KnockbackTimer = 0.75f;
        private Vector2 _playerVelocityWhenHit;
        private float _invincibilityTimer = 0f;

        // Used for pathfinding to the Player so that we can get the Player position
        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }
        public bool IsHit { get; set; }

        /// <summary>
        /// The Enemy Constructor
        /// 
        /// All we need to do is set its move speed
        /// </summary>
        /// <param name="animations"></param>
        public Enemy(Dictionary<string, Animation> animations) : base(animations)
        {
            Speed = 2.75f;
        }

        /// <summary>
        /// The Update Method
        /// 
        /// For now we just move it around in some sort of motion to test movement works
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            var velocity = Vector2.Zero;

            if (IsHit)
            {
                _invincibilityTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_invincibilityTimer > KnockbackTimer)
                {
                    IsHit = false;
                    _invincibilityTimer = 0f;
                    _animationManager.Colour = Color.White;
                }
            }

            var xDistance = Math.Sqrt((Position.X - Player.Position.X) * (Position.X - Player.Position.X));
            var yDistance = Math.Sqrt((Position.Y - Player.Position.Y) * (Position.Y - Player.Position.Y));
            if (xDistance > yDistance)
            {
                if (Position.X < Player.Position.X)
                {
                    velocity = new Vector2(Speed, 0);
                }
                else
                {
                    velocity = new Vector2(-Speed, 0);
                }
            }
            else if (yDistance > xDistance)
            {
                if (Position.Y < Player.Position.Y)
                {
                    velocity = new Vector2(0, Speed);
                }
                else
                {
                    velocity = new Vector2(0, -Speed);
                }
            } 
            else
            {
                velocity = new Vector2(Game1.Random.Next(0, (int)Speed), Game1.Random.Next(0, (int)Speed));
            }


            if (_timer < KnockbackTimer)
                applyKnockback();

            DetermineAnimations();
            _animationManager.Update(gameTime);
            BoundingBox.Update(gameTime);
            Position += velocity;
        }

        /// <summary>
        /// A method used to determine the animation to play for a Sprite depending on their velocity
        /// </summary>
        /// <param name="velocity"></param>
        protected virtual void DetermineAnimations()
        {
            if (_animationManager == null)
                return;

            _animationManager.Play(_animations["Move"]);
        }

        /// <summary>
        /// The OnCollide Method
        /// 
        /// If the Enemy touches a Player, then remove the enemy for now
        /// 
        /// If the Enemy touches a Bullet, then reduce the Enemy's health
        /// </summary>
        /// <param name="boundingBox"></param>
        public override void OnCollide(BoundingBox boundingBox)
        {
            if (boundingBox.Parent is Player && boundingBox is Weapon && !IsHit)
            {
                Health--;

                if (Health <= 0)
                {
                    IsRemoved = true;
                    BoundingBox.IsRemoved = true;
                }
                _playerVelocityWhenHit = Player.PrevVelocity;
                _timer = 0f;
                IsHit = true;
                _animationManager.Colour = Color.Red;
            }
        }

        public void applyKnockback()
        {
            // If the Player is facing the right when attacking, knockback to the right
            if (_playerVelocityWhenHit.X > 0)
            {
                Position += new Vector2(15f - 10f * _timer, 0);
            } 
            else if (_playerVelocityWhenHit.X < 0)
            {
                Position += new Vector2(-15f + 10f * _timer, 0);
            }
            else if (_playerVelocityWhenHit.Y < 0)
            {
                Position += new Vector2(0, -15f + 10f * _timer);
            }
            else if (_playerVelocityWhenHit.Y > 0)
            {
                Position += new Vector2(0, 15f - 10f * _timer);
            }
        }
    }
}