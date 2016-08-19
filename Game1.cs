using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Quarter4Project.Managers;

namespace Quarter4Project
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        #region Constants

        // What's the width of the window?
        private const int windowWidth = 960;

        // What's the height of the window?
        private const int windowHeight = 540;

        #endregion

        #region Fields

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SplashManager splashManager;
        DebugManager debugManager;
        MenuManager menuManager;
        GameManager gameManager;
        EditorManager editorManager;

        public GameLevels.GameLevels currentLevel { get; private set; }
        public GameLevels.GameLevels previousLevel { get; private set; }

        KeyboardState keyboardState, prevKeyboardState;

        #endregion

        #region Initialization

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            splashManager = new SplashManager(this);
            Components.Add(splashManager);
            splashManager.DrawOrder = 1;

            debugManager = new DebugManager(this);
            Components.Add(debugManager);
            debugManager.DrawOrder = 2;
            debugManager.Visible = false;

            menuManager = new MenuManager(this);
            Components.Add(menuManager);
            menuManager.DrawOrder = 1;

            gameManager = new GameManager(this);
            Components.Add(gameManager);
            gameManager.DrawOrder = 1;

            editorManager = new EditorManager(this);
            Components.Add(editorManager);
            editorManager.DrawOrder = 1;

            setGameLevel(GameLevels.GameLevels.Splash);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.F5) && prevKeyboardState.IsKeyUp(Keys.F5) && debugManager.Visible == false)
                setGameLevel(GameLevels.GameLevels.Debug);
            else if (keyboardState.IsKeyDown(Keys.F5) && prevKeyboardState.IsKeyUp(Keys.F5) && debugManager.Visible == true)
                debugManager.Visible = false;

            prevKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the game screen.
        /// </summary>
        /// <param name="gL">Screen to be set to.</param>
        public void setGameLevel(GameLevels.GameLevels gL)
        {
            GameLevels.GameLevels pL = previousLevel;
            splashManager.Enabled = false;
            splashManager.Visible = false;
            menuManager.Enabled = false;
            menuManager.Visible = false;
            gameManager.Enabled = false;
            gameManager.Visible = false;
            editorManager.Enabled = false;
            editorManager.Visible = false;

            switch (gL)
            {
                case GameLevels.GameLevels.Splash:
                    splashManager.Enabled = true;
                    splashManager.Visible = true;
                    previousLevel = currentLevel;
                    currentLevel = gL;
                    break;
                case GameLevels.GameLevels.Debug:
                    debugManager.Visible = true;
                    setGameLevel(currentLevel);
                    previousLevel = pL;
                    break;
                case GameLevels.GameLevels.Menu:
                    menuManager.Enabled = true;
                    menuManager.Visible = true;
                    previousLevel = currentLevel;
                    currentLevel = gL;
                    IsMouseVisible = true;
                    break;
                case GameLevels.GameLevels.Game:
                    gameManager.Enabled = true;
                    gameManager.Visible = true;
                    previousLevel = currentLevel;
                    currentLevel = gL;
                    IsMouseVisible = false;
                    break;
                case GameLevels.GameLevels.Editor:
                    editorManager.Enabled = true;
                    editorManager.Visible = true;
                    previousLevel = currentLevel;
                    currentLevel = gL;
                    break;
            }
        }

        /// <summary>
        /// Gets the game manager.
        /// </summary>
        /// <returns>Game Manager</returns>
        internal GameManager getGameManager()
        {
            return gameManager;
        }

        internal MenuManager getMenuManager()
        {
            return menuManager;
        }

        #endregion

    }
}