///
///FILE          : Asteroids.cs
///PROJECT       : Asteroids
///PROGAMMER     : Stephen Davis/Hekar Khani
///FIRST VERSION : Mar 19th 2013
///DESCRIPTION   : This is basically the main game.
///                 It initilizes everything and starts the game loop.
///
using System.Collections.Generic;
using Asteroids_Xbox.Entities;
using Asteroids_Xbox.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids_Xbox
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Asteroids : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        /// Managers
        private GameManager asteroidManager;
        private InputManager inputManager;
        private EntityManager entityManager;

        /// Screens
        private Titlescreen titleScreen;

        /// Entities

        private Player player;

        public Asteroids()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Load the game
        /// </summary>
        protected override void Initialize()
        {
            NewGame();

            base.Initialize();
        }

        /// <summary>
        /// Load the game content
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            titleScreen.Initialize(Content, GraphicsDevice);
            entityManager.Initialize(Content, GraphicsDevice);

            titleScreen.Visible = true;
        }

        /// <summary>
        /// Unload the game content
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            /// Gets the keyboard state and manages.
            inputManager.PreviousKeyboardState = inputManager.CurrentKeyboardState;
            inputManager.CurrentKeyboardState = Keyboard.GetState();

            inputManager.PreviousGamePadState = inputManager.CurrentGamePadState;
            inputManager.CurrentGamePadState = GamePad.GetState(PlayerIndex.One);
            if (titleScreen.Visible)
            {
                titleScreen.Update(inputManager, gameTime);
                if (titleScreen.NewGameRequested)
                {
                    NewGame();
                    titleScreen.NewGameRequested = false;
                    return;
                }
                else if (titleScreen.ExitRequested)
                {
                    this.Exit();
                }
            }
            else
            {
                var exitPressed = inputManager.WasKeyPressed(Keys.Escape) ||
                    inputManager.WasButtonPressed(Buttons.Back);
                /// Show titlescreen/Pause game
                if (exitPressed)
                {
                    /// Game is paused
                    titleScreen.TitlescreenStatus = TitlescreenStatus.Pause;
                    titleScreen.Visible = true;
                }
                else if (!player.Alive)
                {
                    /// Gameover
                    titleScreen.TitlescreenStatus = TitlescreenStatus.GameOver;
                    titleScreen.Visible = true;
                }
                else
                {
                    asteroidManager.Update(Content, GraphicsDevice, player, gameTime);
                    entityManager.Update(inputManager, gameTime);
                }
            }
            base.Update(gameTime);
        }
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (titleScreen.Visible)
            {
                titleScreen.Draw(spriteBatch);
            }
            else
            {
                entityManager.Draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Initialize the new game. Creates any entities that need to be created
        /// in order to run the game.
        /// </summary>
        private void NewGame()
        {
            if (entityManager == null)
            {
                /// Create a new entity manager.
                entityManager = new EntityManager(Content, GraphicsDevice);
            }
            else
            {
                entityManager.Clear();
            }

            if (inputManager == null)
            {
                inputManager = new InputManager();
            }
            /// Create a new player.
            player = new Player(entityManager);
            if (asteroidManager == null)
            {
                asteroidManager = new GameManager(entityManager, player);
            }
            else
            {
                asteroidManager.Clear();
            }
            if (titleScreen == null)
            {
                /// Create a new title screen
                titleScreen = new Titlescreen(player);
            }
            else
            {
                titleScreen.Player = player;
            }
            entityManager.Add(new Background());
            entityManager.Add(player);
            entityManager.Add(new ScoreDisplay(new List<Player>(new Player[] { player })));
        }

    }
}
