using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaveCrawler.Core;
using CaveCrawler.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CaveCrawler.Sprites;
using Microsoft.Xna.Framework.Audio;

namespace CaveCrawler.Sprites
{
    public class Player : Entity
    {
        private float _attackTimer = 0.4f; // How long it has been between each attack
        private float _invincibilityTimer = 0f;
        private float _attackSpeedTimer = 0.4f;
        private float _mineTimer = 0.6f; // How long it has been between each pickaxe swing
        private float _mineSpeedTimer = 0.6f;
        private Camera _camera;
        private Weapon _weapon;
        private Pickaxe _pickaxe;

        // Returns the evaluted expression to determine if the Player has died or not
        public bool IsDead
        {
            get
            {
                return Health <= 0;
            }
        }
        public Input Input;
        public List<BoundingBox> BoundingBoxes { get; }
        public HighlightSquare HighlightSquare { get; set; }
        public int Score { get; set; }
        public bool IsHit { get; set; }
        private bool _doFadeIncrease { get; set; } // Used to fade the Player in and out of transparency when hit
        public Camera Camera
        {
            set { _camera = value; }
        }

        public Weapon Weapon
        {
            get { return _weapon; }
            set { if (_weapon != value) _weapon = value; }
        }
        public Pickaxe Pickaxe
        {
            get { return _pickaxe; }
            set { if (_pickaxe != value) _pickaxe = value; }
        }
        public bool DestroyRock { get; set; }
        public float MineTimer
        {
            get { return _mineTimer; }
        }
        public float MineSpeedTimer
        {
            get { return _mineSpeedTimer; }
        }
        public SoundEffect WeaponSFX {  get; set; }
        public SoundEffect WalkSFX { get; set; }
        private SoundEffectInstance _walkSFXInstance { get; set; }

        /// <summary>
        /// Player Constructor
        /// 
        /// Only need to set the Speed, so for everything else we use the Entity constructor
        /// </summary>
        /// <param name="animations"></param>
        public Player(Dictionary<string, Animation> animations) : base(animations)
        {
            Speed = 3.5f;
            BoundingBoxes = new List<BoundingBox>();
            DestroyRock = false;
        }

        public void AddBoundingBox(BoundingBox boundaryBox)
        {
            BoundingBoxes.Add(boundaryBox);
        }

        public override void Update(GameTime gameTime)
        {
            if (IsDead)
                return;

            // Updates the mousePosition to the current mouse's position, and also adds on the Camera's position since that is not accounted for with the Mouse's position within the screen and is needed for angle calculation
            var mouse = Mouse.GetState();

            // Creates two local variables for velocity and the keyboard state. If any movement key is pressed, then we move the player in that direction
            var velocity = Vector2.Zero;
            var keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Input.Up))
            {
                velocity.Y -= Speed;
            }
            if (keyboard.IsKeyDown(Input.Down))
            {
                velocity.Y += Speed;
            }
            if (keyboard.IsKeyDown(Input.Left))
            {
                velocity.X -= Speed;
            }
            if (keyboard.IsKeyDown(Input.Right))
            {
                velocity.X += Speed;
            }

