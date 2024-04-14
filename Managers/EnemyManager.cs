using CaveCrawler.Models;
using CaveCrawler.Sprites;
using CaveCrawler.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaveCrawler.Managers
{
    public class EnemyManager
    {
        private float _timer;
        private Dictionary<string, Animation> _animations; // List of textures for potential enemies
        private ContentManager _content;
        private Player _player;
        private GameState _gameState;

        public bool CanAdd { get; set; }
        public int MaxEnemies { get; set; }
        public float SpawnTimer { get; set; }
        public Player Player { 
            get { return _player; } 
            set {  _player = value; }
        }

        /// <summary>
        /// The EnemyManager Constructor
        /// 
        /// Loads all the enemy textures into a list, and initialises the values for MaxEnemies and SpawnTimer
        /// </summary>
        /// <param name="content"> The ContentManager parameter used to load Textures</param>
        public EnemyManager(ContentManager content, GameState gameState)
        {
            _gameState = gameState;
            // Creates the Dictionary of animations, and loads the spritesheets into the dictionary
            _animations = new Dictionary<string, Animation>()
            {
                {"Move", new Animation(content.Load<Texture2D>("Enemies/Bat"), 3) },
            };
            MaxEnemies = 6 + _gameState.Score;

            SpawnTimer = 1.75f;

            _content = content; 
        }

        /// <summary>
        /// The Update Method
        /// 
        /// Adds to the _timer attribute
        /// If _timer exceeds the time required to have elapsed to spawn an enemy, then set CanAdd to true
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            CanAdd = false;

            if (_timer > SpawnTimer)
            {
                CanAdd = true;
                _timer = 0f;
            }
        }

        /// <summary>
        /// Method to return a new Enemy Object
        /// 
        /// Uses the set methods in Enemy to set the values for its Colour, Add a Bullet prefab for it, its Health, etc.
        /// </summary>
        /// <returns></returns>
        public Enemy GetEnemy()
        {
            var enemy = new Enemy(_animations)
            {
                Colour = Color.White,
                Health = 4,
                Layer = 0.9f,
                Position = new Vector2(Game1.Random.Next(0, Game1.ScreenWidth * 3), Game1.Random.Next(0, Game1.ScreenHeight * 3)),
                BoundingBox = new Sprites.BoundingBox(_content.Load<Texture2D>("BoundingBox"))
                {
                    Layer = 0.2f,
                    Offset = Vector2.Zero,
                },
                Player = _player,
            };
            enemy.BoundingBox.Parent = enemy;
            return enemy;
        }
    }
}
