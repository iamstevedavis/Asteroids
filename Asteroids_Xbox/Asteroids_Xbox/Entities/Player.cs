///
///FILE          : player.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the class that makes up the player
///
using System;
using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Player contains the player ship, handles the controls, score, lives, etc.
    /// </summary>
    class Player : AnimatedEntity
    {
        /// <summary>
        /// Default lives
        /// </summary>
        public const int DefaultLives = 3;
        /// <summary>
        /// Texture for the ship
        /// </summary>
        private const string PlayerTextureName = "shipAnimation";
        /// <summary>
        /// Texture for the explosion
        /// </summary>
        private const string PlayerExplosionTextureName = "Ship_Explode";
        /// <summary>
        /// Time between bullet fires
        /// </summary>
        private const double bulletFireTime = 0.5;
        /// <summary>
        /// Protection time on respawn
        /// </summary>
        private const double protectionTime = 1.0;
        /// <summary>
        /// Time between hyperspaces
        /// </summary>
        private const double hyperspaceTime = 2.0;
        /// <summary>
        /// Background color
        /// </summary>
        private readonly Color BackgroundColor = Color.White;
        /// <summary>
        /// Entity manager
        /// </summary>
        private readonly EntityManager entityManager;
        /// <summary>
        /// Content manager
        /// </summary>
        private ContentManager content;
        /// <summary>
        /// Current gametime
        /// </summary>
        private GameTime gameTime;
        /// <summary>
        /// Number of lives that the player has
        /// </summary>
        public int Lives { get; set; }
        /// <summary>
        /// Player score
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// Is the player still alive?
        /// </summary>
        public bool Alive
        {
            get
            {
                return Lives > 0;
            }
        }

        /// <summary>
        /// Is the player under spawn protection?
        /// </summary>
        public bool UnderProtection
        {
            get
            {
                if (gameTime != null)
                {
                    return (gameTime.TotalGameTime.TotalSeconds - lastRespawnTime) <= protectionTime;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The number of gametime seconds that the bullet was last fired
        /// </summary>
        private double lastBulletFireTime;

        /// <summary>
        /// The last time the player was respawned
        /// </summary>
        private double lastRespawnTime;

        /// <summary>
        /// The last time that hyperspace was activated
        /// </summary>
        private double lastHyperspaceTime;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="entityManager">The entity manager.</param>
        public Player(EntityManager entityManager)
        {
            this.entityManager = entityManager;
        }

        /// <summary>
        /// Load content
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            this.content = content;
            Texture2D playerTexture = content.Load<Texture2D>(PlayerTextureName);
            Animation.Initialize(playerTexture, Vector2.Zero, 75, 30, 8, 45, BackgroundColor, 1f, true);
            Score = 0;
            Lives = DefaultLives;
            MoveSpeed = 1.5f;
            MaxSpeed = 2.5f;
            RotationSpeed = 5.0f;
            CurrentSpeed = Vector2.Zero;
            WrapScreen = true;
            Respawn();
        }

        /// <summary>
        /// Respawn the player. If the player died, then an explosion will be shown
        /// </summary>
        /// <param name="died"></param>
        private void Respawn()
        {
            if (gameTime != null)
            {
                lastRespawnTime = gameTime.TotalGameTime.TotalSeconds; 
            }
            
            Position = new Vector2
            (
                GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
                GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2
            );
        }

        /// <summary>
        /// Update. This is where we handle the controls.
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            this.gameTime = gameTime;
            /// Handle controls
            var keyboard = inputManager.CurrentKeyboardState;
            var gamepad = inputManager.CurrentGamePadState;
            /// If the user is pressing left on the DPad or keyboard
            if (keyboard.IsKeyDown(Keys.Left) ||
                gamepad.DPad.Left == ButtonState.Pressed)
            {
                Rotate(-RotationSpeed);
            }
            /// If the user is pressing right on the DPad or keyboard
            else if (keyboard.IsKeyDown(Keys.Right) ||
                gamepad.DPad.Right == ButtonState.Pressed)
            {
                Rotate(RotationSpeed);
            }
            /// If the user is pressing up on the DPad or keyboard
            if (keyboard.IsKeyDown(Keys.Up) ||
                gamepad.DPad.Up == ButtonState.Pressed)
            {
                Forward();
            }
            else
            {
                /// Handle Deccelerate...
                CurrentSpeed = new Vector2(CurrentSpeed.X / 1.05f, CurrentSpeed.Y / 1.05f);
            }

            base.Update(inputManager, gameTime);

            /// Handle Hyperspace. The condition is that the user can't hold
            /// down a hyperspace.
            if (inputManager.WasKeyPressed(Keys.H) ||
                inputManager.WasButtonPressed(Buttons.B) &&
                (gameTime.TotalGameTime.TotalSeconds - lastHyperspaceTime) > hyperspaceTime)
            {
                var random = new Random();
                float x = random.Next(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Width);
                float y = random.Next(GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Height);
                Position = new Vector2(x, y);
                lastHyperspaceTime = gameTime.TotalGameTime.TotalSeconds;
            }

            /// Fire bullet
            if (keyboard.IsKeyDown(Keys.Space) ||
                gamepad.Buttons.A == ButtonState.Pressed)
            {
                FireBullet(new Vector2(5.0f, 5.0f));
            }
        }

        /// <summary>
        /// Do we want to use the alternate draw?
        /// </summary>
        bool alternateDraw = false;
        /// <summary>
        /// Draw the animation to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (UnderProtection)
            {
                if (alternateDraw)
                {
                    base.Draw(spriteBatch);
                }

                alternateDraw = !alternateDraw;
            }
            else
            {
                base.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Fire a bullet in the player's direction
        /// </summary>
        /// <param name="speed">The speed of the bullet.</param>
        /// <returns>A bullet</returns>
        private Bullet FireBullet(Vector2 speed)
        {
            var totalSeconds = gameTime.TotalGameTime.TotalSeconds;
            var timeSinceLast = totalSeconds - lastBulletFireTime;
            if (timeSinceLast > bulletFireTime)
            {
                Vector2 bulletPosition = new Vector2(Position.X - (Width / 2), Position.Y - (Height / 2));
                var bullet = new Bullet(entityManager, bulletPosition, speed, Rotation);
                entityManager.Add(bullet);
                bullet.laserSound.Play();
                lastBulletFireTime = totalSeconds;
                return bullet;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Kill the player
        /// </summary>
        public void Kill()
        {
            Lives -= 1;
            if (Lives > 0)
            {
                var explosion = CreateExplosion(Position, content, GraphicsDevice);
                entityManager.Add(explosion);
                explosion.PlayExplosionSound();

                Respawn();
            }
        }

        /// <summary>
        /// Create the player explosion
        /// </summary>
        /// <param name="position">The position of the explosion</param>
        /// <param name="content">The Content manager</param>
        /// <param name="graphicsDevice">The graphics device</param>
        /// <returns>A explosion</returns>
        private Explosion CreateExplosion(Vector2 position, ContentManager content, GraphicsDevice graphicsDevice)
        {
            var explosion = new Explosion(entityManager, PlayerExplosionTextureName);
            explosion.Initialize(content, graphicsDevice);
            explosion.Position = new Vector2(position.X, position.Y);
            return explosion;
        }

    }
}
