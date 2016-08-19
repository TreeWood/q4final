using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Quarter4Project.Libraries;
using Quarter4Project.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Factories
{
    class DropFactory : Animation
    {

        #region Fields

        GameManager game;
        WeaponFactory wep;
        KeyboardState keyboardState, prevKeyboardState;

        #endregion

        #region Initialization

        public DropFactory(Texture2D tex, Vector2 pos, GameManager g, WeaponFactory weapon)
            : base(pos)
        {
            rotationCenter = new Vector2(15, 10);
            addAnimations(tex);
            game = g;
            wep = weapon;
            setAnimation(wep.getID().ToString());
            Random r = new Random();
            rotation = r.Next(360) * (float)Math.PI / 180;
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            prevKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        #endregion

        #region Accessors

        public WeaponFactory getWep()
        {
            return wep;
        }

        #endregion

        #region Overrides

        public override void addAnimations(Texture2D tex)
        {
            addAnimation("1000", tex, new Point(30, 20), new Point(0, 0), new Point(0, 0), new Point(0, 0), 0, Color.White);
            addAnimation("2000", tex, new Point(30, 20), new Point(0, 0), new Point(0, 0), new Point(0, 0), 0, Color.White);
            addAnimation("3000", tex, new Point(30, 20), new Point(0, 0), new Point(1, 0), new Point(1, 0), 0, Color.White);
            addAnimation("4000", tex, new Point(30, 20), new Point(0, 0), new Point(2, 0), new Point(2, 0), 0, Color.White);
            addAnimation("5000", tex, new Point(30, 20), new Point(0, 0), new Point(3, 0), new Point(3, 0), 0, Color.White);
            addAnimation("6000", tex, new Point(30, 20), new Point(0, 0), new Point(4, 0), new Point(4, 0), 0, Color.White);
        }

        #endregion

    }
}
