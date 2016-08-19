using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Quarter4Project.Libraries;
using Quarter4Project.Managers;

namespace Q4project
{
    class Blood : Animation
    {
        Vector2 offset;
        Vector2 offset2;
        int distance;
        int angle;

        public Blood(Texture2D tex, Vector2 pos, GameManager play, Random rng)
            : base(pos)
        {
            addAnimations(tex);
            offset = new Vector2(22, 15);
            distance = rng.Next(1, 30);
            angle = rng.Next(1, 360);
            offset2 = new Vector2((float)(distance * Math.Cos(angle)), (int)(distance * Math.Sin(angle)));
            position += offset2;
        }

        public override void addAnimations(Texture2D tex)
        {
            addAnimation("IDLE", tex, new Point(30, 30), new Point(0, 0), new Point(0, 0), new Point(0, 0), 0, Color.White);
            setAnimation("IDLE");
        }
    }
}
