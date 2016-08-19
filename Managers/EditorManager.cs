// EditorManager.cs

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Managers
{
    class EditorManager : DrawableGameComponent
    {

        Game1 game;

        public EditorManager(Game1 g)
            : base(g)
        {
            game = g;
        }

    }
}
