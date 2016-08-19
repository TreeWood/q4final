// ButtonEvents.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Quarter4Project.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Event_Managers
{
    /// <summary>
    /// Performs updating and drawing.
    /// </summary>
    static class ButtonEvents
    {

        #region Fields

        private static MouseState mouseState, prevMouseState;
        private static Point mousePos;

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates buttons.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="buttonList">Buttons to be updated.</param>
        /// <param name="game">Game to perform events.</param>
        public static void Update(GameTime gameTime, List<ButtonFactory.Default> buttonList, Game1 game)
        {
            // Sets the mouse state.
            mouseState = Mouse.GetState();

            // Gets the position of the mouse.
            mousePos = new Point(mouseState.X, mouseState.Y);

            // Checks if the mouse is hovering over the button.
            foreach (ButtonFactory.Default b in buttonList)
            {
                if (b.collisionRectangle().Intersects(new Rectangle(mousePos.X, mousePos.Y, 1, 1)))
                {
                    // Perform hover events.

                    // Checks if the mouse is being clicked.
                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        performClickEvents(b.eventNum, b.windowNum, game);
                    }
                }
            }

            // Sets the previous mouse state.
            prevMouseState = mouseState;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="buttonList"></param>
        /// <param name="game"></param>
        public static void Update(GameTime gameTime, List<ButtonFactory.Texture> buttonList, Game1 game)
        {
            mouseState = Mouse.GetState();
            mousePos = new Point(mouseState.X, mouseState.Y);

            for (int i = 0; i < buttonList.Count; i++)
                buttonList[i].Update(gameTime);

            for (int i = 0; i < buttonList.Count; i++)
            {
                if (buttonList[i].collisionRect().Intersects(new Rectangle(mousePos.X, mousePos.Y, 1, 1)))
                {
                    if (buttonList[i].sprites.Count(p => p.name == "HOVER") >= 1)
                        buttonList[i].setAnimation("HOVER");

                    if (mouseState.LeftButton == ButtonState.Pressed)
                    {
                        for (int j = 0; j < buttonList.Count; j++)
                        {
                            if (game.getMenuManager().getWindowList().Count(w => w.collisionRect().Intersects(new Rectangle(mousePos.X, mousePos.Y, 1, 1))) == 0)
                            {
                                performClickEvents(buttonList[i].eventNum, buttonList[i].windowNum, game);
                            }
                        }                    
                    }
                }
                else
                {
                    if (buttonList[i].sprites.Count(p => p.name == "IDLE") >= 1)
                        buttonList[i].setAnimation("IDLE");
                }
            }

            prevMouseState = mouseState;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="gameTime"></param>
        /// <param name="buttonList"></param>
        public static void Draw(SpriteBatch spriteBatch, GameTime gameTime, List<ButtonFactory.Texture> buttonList)
        {
            for (int i = 0; i < buttonList.Count; i++)
                buttonList[i].Draw(gameTime, spriteBatch);
        }

        /// <summary>
        /// Draws buttons.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <param name="spriteBatch">Draws the buttons.</param>
        /// <param name="buttonList">Button to be drawn.</param>
        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, List<ButtonFactory.Default> buttonList)
        {
            if (buttonList != null)
            {
                foreach (ButtonFactory.Default b in buttonList)
                {
                    // Draw border
                    if (b.borderSize != Vector2.Zero)
                        spriteBatch.Draw(b.borderTex, new Rectangle((int)(b.pos.X - (b.borderSize.X / 2)), (int)(b.pos.Y - (b.borderSize.Y / 2)), (int)(b.collisionRectangle().Width + b.borderSize.X), (int)(b.collisionRectangle().Height + b.borderSize.Y)), Color.White);

                    // Draw button background
                    spriteBatch.Draw(b.tex, b.collisionRectangle(), Color.White);

                    // Draw text shadow
                    if (b.textShadowPos != Vector2.Zero)
                        spriteBatch.DrawString(b.fontFamily, b.text, new Vector2(b.getButtonCenter().X - (b.fontFamily.MeasureString(b.text).Length() / 2) + 1 + b.textShadowPos.X, b.getButtonCenter().Y - (b.fontFamily.MeasureString("X").Y / 2) + 1 + b.textShadowPos.Y), b.textShadowColor);

                    // Draw text
                    spriteBatch.DrawString(b.fontFamily, b.text, new Vector2(b.getButtonCenter().X - (b.fontFamily.MeasureString(b.text).Length() / 2) + 1, b.getButtonCenter().Y - (b.fontFamily.MeasureString("X").Y / 2) + 1), b.textColor);

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        /// <param name="buttonList"></param>
        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, List<ButtonFactory.Texture> buttonList)
        {
            if (buttonList != null)
            {
                foreach (ButtonFactory.Texture b in buttonList)
                    spriteBatch.Draw(b.tex, b.collisionRectangle(), Color.White);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Performs the events stored in buttons.
        /// </summary>
        /// <param name="eventNum">Event to be completed.</param>
        /// <param name="game">Helper to complete events.</param>
        private static void performClickEvents(int eventNum, int windowNum, Game1 game)
        {
            switch (eventNum)
            {
                default:
                case 0: // Complete nothing.
                    break;
                case 1: // Closes game.
                    game.Exit();
                    break;
                case 2: // Switch to menumanager.
                    game.setGameLevel(GameLevels.GameLevels.Menu);
                    break;
                case 3: // Switch to gamemanager.
                    game.setGameLevel(GameLevels.GameLevels.Game);
                    break;
                case 4: // Opens debug.
                    game.setGameLevel(GameLevels.GameLevels.Debug);
                    break;
                case 5: // Switch to level editor.
                    game.setGameLevel(GameLevels.GameLevels.Editor);
                    break;
                case 6: // Does something very terrible.
                    game.getGameManager().getCurrentMap().mapTiles[0].setPos(new Vector2(10, 10));
                    break;
                case 7: // Sets the window to invisible.
                    foreach (WindowFactory.Okay w in game.getMenuManager().getWindowList())
                    {
                        if (w.vis && windowNum == w.windowNum)
                        {
                            w.setVisibility(false);
                            w.setPos(new Vector2((game.GraphicsDevice.Viewport.Width / 2) - (w.size.X / 2), (game.GraphicsDevice.Viewport.Height / 2) - (w.size.Y / 2)));
                            // Change button position before we move the window.
                            foreach (ButtonFactory.Default b in w.buttonList)
                                b.setPos(b.origPos);
                        }
                    }
                    break;
                case 8: // Sets the window to visible
                    foreach (WindowFactory.Okay w in game.getMenuManager().getWindowList())
                    {
                        if (!w.vis && windowNum == w.windowNum)
                            w.setVisibility(true);

                        showWindow(windowNum, game.getMenuManager().getWindowList(), game);
                    }
                    break;
            }
        }

        public static void showWindow(int windowNum, List<WindowFactory.Okay> windowList, Game1 game)
        {
            foreach (WindowFactory.Okay w in windowList)
            {
                int wOrder = int.MinValue;

                if (windowNum == w.windowNum)
                {
                    wOrder = w.order;
                    w.setOrder(game.getMenuManager().getWindowList().Count - 1);
                }

                if (wOrder == int.MinValue)
                    return;

                if (w.windowNum != windowNum && w.order > 0 && w.order >= wOrder)
                {
                    w.setOrder(w.order - 1);
                }
            }
        }

        /// <summary>
        /// Completes events upon user hover.
        /// </summary>
        /// <param name="hoverNum"></param>
        private static void performHoverEvents(int hoverNum)
        {

        }

        public static void buttonPressed(ButtonFactory.Default button, Game1 game)
        {
            performClickEvents(button.eventNum, button.windowNum, game);
        }

        /// <summary>
        /// Gets the mouse position.
        /// </summary>
        /// <returns>Rectangle format of the mouse position.</returns>
        public static Rectangle getMousePos()
        {
            return new Rectangle(mousePos.X, mousePos.Y, 1, 1);
        }

        #endregion

    }
}
