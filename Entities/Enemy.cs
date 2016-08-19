using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarter4Project.Factories;
using Quarter4Project.Libraries;
using Quarter4Project.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Entities
{
    class Enemy : Animation
    {
        
        #region Fields

        GameManager game;

        WeaponFactory weapon;

        int hp,
            damage,
            bulletSpeed,
            accuracy,
            range,
            weaponID,
            currentAmmo,
            maxAmmo,
            spareAmmo,
            numberOfShots;

        float fireRateDelay,
            fireRateDelayCounter,
            burstFireDelay,
            maxBurstFireDelay,
            minBurstFireDelay,
            burstFireDelayCounter,
            burstCount,
            maxBurstCount,
            minBurstCount,
            burstCounter,
            reloadDelay,
            reloadDelayCounter,
            visionCone,
            visionDistance,
            playerDistance,
            combatDistance,
            angleToPlayer,
            rotationSpeed;

        bool shooting,
            addBullet,
            alive,
            playerDetected,
            playerVisible;

        Vector2 direction,
            directionVector,
            playerVector,
            offsetVector;

        Random r = new Random();

        #endregion

        #region Initilization

        public Enemy(Texture2D tex, Vector2 pos, GameManager g, int enemyID, int weaponID)
            : base(pos)
        {
            game = g;
            this.weaponID = weaponID;
            addAnimations(tex);

            fireRateDelayCounter = 0;
            burstFireDelayCounter = 0;
            burstCounter = 0;
            reloadDelayCounter = 0;

            shooting = false;
            addBullet = false;
            alive = true;

            directionVector = Collision.unitVector(new Vector2(100, -1));
            offsetVector = new Vector2(currentSprite.frameSize.X / 2, currentSprite.frameSize.Y / 2);
            rotationCenter = offsetVector;

            switch (enemyID)
            { 
                case 1000:

                    hp = 1000;//Game.RNG.Next(90, 110);
                    speed.X = (r.Next(30, 40) / 10);
                    speed.Y = (r.Next(30, 40) / 10);

                    visionCone = MathHelper.ToRadians(45);
                    visionDistance = 400;
                    rotationSpeed = MathHelper.ToRadians(0.65f);

                    combatDistance = 300;

                    break;
            }

            switch (weaponID)
            { 
                case 1000:

                    damage = 35;
                    accuracy = 4;
                    range = 1000;
                    maxAmmo = currentAmmo = 12;
                    spareAmmo = r.Next(2, 3);
                    fireRateDelay = 0.1f;
                    reloadDelay = 2.2f;
                    bulletSpeed = 15;
                    numberOfShots = 1;
                    burstFireDelay = 0.5f; //(Game.RNG.Next(5, 15) / 10) * 60;
                    burstCount = r.Next(2, 5);
                    setAnimation("1");
                    break;
            }
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            rotation = Collision.VectorToDegrees(directionVector);
            playerVector = new Vector2((game.getPlayer().getPos().X + game.getPlayer().getOffset().X)- (position.X + offsetVector.X), (game.getPlayer().getPos().Y + game.getPlayer().getOffset().Y) - (position.Y + offsetVector.Y));
            playerDistance = Collision.magnitude(playerVector);

            if (alive)
            {
                if (IsPlayerInCone())
                {
                    playerDetected = true;
                }

                if (playerDetected)
                {
                    if (playerDistance <= combatDistance)
                    {
                        if (Collision.GetAngle(directionVector, playerVector) < MathHelper.ToRadians(2))
                        {
                            Shoot();
                        }
                        else
                        {
                            FacePlayer();
                        }
                    }
                    else
                    {
                        //aStar follow
                    }
                }
                else
                {
                    //patroll
                }

                #region ShootingStuff
                if (shooting)
                {
                    fireRateDelayCounter++;
                    if (fireRateDelayCounter >= fireRateDelay * 60)
                    {
                        shooting = false;
                        fireRateDelayCounter = 0;
                    }
                }

                if (currentAmmo <= 0)
                {
                    reloadDelayCounter++;
                    if (reloadDelayCounter >= reloadDelay * 60 && spareAmmo >= 1)
                    {
                        currentAmmo = maxAmmo;
                        reloadDelayCounter = 0;
                    }
                }

                if (burstCounter >= burstCount)
                {
                    burstFireDelayCounter++;
                    if (burstFireDelayCounter >= burstFireDelay * 60)
                    {
                        burstCounter = 0;
                        burstFireDelayCounter = 0;
                    }
                }

                if (addBullet)
                {
                    BulletFactory.SpawnBullet(position + offsetVector, directionVector, game.bulletTex, weaponID, damage, bulletSpeed, range, accuracy, numberOfShots, game.bulletList, r, game);
                    addBullet = false;
                }
                #endregion

                if (hp <= 0)
                {
                    alive = false;
                }
            }

            base.Update(gameTime);
        }

        public override void addAnimations(Texture2D tex)
        {
            addAnimation("1", tex, new Point(45, 30), new Point(0, 0), new Point(0, 0), new Point(0, 0), 0, Color.White);
            addAnimation("3", tex, new Point(45, 30), new Point(0, 0), new Point(2, 0), new Point(2, 0), 0, Color.White);
            addAnimation("4", tex, new Point(45, 30), new Point(0, 0), new Point(3, 0), new Point(3, 0), 0, Color.White);
            addAnimation("5", tex, new Point(45, 30), new Point(0, 0), new Point(0, 1), new Point(0, 1), 0, Color.White);
            addAnimation("6", tex, new Point(45, 30), new Point(0, 0), new Point(1, 1), new Point(1, 1), 0, Color.White);
            setAnimation("1");
        }

        public bool IsPlayerInCone()
        {
            return (Collision.GetAngle(playerVector, directionVector) <= visionCone) && playerDistance <= visionDistance;
        }

        void FacePlayer()
        {
            float x = game.getPlayer().getPos().X - position.X;
            float y = game.getPlayer().getPos().Y - position.Y;

            float desiredAngle = Collision.VectorToDegrees(new Vector2(x, y));
            //float desiredAngle = (float)Math.Atan2(y, x);

            float difference = WrapAngle(desiredAngle - rotation);

            difference = MathHelper.Clamp(difference, -rotationSpeed, rotationSpeed);

            directionVector = Collision.DegreesToVector(Collision.VectorToDegrees(directionVector) + difference);
        }
        private static float WrapAngle(float radians)
        {
            while (radians < -MathHelper.Pi) { radians += MathHelper.TwoPi; }
            while (radians > MathHelper.Pi) { radians -= MathHelper.TwoPi; }
            return radians;
        }

        void Shoot()
        {
            if (!shooting && currentAmmo >= 1 && burstCounter < burstCount)
            {
                shooting = true;
                addBullet = true;
                burstCounter++;
                currentAmmo--;
            }
        }

        public Vector2 GetOffsetVector()
        {
            return offsetVector;
        }

        public void DamageEnemy(int damage)
        {
            hp -= damage;
        }

        public int GetHp()
        {
            return hp;
        }
    }
}
