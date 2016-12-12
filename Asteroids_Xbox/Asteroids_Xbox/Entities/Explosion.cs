///
///FILE          : explosion.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the class that makes up the explosions
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
    /// Explosion is inherited from AnimatedEntity
    /// </summary>
    class Explosion : AnimatedEntity
    {
        /// <summary>
        /// The entity manager
        /// </summary>
        private readonly EntityManager entityManager;
        /// <summary>
        /// The explosion texture name
        /// </summary>
        private readonly string explosionTextureName;
        /// <summary>
        /// The explosion texture
        /// </summary>
        private Texture2D explosionTexture;
        /// <summary>
        /// The explode sound
        /// </summary>
        public SoundEffect explodeSound;

        /// <summary>
        /// Initializes a new instance of the <see cref="Explosion"/> class.
        /// </summary>
        /// <param name="entityManager">The entity manager.</param>
        /// <param name="explosionTextureName">Name of the explosion texture.</param>
        public Explosion(EntityManager entityManager, string explosionTextureName)
        {
            this.entityManager = entityManager;
            this.explosionTextureName = explosionTextureName;
        }

        /// <summary>
        /// On load, load the explosion texture and sounds.
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            explodeSound = content.Load<SoundEffect>("sound/explosion");
            explosionTexture = content.Load<Texture2D>(explosionTextureName);
            Animation.Initialize(explosionTexture, Vector2.Zero,
                explosionTexture.Width / 16, explosionTexture.Height, 16, 60, Color.White, 1f, false);
            WrapScreen = true;
        }

        /// <summary>
        /// Update the entity. This is usually performed in the gameloop
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Rotate(5.0f);
            if (!Animation.ShouldDraw)
            {
                entityManager.Remove(this);
            }
            base.Update(inputManager, gameTime);
        }

        /// <summary>
        /// Plays the explosion sound.
        /// </summary>
        public void PlayExplosionSound()
        {
            explodeSound.Play();
        }
    }
}
