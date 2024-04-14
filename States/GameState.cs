using CaveCrawler.Core;
using CaveCrawler.Managers;
using CaveCrawler.Models;
using CaveCrawler.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace CaveCrawler.States
{
    public class GameState : State
    {
        #region Fields
        private EnemyManager _enemyManager;
        private SpriteFont _font;
        private Player _player;
        private List<Sprite> _sprites;
        private Camera _camera;
        private MapManager _mapManager;
        private bool _caveTransition;
        private Sprite _blackWindow;
        private int _health;
        private int _score;
        #endregion

        public Player Player
        {
            get { return _player; }
            set { _player = value; }
        }
        public int Health
        {
            get { return _health; }
            set { _health = value; }
        }
        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }

        #region Methods

        /// <summary>
        /// The GameState Constructor
        /// It has no special implementation and simply uses the default State constructor
        /// </summary>
        /// <param name="game"></param>
        /// <param name="content"></param>
        public GameState(Game1 game, ContentManager content) : base(game, content)
        {
            _caveTransition = false;
        }

        /// <summary>
        /// The LoadContent Method
        /// 
        /// Initialises the Camera
        /// Loads the Player animations and creates the Bullet prefab
        /// 
        /// Creates a new player object and EnemyManager object
        /// </summary>
        public override void LoadContent()
        {
            _camera = new Camera(); // Initialises the Camera

            // Creates a variable for the Player Animations Dictionary
            var playerAnimations = new Dictionary<string, Animation>()
            {
                {"WalkRight", new Animation(_content.Load<Texture2D>("Player/Kitsune_Right_Walk"), 8) },
                {"WalkLeft", new Animation(_content.Load<Texture2D>("Player/Kitsune_Left_Walk"), 8) },
                {"WalkDown", new Animation(_content.Load<Texture2D>("Player/Kitsune_Front_Walk"), 6) },
                {"IdleRight", new Animation(_content.Load<Texture2D>("Player/Kitsune_Right_Idle"), 1) },
                {"IdleLeft", new Animation(_content.Load<Texture2D>("Player/Kitsune_Left_Idle"), 1) },
                {"IdleDown", new Animation(_content.Load<Texture2D>("Player/Kitsune_Front_Idle"), 1) },
                {"IdleUp", new Animation(_content.Load<Texture2D>("Player/Kitsune_Back_Idle"), 1) },
            };
            // Creates a variable for the Bullet's Texture
            var bulletTexture = _content.Load<Texture2D>("Bullet");

            _font = _content.Load<SpriteFont>("Font");
            _sprites = new List<Sprite>();

            var boundingBoxTexture = _content.Load<Texture2D>("BoundingBox");

            // Creates a new Player Object
            // Sets its starting position to the centre of the screen
            // Gives it a Bullet prefab
            // Gives it a reference to the Camera Object
            var player = new Player(playerAnimations)
            {
                Colour = Color.White,
                Position = new Vector2(0, 0),
                Layer = 0.5f,
                Input = new Models.Input()
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                },
                Health = _health,
                Camera = _camera,
                BoundingBox = new Sprites.BoundingBox(_content.Load<Texture2D>("Player/Kitsune_Boundary_Box"))
                {
                    Layer = 0.2f,
                    Colour = new Color(255, 255, 255, 50),
                },
                Weapon = new Weapon(_content.Load<Texture2D>("Items/Katana"))
                {
                    Offset = Vector2.Zero,
                },
                HighlightSquare = new HighlightSquare(_content.Load<Texture2D>("HighlightSquare"))
                {
                    Layer = 0.4f,
                },
                Pickaxe = new Pickaxe(_content.Load<Texture2D>("Items/Pickaxe")),
                Score = 1,
                WeaponSFX = _content.Load<SoundEffect>("Sounds/SwordSwing"),
                WalkSFX = _content.Load<SoundEffect>("Sounds/FootstepStone"),
            };
            player.Position = new Vector2(32 * 2 + 16, 32 - (32 * 20));
            player.BoundingBox.Parent = player;
            player.BoundingBox.Offset = new Vector2(0, (int)player.Rectangle.Height / 4 + 3);
            player.Weapon.Parent = player;
            player.HighlightSquare.Parent = player;
            player.Pickaxe.Parent = player;

            var shadow = new Sprite(_content.Load<Texture2D>("Player/Kitsune_Shadow"))
            {
                Layer = 0.4f,
            };

            player.Shadow = shadow;
            _sprites.Add(shadow);

            // Sets _player to the player object and adds the Object to _sprites
            _player = player;
            _sprites.Add(player);
            _sprites.Add(player.HighlightSquare);

            _sprites.Add(player.BoundingBox);
            _sprites.Add(player.Weapon);
            _sprites.Add(player.Pickaxe);

            // Creates a new EnemyManager object and sets its Bullet to the Bullet Prefab so enemies spawned can clone it
            _enemyManager = new EnemyManager(_content, this)
            {
                Player = _player,
            };

            var rockPrefab = new Rock(_content.Load<Texture2D>("Caves/Cave1/Rock"))
            {
                BoundingBox = new Sprites.BoundingBox(boundingBoxTexture)
                {
                    Layer = 0.2f,
                }
            };

            _mapManager = new MapManager(_content);

            foreach (var sprite in _mapManager.sprites)
            {
                _sprites.Add(sprite);
                if (sprite is Rock)
                {
                    _sprites.Add(((Rock)sprite).BoundingBox);
                }
            }
            _blackWindow = new Sprite(_content.Load<Texture2D>("Caves/BlackScreen"))
            {
                Layer = 1f,
            };
            _blackWindow.Colour = new Color(255, 255, 255, 0);
            _sprites.Add(_blackWindow);
        }

        /// <summary>
        /// The Update Method
        /// 
        /// Checks if Escape is pressed down, and if so then returns to the Main Menu
        /// 
        /// Calls the Update method for every sprite in _sprites
        /// 
        /// Calls the EnemyManager's Update()
        /// If the time elapsed for adding an enemy has passed, and the number of Enemy objects in _sprite is less than the Max Enemies, then add an enemy
        /// Finally, set the Camera to Move To the Player
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (_caveTransition)
            {
                _blackWindow.ColourA += 5;
                if (_blackWindow.ColourA  >= 255)
                {
                    _game.ChangeState(new GameState(_game, _content)
                    {
                        Score = _score + 1,
                        Health = _player.Health,
                    });
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.ChangeState(new MenuState(_game, _content));

            foreach (var sprite in _sprites)
            {
                if (sprite is Sprites.BoundingBox)
                {
                    if (sprite is HighlightSquare)
                    {
                        // Do nothing for this if statement
                    }
                    else if (_player.BoundingBoxes.Contains(sprite) || sprite.Parent != null)
                        continue;
                    else
                        _player.BoundingBoxes.Add((Sprites.BoundingBox)sprite);
                }

                sprite.Update(gameTime);
            }
            _enemyManager.Update(gameTime);
            if (_enemyManager.CanAdd && _sprites.Where(c => c is Enemy).Count() < _enemyManager.MaxEnemies)
            {
                var enemy = _enemyManager.GetEnemy();
                _sprites.Add(enemy);
                _sprites.Add(enemy.BoundingBox);
            }
            _blackWindow.Position = _player.Position + new Vector2(32, 32);

            _camera.MoveTo(_player);
        }

        /// <summary>
        /// The PostUpdate Method
        /// 
        /// First checks if any collidable sprites are intersecting, and if so then calls their Intersect method
        /// Next adds any Children sprites to _sprites, and creates a new Children list for those sprites
        /// Loops through _sprites and removes any sprite which needs removing
        /// Checks if the Player is dead, and if so then change to the Main Menu
        /// </summary>
        /// <param name="gameTime"></param>
        public override void PostUpdate(GameTime gameTime)
        {
            bool addHole = false;
            Sprites.BoundingBox boxForHole = null;
            var boundingBoxes = _sprites.Where(c => c is Sprites.BoundingBox);

            foreach (var spriteA in boundingBoxes)
            {
                foreach (var spriteB in boundingBoxes)
                {
                    // If the two boxes are the same, do nothing
                    if (spriteA == spriteB)
                        continue;

                    if (((Sprites.BoundingBox)spriteA).Parent is Enemy && ((Sprites.BoundingBox)spriteB).Parent is Enemy)
                        continue;

                    // If spriteA is Weapon, do nothing since we do not want to call the Player's OnCollide for this case
                    // If spriteB is Weapon and we do not want to draw the Weapon, then also do nothing since we do not want to incorrectly call an OnCollide method
                    if (spriteA is Weapon || (spriteB is Weapon && !(spriteB.DoDraw)))
                        continue;


                    if (spriteB is HighlightSquare && ((Sprites.BoundingBox)spriteA).RockParent != null)
                    {
                        if (_player.DestroyRock && _player.MineTimer > _player.MineSpeedTimer && ((Sprites.BoundingBox)spriteA).Intersects((Sprites.BoundingBox)spriteB))
                        {
                            _player.BoundingBoxes.Remove((Sprites.BoundingBox)spriteA);
                            spriteA.IsRemoved = true;
                            ((Sprites.BoundingBox)spriteA).RockParent.IsRemoved = true;
                            _player.DestroyRock = false;
                            _mapManager.RockCount--;

                            if (Game1.Random.NextDouble() * 100 < 5 || _mapManager.RockCount == 0)
                            {
                                addHole = true;
                                boxForHole = (Sprites.BoundingBox)spriteA;
                            }

                            continue;
                        }
                    }

                    if (spriteA is HighlightSquare && spriteB is Hole)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.E) && ((Sprites.BoundingBox)spriteB).Intersects((Sprites.BoundingBox)spriteA))
                        {
                            _caveTransition = true;
                        }
                    }

                    if (((Sprites.BoundingBox)spriteA).Parent is null || ((Sprites.BoundingBox)spriteB).Parent is null)
                        continue;

                    if (((Sprites.BoundingBox)spriteA).Intersects((Sprites.BoundingBox)spriteB))
                    {
                        ((Sprites.BoundingBox)spriteA).Parent.OnCollide(((Sprites.BoundingBox)spriteB));
                    }
                }
            }

            if (addHole)
            {
                var hole = _mapManager.GenerateHole(boxForHole);
                _sprites.Add(hole);
                _player.AddBoundingBox(hole);
            }

            // Add any Child sprites to _sprites, and then create a list to hold their Children sprites if needed
            int spriteCount = _sprites.Count;
            for (int i = 0; i < spriteCount; i++)
            {
                var sprite = _sprites[i];

                foreach (var child in sprite.Children)
                {
                    _sprites.Add(child);
                }
                sprite.Children = new List<Sprite>();
            }

            // Loops through the list and checks if any sprite needs removing
            // Decrements i if a removal occurs so no out of bounds error occurs
            for (int i = 0; i < _sprites.Count; i++)
            {
                if (_sprites[i].IsRemoved)
                {
                    _sprites.RemoveAt(i);
                    i--;
                }
            }

            if (_player.IsDead)
            {
                _game.ChangeState(new MenuState(_game, _content));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: _camera.Transform);

            foreach (var sprite in _sprites)
            {
                if (sprite is Weapon && !(sprite.DoDraw))
                    continue;

                if (sprite is Pickaxe && !(sprite.DoDraw))
                    continue;

                if (sprite is Sprites.BoundingBox && sprite is not Weapon && sprite is not HighlightSquare && sprite is not Hole)
                    continue;
                

                sprite.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Health: " + _player.Health, new Vector2(10f, 10f), Color.White);
            spriteBatch.DrawString(_font, "Level: " + Score, new Vector2(10f, 40f), Color.White);

            spriteBatch.DrawString(_font, "Controls: \nLeft Click = Attack\nRight Click = Mine\nE = Interact", new Vector2(Game1.ScreenWidth - 150f, 10f), Color.White);
            spriteBatch.End();
        }
        #endregion
    }
}
