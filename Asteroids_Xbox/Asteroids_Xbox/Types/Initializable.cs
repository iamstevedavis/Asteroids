///
///FILE          : initializable.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : Interface for object allowing it to accept initialization from the content loader
///
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Types
{
    /// <summary>
    /// Interface for object allowing it to accept initialization from the content loader
    /// </summary>
    interface Initializable
    {
        void Initialize(ContentManager content, GraphicsDevice graphicsDevice);
    }
}
