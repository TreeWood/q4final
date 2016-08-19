// GlobalEvents.cs

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Event_Managers
{
    public static class GlobalEvents
    {

        private static Game1 game;

        static GlobalEvents()
        {
            game = new Game1();
        }

        public static void exitGame()
        {
            game.Exit();
        }

        public static void setGameLevel(GameLevels.GameLevels gL)
        {
            game.setGameLevel(gL);
        }

    }
}
