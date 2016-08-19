// DebugManager.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarter4Project.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Managers
{
    class DebugManager : DrawableGameComponent
    {

        #region Fields

        Game1 game;
        SpriteBatch spriteBatch;

        Texture2D background;
        SpriteFont Consolas;

        #endregion

        #region Initialization

        public DebugManager(Game1 g)
            : base(g)
        {
            game = g;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // Texture2Ds
            background = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            background.SetData<Color>(new Color[] { new Color(0, 0, 0, .7f) });

            // Fonts
            Consolas = Game.Content.Load<SpriteFont>(@"Fonts/Consolas");

            base.LoadContent();
        }

        #endregion

        #region Draw

        public override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            // Debug Background
            spriteBatch.Draw(background, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);

            // Numbers
            spriteBatch.DrawString(Consolas, "FPS: " + (Math.Round(1 / (float)gameTime.ElapsedGameTime.TotalSeconds).ToString()), new Vector2(10, Consolas.MeasureString("1").Y * 10 + 10), Color.White);

            // Game State
            spriteBatch.DrawString(Consolas, "Current Level: " + game.currentLevel.ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(Consolas, "Previous Level: " + game.previousLevel.ToString(), new Vector2(10, Consolas.MeasureString("1").Y + 10), Color.White);

            // Game Maps
            spriteBatch.DrawString(Consolas, "Maps:\n", new Vector2(10, (Consolas.MeasureString("1").Y * 2) + 10), Color.White);
            spriteBatch.DrawString(Consolas, "Current Map: Map id: " + game.getGameManager().getCurrentMap().id.ToString() + " Floor: " + game.getGameManager().getCurrentMap().floor.ToString(), new Vector2(10, (Consolas.MeasureString("1").Y * 3) + 10), Color.White);
            for (int i = 0; i < game.getGameManager().getMaps().Count; i++)
                spriteBatch.DrawString(Consolas, "\nMap id: " + game.getGameManager().getMaps()[i].id + " Floor: " + game.getGameManager().getMaps()[i].floor, new Vector2(10, (Consolas.MeasureString("1").Y * (i + 3)) + 10), Color.White);
            spriteBatch.DrawString(Consolas, "HP: " + game.getGameManager().getPlayer().getHP().ToString(), new Vector2(10, (Consolas.MeasureString("1").Y * 11) + 10), Color.White);
            spriteBatch.DrawString(Consolas, "Max Ammo: " + game.getGameManager().getPlayer().getCurrentWeapon().getMaxAmmo().ToString(), new Vector2(10, (Consolas.MeasureString("1").Y * 12) + 10), Color.White);
            spriteBatch.DrawString(Consolas, "Spare Ammo: " + game.getGameManager().getPlayer().getCurrentWeapon().getSpareAmmo().ToString(), new Vector2(10, (Consolas.MeasureString("1").Y * 13) + 10), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion

    }
}
