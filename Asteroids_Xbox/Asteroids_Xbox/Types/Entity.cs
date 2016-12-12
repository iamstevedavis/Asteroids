///
///FILE          : entity.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the entity class. See below for more details.
///
using System;
using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Types
{
    /// <summary>
    /// This is the base class for all entities in the game. An entity need not be visible, but it can respond to the draw event.
    /// 
    /// Otherwise, an entity usually responds to the initialization and update events from the content manager and game loop.
    /// 
    /// 
    /// This game uses an awful inheritance model that was used by older games done in C++ in the early 2000s.
    /// 
    /// Some game development kits and engines still carry this legacy (such as UT3), but most of them are being replaced
    /// (ie. UT4). Composition is the preferred method now, but due to a lack of a dependency injection framework and
    /// being too lazy to make one for this project, this classical model was choosen.
    /// 
    /// Also, if C# had mixins, this would be a lot easier.
    /// </summary>
    abstract class Entity : Initializable, Updatable, Drawable
    {
        /// <summary>
        /// Has the entity been initialized and had its content loaded?
        /// </summary>
        public bool Initialized { get; protected set; }
        /// <summary>
        /// Rotation in degrees
        /// </summary>
        public float Rotation { get; set; }
        /// <summary>
        /// Rotation in radians
        /// </summary>
        public float Radians { get { return MathHelper.ToRadians(Rotation); } }
        /// <summary>
        /// Position of the unit
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// Current speed of the unit
        /// </summary>
        public Vector2 CurrentSpeed { get; set; }
        /// <summary>
        /// Maximum speed of the unit
        /// </summary>
        protected float MaxSpeed { get; set; }
        /// <summary>
        /// Speed at which the unit accelerates
        /// </summary>
        protected float MoveSpeed { get; set; }
        /// <summary>
        /// Speed at which the unit rotates
        /// </summary>
        protected float RotationSpeed { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        public Entity()
        {
            Rotation = 0;
            Position = Vector2.Zero;

            CurrentSpeed = Vector2.Zero;
            MaxSpeed = 0.0f;
            MoveSpeed = 0.0f;
            RotationSpeed = 0.0f;
        }

        /// <summary>
        /// Moves the specified entity
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        public virtual void Move(float x, float y)
        {
            this.Position = new Vector2(this.Position.X + x, this.Position.Y + y);
        }

        /// <summary>
        /// Move forward at the move speed
        /// </summary>
        public void Forward()
        {
            Forward(MoveSpeed, MoveSpeed);
        }

        /// <summary>
        /// Move backward at the move speed
        /// </summary>
        public void Backward()
        {
            Forward(-MoveSpeed, -MoveSpeed);
        }

        /// <summary>
        /// Move the entity forward at the specified speed. The direction is calculated according to rotation
        /// </summary>
        /// <param name="speedModX">How fast to move in the x direction</param>
        /// <param name="speedModY">How fast to move in the y direction</param>
        protected void Forward(float speedModX, float speedModY)
        {
            double rad = MathHelper.ToRadians(Rotation);

            float speedX = (float)Math.Cos(rad) * speedModX;
            float speedY = (float)Math.Sin(rad) * speedModY;
            
            var nextSpeedX = CurrentSpeed.X + speedX;
            var nextSpeedY = CurrentSpeed.Y + speedY;
            /// The new speed to move the entity at.
            var nextSpeed = new Vector2(nextSpeedX, nextSpeedY);

            if (nextSpeed.Length() > MaxSpeed)
            {
                var x = nextSpeed.X;
                var y = nextSpeed.Y;
                var max = Math.Max(x, y);

                var mod = Vector2.Distance(Vector2.Zero, CurrentSpeed) / MaxSpeed;
                if (Math.Abs(mod) > 0.0)
                {
                    nextSpeed = new Vector2
                    (
                        nextSpeed.X / mod,
                        nextSpeed.Y / mod
                    ); 
                }
            }

            CurrentSpeed = nextSpeed;
        }

        /// <summary>
        /// Rotate the entity in degrees.
        /// This function takes into consideration that degrees cannot go below 0 or above 359.
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        public virtual void Rotate(float angle)
        {
            var nextRotation = Rotation + angle;
            if (nextRotation > 0.0f)
            {
                this.Rotation = nextRotation % 360.0f;
            }
            else
            {
                this.Rotation = 360.0f + angle;
            }
        }

        /// <summary>
        /// Load content and resources
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        public abstract void Initialize(ContentManager content, GraphicsDevice graphicsDevice);

        /// <summary>
        /// Update on each game loop
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
        public abstract void Update(InputManager inputManager, GameTime gameTime);

        /// <summary>
        /// Redraw unit
        /// </summary>
        /// <param name="spriteBatch"></param>
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
