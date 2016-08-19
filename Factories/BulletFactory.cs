using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarter4Project.Sprite_Handlers;
using Quarter4Project.Libraries;
using Quarter4Project.Managers;

namespace Quarter4Project.Factories
{
    static class BulletFactory
    {
        public static void SpawnBullet(Vector2 pos, Vector2 dir, Texture2D tex, int WeaponID, int bulletDamage, int bulletSpeed, int bulletRange, int accuracy, int numberOfShots, List<Bullet> bulletList, Random RNG, GameManager g)
        {
            for (int i = 0; i < numberOfShots; i++)
            {
                if (WeaponID >= 6000)
                    bulletSpeed = RNG.Next(7, 10);

                float accuracyMod = RNG.Next((accuracy * -1), accuracy);
                float newDirectionAngle = (float)Math.Atan2(dir.Y, dir.X) + MathHelper.ToRadians(accuracyMod);
                Vector2 newDirectionVector = new Vector2((float)Math.Cos(newDirectionAngle), (float)Math.Sin(newDirectionAngle));
                Vector2 newPostion = pos + ((Collision.unitVector(newDirectionVector) * (g.getPlayer().getOffset().X + tex.Width)));
                
                bulletList.Add(new Bullet(newPostion, newDirectionVector, tex, bulletDamage, bulletSpeed, bulletRange, g));
            }
        }
    }
}