            if (_attackTimer <= _attackSpeedTimer)
            {
                if (_weapon.AttackDirection == "North")
                {
                    _weapon.Layer = this.Layer - 0.1f;
                    _weapon.Rotation += MathHelper.ToRadians(3.5f);
                    _weapon.Offset += new Vector2(2f, 0f);
                } else if (_weapon.AttackDirection == "South")
                {
                    _weapon.Layer = this.Layer + 0.1f;
                    _weapon.Rotation += MathHelper.ToRadians(3.5f);
                    _weapon.Offset -= new Vector2(2f, 0f);
                } 
                else if (_weapon.AttackDirection == "East")
                {
                    _weapon.Layer = this.Layer + 0.1f;
                    _weapon.Rotation -= MathHelper.ToRadians(-4.5f);
                    _weapon.Offset += new Vector2(0.175f, 1.2f);
                } else // West
                {
                    _weapon.Layer = this.Layer + 0.1f;
                    _weapon.Rotation += MathHelper.ToRadians(4.5f);
                    _weapon.Offset -= new Vector2(0.175f, 1.2f);
                }
            }
            if (_mineTimer <= _mineSpeedTimer)
            {
                if (_pickaxe.MineDirection == "North")
                {
                    _pickaxe.Layer = this.Layer - 0.1f;
                    _pickaxe.Rotation += MathHelper.ToRadians(2.2f);
                    _pickaxe.Offset += new Vector2(0.3f, 0f);
                }
                else if (_pickaxe.MineDirection == "South")
                {
                    _pickaxe.Layer = this.Layer + 0.1f;
                    _pickaxe.Rotation += MathHelper.ToRadians(2.2f);
                    _pickaxe.Offset += new Vector2(-0.3f, 0f);
                }
                else if (_pickaxe.MineDirection == "East")
                {
                    _pickaxe.Layer = this.Layer + 0.1f;
                    _pickaxe.Rotation -= MathHelper.ToRadians(-2f);
                    _pickaxe.Offset += new Vector2(0.1f, 0.5f);
                }
                else // West
                {
                    _pickaxe.Layer = this.Layer + 0.1f;
                    _pickaxe.Rotation -= MathHelper.ToRadians(2f);
                    _pickaxe.Offset += new Vector2(-0.1f, 0.5f);
                }
            }

            // If the Player IsHit by an Enemy, then make them invincible for a set amount of time (1.5f)
            // During this time, the player cannot be collided with and will have their Alpha value flashed (fading in and out of transparency)
            if (IsHit)
            {
                _invincibilityTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_animationManager.Colour.A == 50)
                    _doFadeIncrease = true;
                else if (_animationManager.Colour.A == 155)
                    _doFadeIncrease = false;

                if (_doFadeIncrease)
                    _animationManager.ColourA = 7;
                else if (!_doFadeIncrease)
                    _animationManager.ColourA = -7;

