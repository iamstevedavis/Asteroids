///
///FILE          : Program.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : Application entry.
///
using System;

namespace Asteroids_Xbox
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Asteroids game = new Asteroids())
            {
                game.Run();
            }
        }
    }
#endif
}

