///
///FILE          : initializable.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : An interface for objects that accept updates from the game loop
///
using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;

namespace Asteroids_Xbox.Types
{
    /// <summary>
    /// An interface for objects that accept updates from the game loop
    /// </summary>
    interface Updatable
    {
        void Update(InputManager inputManager, GameTime gameTime);
    }
}
