///
///FILE          : inputmanager.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the input manager. Handles user input.
///
using Microsoft.Xna.Framework.Input;

namespace Asteroids_Xbox.Manager
{
    /// <summary>
    /// Contains the current state of input for the current keyboard and gamepad
    /// </summary>
    class InputManager
    {
        /// <summary>
        /// Current state of the keyboard
        /// </summary>
        public KeyboardState CurrentKeyboardState { get; set; }

        /// <summary>
        /// Previous state of the keyboard (from the last gameloop iteration)
        /// </summary>
        public KeyboardState PreviousKeyboardState { get; set; }

        /// <summary>
        /// Current state of the gamepad
        /// </summary>
        public GamePadState CurrentGamePadState { get; set; }

        /// <summary>
        /// Previous state of the gamepad (from the last gameloop iteration)
        /// </summary>
        public GamePadState PreviousGamePadState { get; set; }

        /// <summary>
        /// Was a keyboard key pressed then released
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool WasKeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Was a gamepad button pressed then released
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool WasButtonPressed(Buttons button)
        {
            return CurrentGamePadState.IsButtonDown(button) && PreviousGamePadState.IsButtonUp(button);
        }
    }
}
