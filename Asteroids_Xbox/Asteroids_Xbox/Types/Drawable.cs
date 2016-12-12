///
///FILE          : drawable.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the drawable class. Handles drawing
///                 entities.
///
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Types
{
    /// <summary>
    /// Interface for implementing a drawable event for an entity or object
    /// </summary>
    interface Drawable
    {
        /// <summary>
        /// Rotation 0 - 359
        /// </summary>
        float Rotation { get; set; }
        /// <summary>
        /// Position of the Player relative to the upper left side of the screen
        /// </summary>
        Vector2 Position { get; set; }
        /// <summary>
        /// Draw the entity
        /// </summary>
        /// <param name="spriteBatch"></param>
        void Draw(SpriteBatch spriteBatch);
    }
}
