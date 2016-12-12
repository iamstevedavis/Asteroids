///
///FILE          : titlescreen.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is the class that makes up the title screen.
///                 This handles the help screen and any other screens
///
using System.Collections.Generic;
using Asteroids_Xbox.Manager;
using Asteroids_Xbox.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Asteroids_Xbox.Entities
{
    /// <summary>
    /// Current status of the title screen
    /// </summary>
    enum TitlescreenStatus
    {
        Start,
        GameOver,
        Help,
        Pause
    }

    /// <summary>
    /// Display and process the titlescreen
    /// </summary>
    class Titlescreen : AnimatedEntity
    {
        /// <summary>
        /// Player
        /// </summary>
        public Player Player { get; set; }
        /// <summary>
        /// Titlescreen status
        /// </summary>
        public TitlescreenStatus TitlescreenStatus { get; set; }
        /// <summary>
        /// The visible
        /// </summary>
        private bool visible;
        /// <summary>
        /// Is the titlescreen currently visible
        /// </summary>
        public bool Visible
        {
            get { return visible; }
            set
            {
                visible = value;
                if (visible)
                {
                    PlayMusic(menuSong);
                }
                else
                {
                    PlayMusic(gameSong);
                }
            }
        }
        /// <summary>
        /// Is a new game being requested by the titlescreen?
        /// </summary>
        public bool NewGameRequested { get; set; }
        /// <summary>
        /// Is the titlescreen asking for an exit?
        /// </summary>
        public bool ExitRequested { get; private set; }
        /// <summary>
        /// The font
        /// </summary>
        private SpriteFont font;
        /// <summary>
        /// The menu song
        /// </summary>
        private Song menuSong;
        /// <summary>
        /// The game song
        /// </summary>
        private Song gameSong;
        /// <summary>
        /// Are we using a gamepad?
        /// </summary>
        private bool isGamepad;

        /// <summary>
        /// Initializes a new instance of the <see cref="Titlescreen"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        public Titlescreen(Player player)
        {
            this.Player = player;
        }

        /// <summary>
        /// On Loading of content
        /// </summary>
        /// <param name="content"></param>
        public override void Load(ContentManager content)
        {
            NewGameRequested = false;
            ExitRequested = false;
            TitlescreenStatus = TitlescreenStatus.Start;
            /// Load up the game font
            font = content.Load<SpriteFont>("gameFont");
            /// Load the songs for the game
            menuSong = content.Load<Song>("sound/menuMusic");
            gameSong = content.Load<Song>("sound/gameMusic");
            /// Load the main menu texture
            var texture = content.Load<Texture2D>("mainMenu");
            Animation.Initialize(texture, Vector2.Zero, texture.Width, texture.Height, 1, 30, Color.White, 1f, true);
            MoveSpeed = 8.0f;
            MaxSpeed = 10.0f;
            RotationSpeed = 5.0f;
            CurrentSpeed = Vector2.Zero;
            WrapScreen = true;
            Position = new Vector2(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y);
        }

        /// <summary>
        /// Plays the music.
        /// </summary>
        /// <param name="music">The music.</param>
        private void PlayMusic(Song music)
        {
            /// Due to the way the MediaPlayer plays music,
            /// we have to catch the exception. Music will play when the game is not tethered
            try
            {
                MediaPlayer.Stop();
                /// Play the music
                MediaPlayer.Play(music);

                /// Loop the currently playing song
                MediaPlayer.IsRepeating = true;
            }
            catch 
            {
                /// Ignore...
            }
        }

        /// <summary>
        /// Update the entity. This is usually performed in the gameloop
        /// </summary>
        /// <param name="inputManager"></param>
        /// <param name="gameTime"></param>
        public override void Update(InputManager inputManager, GameTime gameTime)
        {
            /// Handle controls
            var keyboard = inputManager.CurrentKeyboardState;
            var gamepad = inputManager.CurrentGamePadState;

            if (TitlescreenStatus == Entities.TitlescreenStatus.Start)
            {
                if (keyboard.IsKeyDown(Keys.F1) ||
                    gamepad.Buttons.Y == ButtonState.Pressed)
                {
                    if (gamepad.Buttons.Y == ButtonState.Pressed)
                    {
                        isGamepad = true;
                    }
                    TitlescreenStatus = TitlescreenStatus.Help;
                }
                else if (keyboard.IsKeyDown(Keys.Space) ||
                    keyboard.IsKeyDown(Keys.Enter) ||
                    gamepad.Buttons.A == ButtonState.Pressed)
                {
                    if (Player.Alive)
                    {
                        /// Resume game
                        Visible = false;
                    }
                    else
                    {
                        /// Start new game
                        Visible = false;
                        NewGameRequested = true;
                    }
                }
                else if (inputManager.WasKeyPressed(Keys.Escape) ||
                    inputManager.WasButtonPressed(Buttons.Back))
                {
                    ExitRequested = true;
                }
            }
            else
            {
                if (inputManager.WasKeyPressed(Keys.Escape) ||
                    inputManager.WasButtonPressed(Buttons.Back))
                {
                    TitlescreenStatus = TitlescreenStatus.Start;
                }
            }

            base.Update(inputManager, gameTime);
        }

        /// <summary>
        /// Draw the animation to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            /// Depending on the title screen status, display the appropriate
            /// content.
            switch (TitlescreenStatus)
            {
                case TitlescreenStatus.Start:
                    WriteTitle(spriteBatch, "Captain Asteroids");
                    WriteSubTitle(spriteBatch, "New Game - Enter (A) ");
                    WriteSubSubTitle(spriteBatch, "Help - F1 (Y), Exit - Escape (Back)");
                    break;
                case TitlescreenStatus.GameOver:
                    WriteTitle(spriteBatch, "Game Over!");
                    WriteSubTitle(spriteBatch, string.Format("Score: {0}", Player.Score));
                    WriteSubSubTitle(spriteBatch, "Menu - Escape (Back)");
                    break;
                case TitlescreenStatus.Help:
                    WriteHelp(spriteBatch);
                    break;
                case Entities.TitlescreenStatus.Pause:
                    WriteTitle(spriteBatch, "Game Paused");
                    WriteSubTitle(spriteBatch, "Game - Enter (A)");
                    WriteSubSubTitle(spriteBatch, "Menu - Escape (Back)");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Writes the help page to the screen.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        private void WriteHelp(SpriteBatch spriteBatch)
        {
            List<string> texts = new List<string>();
            if (isGamepad)
            {
                texts.AddRange(new string[] 
                {
                    "Help",
                    "Use 'D-Pad' to move",
                    "Press 'A' to shoot",
                    "Press 'B' to hyperspace",
                    "",
                    "Professor: Norbert Mika",
                    "Course: SET Simulation and Game Development 2013",
                    "Students: Steve Davis and Hekar Khani",
                    "Press (Back) to go back"
                });
            }
            else
            {
                texts.AddRange(new string[] 
                {
                    "Help",
                    "Use 'Arrows' to move",
                    "Press 'H' to hyperspace",
                    "Press 'Spacebar' to shoot",
                    "",
                    "Professor: Norbert Mika",
                    "Course: SET Simulation and Game Development 2013",
                    "Students: Steve Davis and Hekar Khani",
                    "Press Escape to go back"
                });
            }

            int i = 1;
            /// Dynamically write text based on how many lines have
            /// been written
            foreach (var line in texts)
            {
                var offset = font.MeasureString(line);
                var pos = new Vector2
                (
                    GraphicsDevice.Viewport.X + 50, 
                    (offset.Y * i)
                );
                spriteBatch.DrawString(font, line, pos, Color.Green);
                i++;
            }
        }

        /// <summary>
        /// Writes the title.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="text">The text.</param>
        private void WriteTitle(SpriteBatch spriteBatch, string text)
        {
            var offset = font.MeasureString(text);
            var pos = new Vector2
            (
                GraphicsDevice.Viewport.X + (GraphicsDevice.Viewport.Width / 2) - (offset.X / 2),
                GraphicsDevice.Viewport.Y + (GraphicsDevice.Viewport.Height / 3) - (offset.Y / 2)
            );

            spriteBatch.DrawString(font, text, pos, Color.Green);
        }

        /// <summary>
        /// Writes the sub title.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="text">The text.</param>
        private void WriteSubTitle(SpriteBatch spriteBatch, string text)
        {
            var offset = font.MeasureString(text);
            var pos = new Vector2
            (
                GraphicsDevice.Viewport.X + (GraphicsDevice.Viewport.Width / 2) - (offset.X / 2),
                GraphicsDevice.Viewport.Y + (GraphicsDevice.Viewport.Height / 2) - (offset.Y / 2)
            );

            spriteBatch.DrawString(font, text, pos, Color.White);
        }

        /// <summary>
        /// Writes the sub sub title.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="text">The text.</param>
        private void WriteSubSubTitle(SpriteBatch spriteBatch, string text)
        {
            var offset = font.MeasureString(text);
            var pos = new Vector2
            (
                GraphicsDevice.Viewport.X + (GraphicsDevice.Viewport.Width / 2) - (offset.X / 2),
                GraphicsDevice.Viewport.Y + (GraphicsDevice.Viewport.Height / 1.25f) - (offset.Y / 2)
            );

            spriteBatch.DrawString(font, text, pos, Color.White);
        }
    }
}
