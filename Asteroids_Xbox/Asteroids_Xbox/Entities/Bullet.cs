///
///FILE          : bullet.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the class that makes up the bullets that are fired
///                 by the player.
///
using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Bullet for the player
    /// </summary>
    class Bullet : AnimatedEntity
    {
        /// <summary>
        /// The texture name for the bullet
        /// </summary>
        private const string TextureName = "bullet";
        /// <summary>
        /// The background color
        /// </summary>
        private readonly Color BackgroundColor = Color.GhostWhite;
        /// <summary>
        /// The entity manager
        /// </summary>
        private readonly EntityManager entityManager;
        /// <summary>
        /// The speed of the bullet
        /// </summary>
        private Vector2 speed;
        /// <summary>
        /// The origin of the bullet
        /// </summary>
        private Vector2 origin;
        /// <summary>
        /// The laser sound that is made by the bullet
        /// </summary>
        public SoundEffect laserSound;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bullet"/> class.
        /// </summary>
        /// <param name="entityManager">The entity manager.</param>
        /// <param name="position">The position.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="rotation">The rotation.</param>
        public Bullet(EntityManager entityManager,
            Vector2 position, Vector2 speed, float rotation)
        {
            this.entityManager = entityManager;
            this.speed = speed;
            this.Rotation = rotation;
            this.origin = position;
            Position = position;
        }

        /// <summary>
        /// Load the content for the bullet
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            laserSound = content.Load<SoundEffect>("sound/laserFire");
            Texture2D texture = content.Load<Texture2D>(TextureName);
            Animation.Initialize(texture, Vector2.Zero, 5, 5, 1, 30, BackgroundColor, 1f, true);

            MaxSpeed = 2.0f;
            MoveSpeed = 2.0f;
            RotationSpeed = 0.0f;
            WrapScreen = false;
        }

        /// <summary>
        /// Update the bullet on the game loop
        /// </summary>
        /// <param name="inputManager">The input manager.</param>
        /// <param name="gameTime">The current game time.</param>
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Forward(speed.X, speed.Y);
            base.Update(inputManager, gameTime);
        }

        /// <summary>
        /// Handle collisions with other entities
        /// </summary>
        /// <param name="other">The entity that the bullet is colliding with.</param>
        public override void Touch(AnimatedEntity other)
        {
            if (other is Asteroid)
            {
                var asteroid = other as Asteroid;
                asteroid.Health = 0;
                asteroid.Killer = this;
                Kill();
            }
            else if (other is EnemyShip)
            {
                var enemyShip = other as EnemyShip;
                enemyShip.Kill(this);
                Kill();
            }

            base.Touch(other);
        }

        /// <summary>
        /// Destroy the bullet
        /// </summary>
        protected void Kill()
        {
            entityManager.Remove(this);
        }
    }
}
