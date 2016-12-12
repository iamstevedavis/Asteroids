///
///FILE          : enemybullet.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the class that makes up the bullets that are fired
///                 by the enemy ships.
///
using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Extension of a bullet that may also kill the player.
    /// Fired from the enemy ship.
    /// </summary>
    class EnemyBullet : Bullet
    {
        /// <summary>
        /// The ship that fired this bullet
        /// </summary>
        private EnemyShip ship;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnemyBullet"/> class.
        /// </summary>
        /// <param name="entityManager">The entity manager.</param>
        /// <param name="ship">The ship.</param>
        /// <param name="position">The position.</param>
        /// <param name="speed">The speed.</param>
        /// <param name="rotation">The rotation.</param>
        public EnemyBullet(EntityManager entityManager, EnemyShip ship,
            Vector2 position, Vector2 speed, float rotation) : 
            base(entityManager, position, speed, rotation)
        {
            this.ship = ship;
        }

        /// <summary>
        /// Change the touch event details slightly to also kill the player
        /// </summary>
        /// <param name="other">The entity that the bullet is colliding with.</param>
        public override void Touch(Types.AnimatedEntity other)
        {
            /// Check if the bullet is colliding with a player.
            /// This also checks and makes sure bug ships can kill
            /// asteroids and small ships cant.
            if (other is Player)
            {
                var player = other as Player;
                if (!player.UnderProtection)
                {
                    player.Kill();
                }

                Kill();
            }
            else if (other is Asteroid && ship.Size == Sizes.Large)
            {
                var asteroid = other as Asteroid;
                asteroid.Health = 0;
                asteroid.Killer = this;
                Kill();
            }
        }
    }
}
