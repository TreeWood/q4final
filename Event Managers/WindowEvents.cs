// WindowEvents.cs

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
    static class WindowEvents
    {

        private static MouseState mouseState, 
                                  prevMouseState;
        private static Vector2 posDisplacement,
                               sizeDisplacement,
                               newPos, 
                               mousePos;
        private static Game1 game;

        public static void Update(GameTime gameTime, List<WindowFactory.Okay> windowList, Game1 g)
        {
            mouseState = Mouse.GetState();
            mousePos = new Vector2(mouseState.X, mouseState.Y);
            game = g;

            foreach (WindowFactory.Okay w in windowList)
            {
                if (w.buttonList != null && w.vis)
                    ButtonEvents.Update(gameTime, w.buttonList, g);
            }

            // Update movement.
            updateWindowMovement(windowList);

            prevMouseState = mouseState;
        }

        public static void updateWindowMovement(List<WindowFactory.Okay> windowList)
        {           

            for (int i = 0; i < windowList.Count; i++)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    ButtonEvents.showWindow(windowList[i].windowNum, windowList, game);

                if (windowList[i].collisionRect().Intersects(new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1)) && windowList[i].vis)
                {
                    if (prevMouseState.LeftButton == ButtonState.Released)
                    {
                        // Displacements from the mouse to the window corners so we can check for any collisions.
                        posDisplacement = new Vector2(mousePos.X - windowList[i].pos.X, mousePos.Y - windowList[i].pos.Y);
                        sizeDisplacement = new Vector2((windowList[i].pos.X + windowList[i].size.X) - mousePos.X, (windowList[i].pos.Y + windowList[i].size.Y) - mousePos.Y);
                    }

                    if (mouseState.LeftButton == ButtonState.Pressed && windowList[i].vis == true && windowList.Any(w => w.buttonList.Any(b => new Rectangle((int)b.pos.X, (int)b.pos.Y, b.size.X, b.size.Y).Intersects(new Rectangle((int)mousePos.X, (int)mousePos.Y, 1, 1)))) == false)
                    {
                        // Make sure the window doesn't go out of view.
                        if ((int)mousePos.X - posDisplacement.X < 0 - (windowList[i].size.X / 2))
                            newPos.X = windowList[i].pos.X;
                        else if ((int)mousePos.X + sizeDisplacement.X > game.GraphicsDevice.Viewport.Width + (windowList[i].size.X / 2))
                            newPos.X = windowList[i].pos.X;
                        else
                            newPos.X = mousePos.X - posDisplacement.X;

                        if ((int)mousePos.Y - posDisplacement.Y < 0 - (windowList[i].size.Y / 2))
                            newPos.Y = windowList[i].pos.Y;
                        else if ((int)mousePos.Y + sizeDisplacement.Y > game.GraphicsDevice.Viewport.Height + (windowList[i].size.Y / 2))
                            newPos.Y = windowList[i].pos.Y;
                        else
                            newPos.Y = mousePos.Y - posDisplacement.Y;

                        // Change button position before we move the window.
                        foreach (ButtonFactory.Default b in windowList[i].buttonList)
                            b.setPos(new Vector2(newPos.X + (b.pos.X - windowList[i].pos.X), newPos.Y + (b.pos.Y - windowList[i].pos.Y)));

                        // Move the window.
                        if (windowList[i].color == Color.Transparent)
                        {
                            windowList[i].setNewValues(newPos, windowList[i].size, windowList[i].tex, windowList[i].font, windowList[i].text, windowList[i].textColor, windowList[i].textShadowPos, windowList[i].textShadowColor, windowList[i].vis);

                        }
                        else
                        {
                            windowList[i].setNewValues(newPos, windowList[i].size, windowList[i].color, game.GraphicsDevice, windowList[i].font, windowList[i].text, windowList[i].textColor, windowList[i].textShadowPos, windowList[i].textShadowColor, windowList[i].borderSize, windowList[i].borderColor, windowList[i].vis);
                        }
                    }
                }
                else
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                    {
                        foreach (WindowFactory.Okay w in windowList)
                        {
                            // if (w.vis && game.IsActive) ButtonEvents.buttonPressed(w.buttonList[0], game);
                        }
                    }
                }
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, List<WindowFactory.Okay> windowList)
        {
            foreach (var w in windowList.OrderBy(i => i.order))
            {
                if (w.vis)
                {
                    spriteBatch.Draw(w.tex, new Rectangle((int)w.pos.X, (int)w.pos.Y, w.size.X, w.size.Y), Color.White);
                    spriteBatch.DrawString(w.font, w.text, new Vector2(w.pos.X + 30, w.pos.Y + 30), w.textColor);
                    ButtonEvents.Draw(gameTime, spriteBatch, w.buttonList);
                }
            }
        }

    }
}
