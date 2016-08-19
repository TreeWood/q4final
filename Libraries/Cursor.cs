// Cursor.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Quarter4Project.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Libraries
{
    class Cursor : Animation
    {

        #region Fields

        MouseState mouseState, prevMouseState;
		Camera camera;

        #endregion

        #region Initialization

        public Cursor(Texture2D tex, Vector2 position, Camera mycamera)
            : base(position)
        {
            camera = mycamera;
            addAnimations(tex);
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            position = new Vector2(mouseState.X, mouseState.Y) + camera.Position;

            prevMouseState = mouseState;
            base.Update(gameTime);
        }

        #endregion

        #region Methods

        public override void addAnimations(Texture2D tex)
        {
            addAnimation("1", tex, new Point(20, 20), new Point(0, 0), new Point(0, 0), new Point(0, 0), 0, Color.White);
            setAnimation("1");
        }

        #endregion

    }
}
