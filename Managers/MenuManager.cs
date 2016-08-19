using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Quarter4Project.Event_Managers;
using Quarter4Project.Factories;
using Quarter4Project.Libraries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Managers
{
    class MenuManager : DrawableGameComponent
    {

        #region Constants

        #region Directories

        // What font is being used for the generated buttons?
        private const string buttonFontDir = "Fonts/Consolas";

        #endregion

        #endregion

        #region Fields

        Game1 game;
        SpriteBatch spriteBatch;

        SpriteFont buttonFont;
        Texture2D backgroundImage;
        MouseState mouseState, prevMouseState;
        Point mousePos;

        List<ButtonFactory.Default> buttonList;
        List<ButtonFactory.Texture> btnList;
        List<ButtonFactory.Texture.TextureAnimation> btnAnimList;
        Texture2D menuButtons;
        SpriteFont fixedSys;

        List<WindowFactory.Okay> windowList;
        List<ButtonFactory.Default> windowButtonList;
        Texture2D windowBG;

        private List<Vector2> moveMag;
        private List<Vector2> move;
        private List<Camera> camList;
        private float timer = 0;
        Random r;
        List<Cursor> cursor;
        Texture2D cursorTex;

        Color colorFlow;
        float p;
        private int a, b, c, d, e;
        private float timer2;

        #endregion

        #region Initialization

        public MenuManager(Game1 g)
            : base(g)
        {
            game = g;
        }

        public override void Initialize()
        {
            moveMag = move = new List<Vector2>();
            camList = new List<Camera>();
            cursor = new List<Cursor>();

            buttonList = new List<ButtonFactory.Default>();
            btnList = new List<ButtonFactory.Texture>();
            btnAnimList = new List<ButtonFactory.Texture.TextureAnimation>();

            windowList = new List<WindowFactory.Okay>();
            windowButtonList = new List<ButtonFactory.Default>();

            r = new Random();

            base.Initialize();

            // Editor button
            buttonList.Add(new ButtonFactory.Default(new Vector2(100, 450), new Point(75, 55), new Color(100, 100, 255), GraphicsDevice, buttonFont, "Editor", new Color(255, 255, 255), new Vector2(2, 2), new Color(0, 0, 0), new Vector2(4, 4), new Color(0, 0, 0), 0, 8, 1));

            // Play button
            btnAnimList.Add(new ButtonFactory.Texture.TextureAnimation("IDLE", menuButtons, new Point(130, 80), new Point(0, 0), new Point(5, 5), new Point(5, 5), 0, Color.Transparent));
            btnList.Add(new ButtonFactory.Texture(btnAnimList, new Vector2(310, 430), 0, 3, 0));
            btnAnimList.Clear(); // Button animations list must be cleared before you can begin creating a new animated button using a spritesheet.

            // Quit button
            btnAnimList.Add(new ButtonFactory.Texture.TextureAnimation("IDLE", menuButtons, new Point(130, 80), new Point(0, 0), new Point(5, 5), new Point(5, 5), 0, Color.Transparent));
            btnList.Add(new ButtonFactory.Texture(btnAnimList, new Vector2(510, 430), 0, 8, 2));
            btnAnimList.Clear();

            // Window for build editor
            windowList.Add(new WindowFactory.Okay(new Vector2((GraphicsDevice.Viewport.Width / 2) - (450 / 2), (GraphicsDevice.Viewport.Height / 2) - (200 / 2)), new Point(450, 200), windowBG, fixedSys, "The build editor is currently \nunavailable.", new Color(255, 255, 255), new Vector2(2, 2), new Color(0, 0, 0), false, 1));
            windowButtonList.Add(new ButtonFactory.Default(new Vector2(windowList[0].pos.X + windowList[0].size.X - 55, windowList[0].pos.Y + 15), new Point(25, 25), Color.Transparent, GraphicsDevice, fixedSys," x", new Color(183, 116, 53), new Vector2(2, 2), new Color(0, 0, 0), new Vector2(0, 0), new Color(0, 0, 0), 0, 7, 1));
            windowButtonList.Add(new ButtonFactory.Default(new Vector2(windowList[0].pos.X + (windowList[0].size.X / 2) - (int)37.5, windowList[0].pos.Y + (windowList[0].size.Y) - 45), new Point(75, 35), Color.Transparent, GraphicsDevice, fixedSys, "Ok", new Color(255, 255, 255), new Vector2(2, 2), new Color(0, 0, 0), new Vector2(0, 0), new Color(0, 0, 100), 0, 7, 1));
            windowList[0].setButtons(windowButtonList);
            windowButtonList.Clear();

            // Window for exit
            windowList.Add(new WindowFactory.Okay(new Vector2((GraphicsDevice.Viewport.Width / 2) - (450 / 2), (GraphicsDevice.Viewport.Height / 2) - (200 / 2)), new Point(450, 200), new Color(100, 100, 225), GraphicsDevice, buttonFont, "The game will now exit.", new Color(255, 255, 255), new Vector2(2, 2), new Color(0, 0, 0), new Vector2(4, 4), new Color(255, 255, 255), false, 2));
            windowButtonList.Add(new ButtonFactory.Default(new Vector2(windowList[0].pos.X + windowList[0].size.X - 25, windowList[0].pos.Y), new Point(25, 25), new Color(155, 100, 100), GraphicsDevice, buttonFont, " X", new Color(255, 255, 255), new Vector2(2, 2), new Color(255, 0, 0), new Vector2(0, 0), new Color(0, 0, 0), 0, 7, 2));
            windowButtonList.Add(new ButtonFactory.Default(new Vector2(windowList[0].pos.X + (windowList[0].size.X / 2) - (int)37.5, windowList[0].pos.Y + (windowList[0].size.Y) - 45), new Point(75, 35), new Color(110, 100, 235), GraphicsDevice, buttonFont, "Ok", new Color(255, 255, 255), new Vector2(2, 2), new Color(0, 0, 0), new Vector2(0, 0), new Color(0, 0, 0), 0, 1, 2));
            windowList[1].setButtons(windowButtonList);
            windowButtonList.Clear();

            for (int i = 0; i < 7; i++)
            {
                camList.Add(new Camera());
            }

            cursor.Add(new Cursor(cursorTex, new Vector2(0, 0), camList[0]));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load Fonts
            buttonFont = Game.Content.Load<SpriteFont>(buttonFontDir);
            fixedSys = Game.Content.Load<SpriteFont>(@"Fonts/FixedSys");

            // Button textures
            menuButtons = Game.Content.Load<Texture2D>(@"Sprites/menuTexture");

            // Background
            backgroundImage = Game.Content.Load<Texture2D>(@"Backgrounds/Background");

            // Windows
            windowBG = Game.Content.Load<Texture2D>(@"Images/windowBG");

            cursorTex = Game.Content.Load<Texture2D>(@"Images/cursor");

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            mousePos = new Point(mouseState.X - (GraphicsDevice.Viewport.Width / 2), mouseState.Y - (GraphicsDevice.Viewport.Height / 2));

            ButtonEvents.Update(gameTime, buttonList, game);
            ButtonEvents.Update(gameTime, btnList, game);
            WindowEvents.Update(gameTime, windowList, game);

            timer2 += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer2 >= 0.5f)
            {
                c = ((int)r.Next(0, 255) + 1);
                d = ((int)r.Next(0, 255) + 1);
                e = ((int)r.Next(0, 255) + 1);
                timer2 = 0.0f;
            }

            p += .05f;

            if (p < 1)
                colorFlow = LerpColor(Color.Red, Color.Magenta, p);
            else if (p < 2)
                colorFlow = LerpColor(Color.Magenta, Color.Blue, p - 1.0f);
            else if (p < 3)
                colorFlow = LerpColor(Color.Blue, Color.Cyan, p - 2.0f);
            else if (p < 4)
                colorFlow = LerpColor(Color.Cyan, Color.Green, p - 3.0f);
            else if (p < 5)
                colorFlow = LerpColor(Color.Green, Color.Yellow, p - 4.0f);
            else if (p < 6)
                colorFlow = LerpColor(Color.Yellow, Color.Red, p - 5.0f);
            else if (p > 6)
                p = 0;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundImage, new Rectangle(0, 0, 960, 540), colorFlow);
            WindowEvents.Draw(gameTime, spriteBatch, windowList);   

            spriteBatch.End();

            for (int i = 0; i < camList.Count; i++)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, null, null, null, null, camList[i].getMatrix(Vector2.One));

                ButtonEvents.Draw(gameTime, spriteBatch, buttonList);
                ButtonEvents.Draw(spriteBatch, gameTime, btnList);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        #endregion

        #region Accessors

        public List<WindowFactory.Okay> getWindowList()
        {
            return windowList;
        }

        public List<ButtonFactory.Default> getButtonsList()
        {
            return buttonList;
        }

        

        #endregion

        #region Functions

        /// <summary>
        /// Returns the next color between color 'a' and color 'b' at the 'percentage' point.
        /// </summary>
        /// <param name="a">Start Color</param>
        /// <param name="b">End Color</param>
        /// <param name="percentage">Position of color in relation to starting color 'a' and ending color 'b'</param>
        /// <returns>Next color inline with percentage</returns>
        private Color LerpColor(Color a, Color b, float percentage)
        {
            return new Color(
                (byte)MathHelper.Lerp(a.R, b.R, percentage),
                (byte)MathHelper.Lerp(a.G, b.G, percentage),
                (byte)MathHelper.Lerp(a.B, b.B, percentage),
                (byte)MathHelper.Lerp(a.A, b.A, percentage));
        }

        /// <summary>
        /// Gets a random double based off the parameters
        /// </summary>
        /// <param name="minimum">Minimum amount the random will return.</param>
        /// <param name="maximum">Maximum amount the random will return.</param>
        /// <returns>Returns a double between the minimum and maximum parameters</returns>
        public double NextDouble(double minimum, double maximum)
        {
            return r.NextDouble() * (maximum - minimum) + minimum;
        }

        #endregion

    }
}
