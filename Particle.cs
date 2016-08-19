using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarter4Project.Libraries;

namespace Q4project
{
    class Particle : Animation
    {
        public Vector2 direction = Vector2.Zero;
        public bool deleteMe = false;
        Color washColor;
        int elapsedTime = 0;
        int lifeSpan;

        public Particle(Texture2D tex, Vector2 pos, int lifespan, int myspeed, Color washcolor, float angle)
            : base(pos)
        {
            addAnimations(tex);
            angle *= (float)(Math.PI / 180);
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            lifeSpan = lifespan;
            washColor = washcolor;
            speed.X = myspeed;
            speed.Y = myspeed;

        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime >= lifeSpan)
            {
                deleteMe = true;
            }
            position += direction * speed;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gametime, SpriteBatch spritebatch)
        {
            spritebatch.Draw(currentSprite.texture, new Vector2(position.X + rotationCenter.X, position.Y + rotationCenter.Y), new Rectangle((currentSprite.frameSize.X) + currentSprite.startPos.X, (currentSprite.frameSize.Y) + currentSprite.startPos.Y, currentSprite.frameSize.X, currentSprite.frameSize.Y), washColor, rotation, rotationCenter, 1, SpriteEffects.None, 0);

        }

        public override void addAnimations(Texture2D tex)
        {
            addAnimation("IDLE", tex, new Point(tex.Width, tex.Height), new Point(0, 0), Point.Zero, Point.Zero, 0, Color.White);
            setAnimation("IDLE");
        }

    }
}