                if (_invincibilityTimer > 1.5f)
                {
                    IsHit = false;
                    _invincibilityTimer = 0f;
                    _animationManager.Colour = Color.White;
                }
            }

            // Checks if the required time gap between attacks has been elapsed, and if so and the button to attack has been pressed, then attack
            _attackTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            _mineTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ((mouse.LeftButton == ButtonState.Pressed) && _attackTimer > _attackSpeedTimer)
            {
                // Mouse is in top section of the game screen
                if ((mouse.Y < (Game1.ScreenHeight / 3)) && ((mouse.X > (Game1.ScreenWidth / 4)) && (mouse.X < (Game1.ScreenWidth * ((float)3 / 4))))) {
                    _weapon.AttackDirection = "North";
                    _weapon.Offset = new Vector2(-20, -30); 
                    _weapon.Rotation = MathHelper.ToRadians(-90);
                    PrevVelocity = new Vector2(0, -1);
                } // Mouse is in the bottom section of the game screen
                else if ((mouse.Y > (Game1.ScreenHeight * ((float)2 / 3)) && (mouse.X > (Game1.ScreenWidth / 4)) && (mouse.X < (Game1.ScreenWidth * ((float)3 / 4)))))
                {
                    _weapon.AttackDirection = "South";
                    _weapon.Offset = new Vector2(20, 45);
                    _weapon.Rotation = MathHelper.ToRadians(90);
                    PrevVelocity = new Vector2(0, 1);
                } // Mouse is in the left section of the game screen
                else if (mouse.X < (Game1.ScreenWidth / 2))
                {
                    _weapon.AttackDirection = "West";
                    _weapon.Offset = new Vector2(-28, 25);
                    _weapon.Rotation = MathHelper.ToRadians(180);
                    PrevVelocity = new Vector2(-1, 0);
                } // Mouse is in the right section of the game screen
                else
                {
                    _weapon.AttackDirection = "East";
                    _weapon.Offset = new Vector2(28, -3);
                    _weapon.Rotation = MathHelper.ToRadians(-40);
                    PrevVelocity = new Vector2(1, 0);
                } 
                _attackTimer = 0f;
                _weapon.DoDraw = true;
                WeaponSFX.Play();
            }
            // Checks if the required time gap between mines has been elapsed, and if so and the button to mine has been pressed, then mine
            else if ((mouse.RightButton == ButtonState.Pressed) && _mineTimer > _mineSpeedTimer) 
            {
                // Mouse is in top section of the game screen
                if ((mouse.Y < (Game1.ScreenHeight / 3)) && ((mouse.X > (Game1.ScreenWidth / 4)) && (mouse.X < (Game1.ScreenWidth * ((float)3 / 4)))))
                {
                    _pickaxe.MineDirection = "North";
                    _pickaxe.Offset = new Vector2(-20, -25);
                    _pickaxe.Rotation = MathHelper.ToRadians(-90);
                    PrevVelocity = new Vector2(0, -1);
                } // Mouse is in the bottom section of the game screen
                else if ((mouse.Y > (Game1.ScreenHeight * ((float)2 / 3)) && (mouse.X > (Game1.ScreenWidth / 4)) && (mouse.X < (Game1.ScreenWidth * ((float)3 / 4)))))
                {
                    _pickaxe.MineDirection = "South";
                    _pickaxe.Offset = new Vector2(20, 40);
                    _pickaxe.Rotation = MathHelper.ToRadians(90);
                    PrevVelocity = new Vector2(0, 1);
                } // Mouse is in the left section of the game screen
                else if (mouse.X < (Game1.ScreenWidth / 2))
                {
                    _pickaxe.MineDirection = "West";
                    _pickaxe.Offset = new Vector2(-24, -8);
                    _pickaxe.Rotation = MathHelper.ToRadians(-40);
                    PrevVelocity = new Vector2(-1, 0);
                } // Mouse is in the right section of the game screen
                else
                {
                    _pickaxe.MineDirection = "East";
                    _pickaxe.Offset = new Vector2(24, -8);
                    _pickaxe.Rotation = MathHelper.ToRadians(-40);
                    PrevVelocity = new Vector2(1, 0);
                }
                _mineTimer = 0f;
                _pickaxe.DoDraw = true;
                DestroyRock = true;
            }
            foreach (var box in BoundingBoxes)
            {
                if (box.Parent != null)
                    continue;

                if ((BoundingBox.IsTouchingTop(box) && velocity.Y > 0)
                    || (BoundingBox.IsTouchingBottom(box) && velocity.Y < 0))
                {
                    velocity.Y = 0;
                } else if ((BoundingBox.IsTouchingLeft(box) && velocity.X > 0)
                    || (BoundingBox.IsTouchingRight(box) && velocity.X < 0))
                {
                    velocity.X = 0;
                }
            }

            if (_attackTimer > _attackSpeedTimer && _mineTimer > _mineSpeedTimer)
            {
                if (velocity != Vector2.Zero)
                {
                    PrevVelocity = velocity;
                    velocity = Vector2.Normalize(velocity) * Speed;
                    if (_walkSFXInstance == null)
                        _walkSFXInstance = WalkSFX.CreateInstance();

                    if (_walkSFXInstance.State != SoundState.Playing)
                    {
                        _walkSFXInstance.Play();
                    }
                }
                Position += velocity;

                _weapon.AttackDirection = "";
                _weapon.DoDraw = false;
                _weapon.Rotation = 0;
                _pickaxe.MineDirection = "";
                _pickaxe.DoDraw = false;
                _pickaxe.Rotation = 0;
                if (_mineTimer > _mineSpeedTimer + 0.1f)
                {
                    DestroyRock = false;
                }
            }
            else
            {
                velocity = Vector2.Zero;
            }

            DetermineAnimations(velocity);
            _animationManager.Update(gameTime);

            BoundingBox.Update(gameTime);

            Weapon.Update(gameTime);

            if (Shadow  != null) 
            { 
                Shadow.Position = Position + new Vector2(0, 30);
            }
        }

        /// <summary>
        /// The Draw method
        /// 
        /// If the Player is dead, then do not draw them. Otherwise call the Entity's draw method
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDead) 
                return;

            base.Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// The OnCollide method for the ICollidable interface
        /// </summary>
        /// <param name="boundingBox"></param>
        public override void OnCollide(BoundingBox boundingBox)
        {
            if (IsDead)
                return;

            if (boundingBox.Parent is Enemy && !IsHit)
            {
                Health--;
                IsHit = true;
                _animationManager.Colour = new Color(255, 255, 255, 50);
                _doFadeIncrease = true;
            }
        }
    }
}
