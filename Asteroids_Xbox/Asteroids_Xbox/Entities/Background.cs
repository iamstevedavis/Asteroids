///
///FILE          : background.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the class that makes up the backgrounds on the screen.
///                 There are two possible backgrounds, one is chosen at random.
///
using System;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Background image entity
    /// </summary>
    class Background : AnimatedEntity
    {
        /// <summary>
        /// Load the background content
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            Texture2D texture;
            var rand = new Random();
            var bg = rand.Next(0, 10);
            if(bg < 5)
                texture = content.Load<Texture2D>("space1");
            else
                texture = content.Load<Texture2D>("space2");
            

            Animation.Initialize(texture, Vector2.Zero, texture.Width, texture.Height, 1, 30, Color.White, 1f, true);
            WrapScreen = true;
        }

        /// <summary>
        /// Draw the animation to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
