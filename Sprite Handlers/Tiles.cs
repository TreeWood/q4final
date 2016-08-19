// Tiles.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarter4Project.Libraries;
using Quarter4Project.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Sprite_Handlers
{
    class Tiles : Animation
    {

        #region Fields

        private char[] isBackground = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' },
                             isObject = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' },
                             isPortal = { '!' },
                             isWall = { '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        #endregion

        #region Initialization

        public Tiles(Texture2D tileSheet, Vector2 pos, GameManager g, string tileName, List<string> tileList)
            : base(pos)
        {
            addAnimations(tileSheet);
            position = pos;
            Point pos2 = new Point((int)pos.X / 15, (int)pos.Y / 15);

            if (isWall.Any(w => w.ToString() == tileName))
            {
                // Set default to horizontal animation
                setAnimation(tileName.ToString() + ".1.1");

                updateWalls(tileList, pos2, tileName);
            }
            else if (isBackground.Any(b => b.ToString() == tileName))
            {
                setAnimation(tileName);
            }
            else if (isObject.Any(o => o.ToString() == tileName))
            {
                setAnimation(tileName);
            }
            else if (isPortal.Any(p => p.ToString() == tileName))
            {
                setAnimation(tileName);
            }
        }

        #endregion

        #region Update

        private void updateWalls(List<string> tileList, Point pos2, string tileName)
        {
            // Check if there is a tile above and below.
            bool isVertical = isWall.Any(w => w == tileList[pos2.Y + 1][pos2.X]) || isWall.Any(w => w == tileList[pos2.Y - 1][pos2.X]),
                // Check if there is a tile left and right.
            isHorizontal = isWall.Any(w => w == tileList[pos2.Y][pos2.X + 1]) || isWall.Any(w => w == tileList[pos2.Y][pos2.X - 1]),
                // Check if there is a tile below.
            isTopCorner = isWall.Any(w => w == tileList[pos2.Y + 1][pos2.X]),
                // Check if there is a tile above.
            isBottomCorner = isWall.Any(w => w == tileList[pos2.Y - 1][pos2.X]),
                // Check if there is a tile to the right.
            isLeft = isWall.Any(w => w == tileList[pos2.Y][pos2.X + 1]),
                // Check if there is a tile to the left.
            isRight = isWall.Any(w => w == tileList[pos2.Y][pos2.X - 1]),
                // Check if there are tiles on left, right, top sides.
            is3WayNorth = isWall.Any(w => w == tileList[pos2.Y - 1][pos2.X]) && isWall.Any(w => w == tileList[pos2.Y][pos2.X - 1]) && isWall.Any(w => w == tileList[pos2.Y][pos2.X + 1]),
                // Check if there are tiles on left, right, bottom sides.
            is3WaySouth = isWall.Any(w => w == tileList[pos2.Y + 1][pos2.X]) && isWall.Any(w => w == tileList[pos2.Y][pos2.X - 1]) && isWall.Any(w => w == tileList[pos2.Y][pos2.X + 1]),
                // Check if there are tiles on right, top, bottom sides.
            is3WayWest = isWall.Any(w => w == tileList[pos2.Y][pos2.X + 1]) && isWall.Any(w => w == tileList[pos2.Y + 1][pos2.X]) && isWall.Any(w => w == tileList[pos2.Y - 1][pos2.X]),
                // Check if there are tiles on left, top, bottom sides.
            is3WayEast = isWall.Any(w => w == tileList[pos2.Y][pos2.X - 1]) && isWall.Any(w => w == tileList[pos2.Y + 1][pos2.X]) && isWall.Any(w => w == tileList[pos2.Y - 1][pos2.X]),
                // Check if there are tiles on all sides.
            is4Way = isWall.Any(w => w == tileList[pos2.Y - 1][pos2.X]) && isWall.Any(w => w == tileList[pos2.Y + 1][pos2.X]) && isWall.Any(w => w == tileList[pos2.Y][pos2.X - 1]) && isWall.Any(w => w == tileList[pos2.Y][pos2.X + 1]),
                // Check if tile is at the top end.
            isWallEndTop = isWall.Any(w => w == tileList[pos2.Y + 1][pos2.X]) && tileList[pos2.Y - 1][pos2.X] == '0',
                // Check if tile is at the bottom end.
            isWallEndBottom = isWall.Any(w => w == tileList[pos2.Y - 1][pos2.X]) && tileList[pos2.Y + 1][pos2.X] == '0',
                // Check if tile is at the right end.
            isWallEndRight = isWall.Any(w => w == tileList[pos2.Y][pos2.X + 1]) && tileList[pos2.Y][pos2.X - 1] == '0',
                // Check if tile is at the left end.
            isWallEndLeft = isWall.Any(w => w == tileList[pos2.Y][pos2.X - 1]) && tileList[pos2.Y][pos2.X + 1] == '0';

            // Check variables and set animations accordingly.
            if (isHorizontal)
                setAnimation(tileName.ToString() + ".0.0");
            if (isVertical)
                setAnimation(tileName.ToString() + ".0.1");
            if (isWallEndTop)
                setAnimation(tileName.ToString() + ".1.2");
            if (isWallEndBottom)
                setAnimation(tileName.ToString() + ".1.3");
            if (isWallEndRight)
                setAnimation(tileName.ToString() + ".1.5");
            if (isWallEndLeft)
                setAnimation(tileName.ToString() + ".1.4");
            if (isTopCorner)
            {
                if (isLeft)
                    setAnimation(tileName.ToString() + ".0.2");
                else if (isRight)
                    setAnimation(tileName.ToString() + ".0.3");
            }
            else if (isBottomCorner)
            {
                if (isLeft)
                    setAnimation(tileName.ToString() + ".0.4");
                else if (isRight)
                    setAnimation(tileName.ToString() + ".0.5");
            }
            if (is3WayNorth)
                setAnimation(tileName.ToString() + ".0.6");
            if (is3WaySouth)
                setAnimation(tileName.ToString() + ".0.7");
            if (is3WayWest)
                setAnimation(tileName.ToString() + ".0.9");
            if (is3WayEast)
                setAnimation(tileName.ToString() + ".0.8");
            if (is4Way)
                setAnimation(tileName.ToString() + ".1.0");


        }

        #endregion

        #region Overrides(Animation)

        public override void addAnimations(Texture2D tex)
        {
            int jsave = 0;
            addAnimation("A", tex, new Point(30, 30), new Point(0, 0), new Point(2, 0), new Point(2, 0), 0, Color.Snow);  // Bronze use NavajoWhite
            addAnimation("B", tex, new Point(30, 30), new Point(0, 0), new Point(3, 1), new Point(3, 1), 0, Color.White);
            addAnimation("C", tex, new Point(30, 30), new Point(0, 0), new Point(3, 3), new Point(3, 3), 0, Color.White);
            addAnimation("D", tex, new Point(30, 30), new Point(0, 0), new Point(3, 1), new Point(3, 1), 0, Color.White);
            addAnimation("E", tex, new Point(30, 30), new Point(0, 0), new Point(1, 0), new Point(1, 0), 0, Color.White);
            addAnimation("F", tex, new Point(30, 30), new Point(0, 0), new Point(0, 0), new Point(0, 0), 0, Color.White);
            addAnimation("G", tex, new Point(30, 30), new Point(0, 0), new Point(0, 1), new Point(0, 1), 0, Color.White);
            addAnimation("H", tex, new Point(30, 30), new Point(0, 0), new Point(1, 2), new Point(1, 2), 0, Color.White);
            addAnimation("T", tex, new Point(30, 30), new Point(0, 0), new Point(0, 1), new Point(0, 1), 0, Color.Transparent);

            addAnimation("!", tex, new Point(30, 30), new Point(0, 0), new Point(0, 0), new Point(0, 0), 0, Color.Transparent);

            // Adds all 9 wall animations. (Note: Make sure sprite sheet is in order.)
            // Wall 1: Row0-1 Wall 2: Row2-3...etc
            for (int i = 1; i <= 9; i++)
            {
                for (int j = jsave; j < 9 * 2; j++)
                {
                    if (j % 2 == 0) // Even row
                    {
                        addAnimation((i).ToString() + ".0.0", tex, new Point(15, 15), new Point(0, 0), new Point(0, j), new Point(0, j), 0, Color.White);
                        addAnimation((i).ToString() + ".0.1", tex, new Point(15, 15), new Point(0, 0), new Point(1, j), new Point(1, j), 0, Color.White);
                        addAnimation((i).ToString() + ".0.2", tex, new Point(15, 15), new Point(0, 0), new Point(2, j), new Point(2, j), 0, Color.White);
                        addAnimation((i).ToString() + ".0.3", tex, new Point(15, 15), new Point(0, 0), new Point(3, j), new Point(3, j), 0, Color.White);
                        addAnimation((i).ToString() + ".0.4", tex, new Point(15, 15), new Point(0, 0), new Point(4, j), new Point(4, j), 0, Color.White);
                        addAnimation((i).ToString() + ".0.5", tex, new Point(15, 15), new Point(0, 0), new Point(5, j), new Point(5, j), 0, Color.White);
                        addAnimation((i).ToString() + ".0.6", tex, new Point(15, 15), new Point(0, 0), new Point(6, j), new Point(6, j), 0, Color.White);
                        addAnimation((i).ToString() + ".0.7", tex, new Point(15, 15), new Point(0, 0), new Point(7, j), new Point(7, j), 0, Color.White);
                    }
                    else if (j % 2 == 1) // Odd row
                    {
                        addAnimation((i).ToString() + ".0.8", tex, new Point(15, 15), new Point(0, 0), new Point(0, j), new Point(0, j), 0, Color.White);
                        addAnimation((i).ToString() + ".0.9", tex, new Point(15, 15), new Point(0, 0), new Point(1, j), new Point(1, j), 0, Color.White);
                        addAnimation((i).ToString() + ".1.0", tex, new Point(15, 15), new Point(0, 0), new Point(2, j), new Point(2, j), 0, Color.White);
                        addAnimation((i).ToString() + ".1.1", tex, new Point(15, 15), new Point(0, 0), new Point(3, j), new Point(3, j), 0, Color.White);
                        addAnimation((i).ToString() + ".1.2", tex, new Point(15, 15), new Point(0, 0), new Point(4, j), new Point(4, j), 0, Color.White);
                        addAnimation((i).ToString() + ".1.3", tex, new Point(15, 15), new Point(0, 0), new Point(5, j), new Point(5, j), 0, Color.White);
                        addAnimation((i).ToString() + ".1.4", tex, new Point(15, 15), new Point(0, 0), new Point(6, j), new Point(6, j), 0, Color.White);
                        addAnimation((i).ToString() + ".1.5", tex, new Point(15, 15), new Point(0, 0), new Point(7, j), new Point(7, j), 0, Color.White);
                        jsave = j + 1;
                        break;
                    }
                }
            }
        }

        #endregion

        #region Methods

        public Rectangle collisionRectangle()
        {
            return collisionRect();
        }

        #endregion

    }
}
