///
///FILE          : animatedentity.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the animated entity class.
///                 It handles the entites in the game that are
///                 animated.
///
using System;
using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Types
{
    /// <summary>
    /// Animated entity. This is an entity which contains an animation and collision detection.
    /// </summary>
    abstract class AnimatedEntity : Entity
    {
        /// <summary>
        /// The animation for the entity
        /// </summary>
        public Animation Animation { get; set; }

        /// <summary>
        /// The graphics device for the game
        /// </summary>
        protected GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Position of the unit
        /// </summary>
        public Vector2 CenterPosition
        {
            get
            {
                return new Vector2(Position.X - (Width / 2), Position.Y - (Height / 2));
            }
        }

        /// <summary>
        /// Wrap movement around the screen (ie. instead of item moving off screen, it wraps back to the other end)
        /// </summary>
        public bool WrapScreen { get; set; }

        /// <summary>
        /// The number of times the entity has wrapped across the screen
        /// 
        /// See property WrapScreen for more details
        /// </summary>
        public int WrapScreenCount { get; private set; }

        /// <summary>
        /// Bounds of the entity
        /// 
        /// This is the position and width/height
        /// </summary>
        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        /// <summary>
        /// Radius of the collision circle
        /// </summary>
        public float Radius
        {
            get
            {
                return (Bounds.Width > Bounds.Height) ? Bounds.Width / 2 : Bounds.Height / 2;
            }
        }

        /// <summary>
        /// Frame width of the animated texture
        /// </summary>
        public int Width
        {
            get { return Animation.FrameWidth; }
        }

        /// <summary>
        /// Frame height of the animated texture
        /// </summary>
        public int Height
        {
            get { return Animation.FrameHeight; }
        }

        /// <summary>
        /// Is the entity offscreen?
        /// </summary>
        public bool Offscreen
        {
            get
            {
                var intersects = GraphicsDevice.Viewport.Bounds.Intersects(Bounds);
                var contains = GraphicsDevice.Viewport.Bounds.Contains(Bounds);
                return !intersects && !contains;
            }
        }

        public AnimatedEntity()
        {
            WrapScreen = false;
        }

        /// <summary>
        /// On Loading of content
        /// </summary>
        /// <param name="content"></param>
        public abstract void Load(ContentManager content);

        /// <summary>
        /// Initialize the entity and load its resources
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            Animation = new Animation();
            Load(content);

            WrapScreenCount = 0;

            Initialized = true;
        }

        /// <summary>
        /// Update the entity. This is usually performed in the gameloop
        /// </summary>
        /// <param name="inputManager">The input manager</param>
        /// <param name="gameTime">The current game time</param>
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            Animation.Position = Position;
            Animation.Update(gameTime);

            Move(CurrentSpeed.X, CurrentSpeed.Y);
        }

        /// <summary>
        /// Draw the animation to the screen
        /// </summary>
        /// <param name="spriteBatch">A sprite batch to draw</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            Animation.Draw(spriteBatch);
        }

        /// <summary>
        /// Translate or move the entity x and y units
        /// </summary>
        /// <param name="x">The x location to move to</param>
        /// <param name="y">The y location to move to</param>
        public override void Move(float x, float y)
        {
            base.Move(x, y);
            if (WrapScreen)
            {
                var previousPosition = Position;
                /// Make sure that the unit does not go out of bounds, but instead wraps across the screen
                if (Position.X <= 0.0f)
                {
                    var width = GraphicsDevice.Viewport.Width;
                    Position = new Vector2(width - 1, Position.Y);
                }
                if (Position.Y <= 0.0f)
                {
                    var height = GraphicsDevice.Viewport.Height;
                    Position = new Vector2(Position.X, height - 1);
                }
                Position = new Vector2
                (
                    Position.X % GraphicsDevice.Viewport.Width,
                    Position.Y % GraphicsDevice.Viewport.Height
                );
                if (previousPosition != Position)
                {
                    WrapScreenCount++;
                }
            }
        }

        /// <summary>
        /// Rotate the entity and animation. This changes the forward/backward directions
        /// </summary>
        /// <param name="angle"></param>
        public override void Rotate(float angle)
        {
            base.Rotate(angle);
            Animation.Rotation = Rotation;
        }

        /// <summary>
        /// Check if this entity has collided with another entity
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CheckCollision(AnimatedEntity other)
        {
            var distance = Math.Abs(Vector2.Distance(this.CenterPosition, other.CenterPosition));
            var inThisRadius = distance < this.Radius;
            var inOtherRadius = distance < other.Radius;
            if (inThisRadius || inOtherRadius)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Calculate the colission on a per pixel level
        /// Looks at both pixel colors. If they are not transparent (the alpha channel is not 0), 
        /// then there is a collision.
        /// </summary>
        /// <param name="e1">Entity 1</param>
        /// <param name="e2">Entity 2</param>
        /// <returns></returns>
        private bool PerPixelCollision(AnimatedEntity e1, AnimatedEntity e2)
        {
            var at = e1.Animation.Transformations;
            var afw = e1.Animation.FrameWidth;
            var afh = e1.Animation.FrameHeight;
            var acd = e1.Animation.ColorData;
            var bt = e2.Animation.Transformations;
            var bfw = e2.Animation.FrameWidth;
            var bfh = e2.Animation.FrameHeight;
            var bcd = e2.Animation.ColorData;

            /// Get Color data of each Texture
            Color[] bitsA = new Color[afw * afh];
            Color[] bitsB = new Color[bfw * bfh];

            /// Calculate the intersecting rectangle
            int x1 = Math.Max(e1.Bounds.X, e2.Bounds.X);
            int x2 = Math.Min(e1.Bounds.X + e1.Bounds.Width, e2.Bounds.X + e2.Bounds.Width);

            int y1 = Math.Max(e1.Bounds.Y, e2.Bounds.Y);
            int y2 = Math.Min(e1.Bounds.Y + e1.Bounds.Height, e2.Bounds.Y + e2.Bounds.Height);

            /// For each single pixel in the intersecting rectangle
            for (int y = y1; y < y2; ++y)
            {
                for (int x = x1; x < x2; ++x)
                {
                    /// Get the color from each texture
                    Color a = bitsA[(x - e1.Bounds.X) + (y - e1.Bounds.Y) * afw];
                    Color b = bitsB[(x - e2.Bounds.X) + (y - e2.Bounds.Y) * bfw];

                    /// If both colors are not transparent (the alpha channel is not 0), then there is a collision
                    if (a.A != 0 && b.A != 0)
                    {
                        return true;
                    }
                }
            }
            /// If no collision occurred by now, we're clear.
            return false;
        }

        /// <summary>
        /// Handle a touch event with another entity.
        /// This occurs when two entities collide
        /// </summary>
        /// <param name="other">The other entity that has been collided with</param>
        public virtual void Touch(AnimatedEntity other)
        {
        }
    }
}
