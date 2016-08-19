// Bullet.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarter4Project.Factories;
using Quarter4Project.Libraries;
using Quarter4Project.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Sprite_Handlers
{
    class Bullet : Animation
    {

        #region Fields

        GameManager game;

        public int damage,
                   ricochetMax,
                   ricochetCount;

        private Vector2 direction,
                        offset,
                        spawnOffset,
                        range,
                        distance;

        public bool deleteMe;

        #endregion

        #region Initialization

        public Bullet(Vector2 pos, Vector2 dir, Texture2D tex, int bulletDamage, float bulletSpeed, int bulletRange, GameManager g)
            : base(pos)
        {
            addAnimations(tex);
            direction = Collision.unitVector(dir);

            game = g;

            position = pos;
            direction = dir;
            damage = bulletDamage;
            range = new Vector2(bulletRange, bulletRange);
            speed = new Vector2(bulletSpeed, bulletSpeed);

            deleteMe = false;
            
            offset = new Vector2(currentSprite.frameSize.X / 2, currentSprite.frameSize.Y / 2);
            spawnOffset = new Vector2(currentSprite.frameSize.X / 2, currentSprite.frameSize.Y / 2);

            position -= spawnOffset;

            correctRotation();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            position += direction * speed;
            distance += speed;

            if (distance.X >= range.X && distance.Y >= range.Y)
                deleteMe = true;

            /*
            string[] walls = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            
            if (game.getCurrentMap().mapTiles.Any(m => walls.Any(w => m.getAnimName().Contains(w.ToString()) && m.collisionRect().Intersects(collisionRect()))))
                deleteMe = true;
            */
            base.Update(gameTime);
        }

        #endregion

        #region Overrides

        public override void addAnimations(Texture2D tex)
        {
            addAnimation("1", tex, new Point(10, 10), new Point(1, 1), new Point(0, 0), new Point(0, 0), 1600, Color.White);
            setAnimation("1");
        }

        #endregion

        #region Methods

        private void correctRotation()
        {
            rotation = (float)Math.Atan2(direction.Y, direction.X);
        }

        public Vector2 getOffsetVector()
        {
            return offset;
        }

        #endregion

    }
}
