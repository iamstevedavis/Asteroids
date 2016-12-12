///
///FILE          : scoredisplay.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the class that makes up the score display
///
using System.Collections.Generic;
using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Display the score for the player and their remaining lives
    /// </summary>
    class ScoreDisplay : Entity
    {
        /// <summary>
        /// The players
        /// </summary>
        private readonly List<Player> players = new List<Player>();
        /// <summary>
        /// The font color
        /// </summary>
        private readonly Color FontColor = Color.FloralWhite;
        /// <summary>
        /// The font
        /// </summary>
        private SpriteFont font;
        /// <summary>
        /// The graphics device
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoreDisplay"/> class.
        /// </summary>
        /// <param name="players">The players.</param>
        public ScoreDisplay(List<Player> players)
        {
            this.players.AddRange(players);
        }

        /// <summary>
        /// Load content and resources
        /// </summary>
        /// <param name="content"></param>
        /// <param name="graphicsDevice"></param>
        public override void Initialize(ContentManager content, GraphicsDevice graphicsDevice)
        {
            font = content.Load<SpriteFont>("gameFont");
            this.graphicsDevice = graphicsDevice;
        }

        /// <summary>
        /// Update on each game loop
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
        }

        /// <summary>
        /// Redraw unit
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (var player in players)
            {
                /// Draw the score
                spriteBatch.DrawString(font, "Score: " + player.Score,
                    new Vector2(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y), FontColor);

                /// Draw the lives of the player
                spriteBatch.DrawString(font, "Lives: " + player.Lives,
                    new Vector2(graphicsDevice.Viewport.X, graphicsDevice.Viewport.Y + 35), FontColor);    
            }
        }
    }
}
