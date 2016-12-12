///
///FILE          : enemyship.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the class that makes up the enemy ships
///
using System;
using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Enemy ship can either be small or large
    /// </summary>
    class EnemyShip : AnimatedEntity
    {
        /// <summary>
        /// The entity manager
        /// </summary>
        private readonly EntityManager entityManager;
        /// <summary>
        /// A random number generator
        /// </summary>
        private readonly Random random;
        /// <summary>
        /// The background color
        /// </summary>
        private readonly Color BackgroundColor = Color.White;
        /// <summary>
        /// The enemy explosion texture name
        /// </summary>
        private const string EnemyExplosionTextureName = "Ship_Explode";
        /// <summary>
        /// Time between bullet fire times
        /// </summary>
        private const double bulletFireTime = 2.0;
        /// <summary>
        /// Size of the enemy ship
        /// </summary>
        public Sizes Size { get; set; }
        /// <summary>
        /// The enemy texture
        /// </summary>
        private Texture2D enemyTexture;
        /// <summary>
        /// The seconds since the last spawn
        /// </summary>
        private double previousSeconds;
        /// <summary>
        /// The speed of the ship
        /// </summary>
        private Vector2 speed;
        /// <summary>
        /// The player
        /// </summary>
        private Player player;
        /// <summary>
        /// The current game time
        /// </summary>
        private GameTime gameTime;
        /// <summary>
        /// The content
        /// </summary>
        private ContentManager content;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyShip"/> class.
        /// </summary>
        /// <param name="entityManager">The entity manager.</param>
        /// <param name="position">The ship position.</param>
        /// <param name="speed">The ship speed.</param>
        /// <param name="size">The ship size.</param>
        /// <param name="player">The player.</param>
        public EnemyShip(EntityManager entityManager, Vector2 position, Vector2 speed, Sizes size, Player player)
        {
            this.entityManager = entityManager;
            random = new Random();
            Position = position;
            this.Size = size;
            this.speed = speed;
            this.player = player;
        }

        /// <summary>
        /// Load the ship's content
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            this.content = content;
            /// Figure out if it is a small or large ship
            switch (this.Size)
            {
                case Sizes.Small:
                    enemyTexture = content.Load<Texture2D>("Enemy_Small_Animated");
                    Animation.Initialize(enemyTexture, Vector2.Zero, 50, 20, 8, 45, BackgroundColor, 1f, true);
                    break;
                case Sizes.Large:
                    enemyTexture = content.Load<Texture2D>("Enemy_Large_Animated");
                    Animation.Initialize(enemyTexture, Vector2.Zero, 75, 30, 8, 45, BackgroundColor, 1f, true);
                    break;
                default:
                    enemyTexture = content.Load<Texture2D>("Enemy_Large_Animated");
                    Animation.Initialize(enemyTexture, Vector2.Zero, 75, 30, 8, 45, BackgroundColor, 1f, true);
                    break;
            }
            MaxSpeed = 5.0f;
            MoveSpeed = 5.0f;
            WrapScreen = true;
        }

        /// <summary>
        /// Update the ship on the game loop
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            this.gameTime = gameTime;
            Move(speed.X, speed.Y);
            Move(0, random.Next(-1, 1));
            var vec = new Vector2((float)random.Next(-(int)MaxSpeed, (int)MaxSpeed), (float)random.Next(-(int)MaxSpeed, (int)MaxSpeed));
            vec.Normalize();
            FireBullet(vec);
            base.Update(inputManager, gameTime);
        }

        /// <summary>
        /// Kill the ship
        /// </summary>
        public void Kill(Entity killer)
        {
            var enemyBullet = (killer is EnemyBullet);
            if (!enemyBullet)
            {
                entityManager.Remove(this);

                if (Size == Sizes.Small)
                {
                    player.Score += 1000;
                }
                else
                {
                    player.Score += 200;
                }

                var explosion = CreateExplosion(Position, content, GraphicsDevice);
                entityManager.Add(explosion);
            }
        }

        /// <summary>
        /// Fire a bullet from the ship
        /// </summary>
        /// <param name="speed"></param>
        /// <returns></returns>
        private Bullet FireBullet(Vector2 speed)
        {
            var totalSeconds = gameTime.TotalGameTime.TotalSeconds;
            var timeSinceLast = totalSeconds - previousSeconds;
            if (timeSinceLast > bulletFireTime)
            {
                Vector2 bulletPosition = new Vector2(Position.X - (Width / 2), Position.Y - (Height / 2));
                var bullet = new EnemyBullet(entityManager, this, bulletPosition, speed, Rotation);
                entityManager.Add(bullet);
                bullet.laserSound.Play();
                previousSeconds = totalSeconds;
                return bullet;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Explosion
        /// </summary>
        /// <param name="position"></param>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        /// <returns></returns>
        private Explosion CreateExplosion(Vector2 position, ContentManager content, GraphicsDevice graphicsDevice)
        {
            var explosion = new Explosion(entityManager, EnemyExplosionTextureName);
            explosion.Initialize(content, graphicsDevice);
            explosion.Position = new Vector2(position.X, position.Y);
            return explosion;
        }

    }
}
