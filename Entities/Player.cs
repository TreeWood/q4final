using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Quarter4Project.Factories;
using Quarter4Project.Libraries;
using Quarter4Project.Managers;
using Quarter4Project.Sprite_Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Entities
{
    class Player : Animation
    {

        #region Fields

        GameManager game;

        private List<char> walls, objects;

        private WeaponFactory currentWeapon,
                              previousWeapon;

        private SoundEffect SMGFire,
                            ShotgunFire,
                            PistolFire,
                            ARFire,
                            RifleFire,
                            FlameFire;

        // Player vars
        private int hp;
        bool isAlive;

        // Weapon vars
        private int //bulletSpeed,
                    //damage,
                    //accuracy,
                    //range,
                    //weaponID,
                    //currentAmmo,
                    //maxAmmo,
                    //spareAmmo,
                    numOfShots;

        private float fireRateDelayCounter,
                      reloadDelayCounter;

        private bool isFiring,
                     addBullet,
                     hasAmmo,
                     pistolUnlocked,
                     smgUnlocked,
                     shotgunUnlocked,
                     arUnlocked,
                     rifleUnlocked;

        private Vector2 direction,
                        directionVector;

        private MouseState mouseState, prevMouseState;

        #endregion

        #region Initialization

        public Player(Texture2D tex, Vector2 pos, GameManager g, WeaponFactory weapon)
            : base(pos)
        {
            addAnimations(tex);
            game = g;
            position = pos;
            rotationCenter = new Vector2(30 / 2, 30 / 2);
            speed = new Vector2(3, 3);
            currentWeapon = weapon;
            //weaponID = currentWeapon.getID();
            //setWeapon(0);
            walls = new List<char>();
            objects = new List<char>();

            hp = 100;
            isAlive = true;

            isFiring = false;
            hasAmmo = true;
            //currentAmmo = maxAmmo;

            pistolUnlocked = true;
            smgUnlocked = shotgunUnlocked = arUnlocked = rifleUnlocked = false;

            for (int y = 0; y < game.getCurrentMap().wallTiles.Count; y++)
            {
                for (int x = 0; x < game.getCurrentMap().wallTiles[0].Length; x++)
                {
                    if (walls.Count(w => w == game.getCurrentMap().wallTiles[y][x]) <= 0 && game.getCurrentMap().wallTiles[y][x] != '0' && game.getCurrentMap().wallTiles[y][x] != '9')
                        walls.Add(game.getCurrentMap().wallTiles[y][x]);
                }
            }

            for (int y = 0; y < game.getCurrentMap().objectTiles.Count; y++)
            {
                for (int x = 0; x < game.getCurrentMap().objectTiles[0].Length; x++)
                {
                    if(objects.Count(o => o == game.getCurrentMap().objectTiles[y][x]) <= 0 && game.getCurrentMap().objectTiles[y][x] != '0' && game.getCurrentMap().objectTiles[y][x] != 'B')
                        objects.Add(game.getCurrentMap().objectTiles[y][x]);
                }
            }

                SoundEffect.MasterVolume = .1f;

            ShotgunFire = game.Game.Content.Load<SoundEffect>(@"SoundEffects/shot gun");
            PistolFire = game.Game.Content.Load<SoundEffect>(@"SoundEffects/9mm");
            RifleFire = game.Game.Content.Load<SoundEffect>(@"SoundEffects/Rifle");
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if (game.Game.IsActive)
            {
                if (isAlive)
                {

                    // Update rotation
                    directionVector = Collision.unitVector(new Vector2(game.cursor.getPos().X - (position.X + rotationCenter.X), game.cursor.getPos().Y - (position.Y + rotationCenter.Y)));
                    rotation = Collision.VectorToDegrees(directionVector);
                    
                    // Update firing.
                    if (mouseState.LeftButton == ButtonState.Pressed && !isFiring && currentWeapon.getCurrentAmmo() >= 1)
                    {
                        addBullet = true;
                        isFiring = true;
                        currentWeapon.setCurrentAmmo(currentWeapon.getCurrentAmmo() - 1);

                        switch (currentWeapon.getID())
                        {
                            case 1000:
                                PistolFire.Play();
                                break;
                            case 3000:
                                ShotgunFire.Play();
                                break;
                            case 5000:
                                RifleFire.Play();
                                break;

                        }
                    }

                    if (isFiring)
                    {
                        fireRateDelayCounter++;
                        if (fireRateDelayCounter >= currentWeapon.getFireRateDelay() * 60)
                        {
                            if ((currentWeapon.getID() >= 1000 && currentWeapon.getID() < 2000) || (currentWeapon.getID() >= 3000 && currentWeapon.getID() < 4000) || (currentWeapon.getID() >= 5000 && currentWeapon.getID() < 6000))
                            {
                                if (mouseState.LeftButton == ButtonState.Released)
                                {
                                    isFiring = false;
                                    fireRateDelayCounter = 0;
                                }
                            }
                            else
                            {
                                isFiring = false;
                                fireRateDelayCounter = 0;
                            }
                        }
                    }

                    // Update ammo
                    if (currentWeapon.getCurrentAmmo() <= 0 && currentWeapon.getSpareAmmo() >= 1)
                    {
                        reloadDelayCounter++;
                        if (reloadDelayCounter >= currentWeapon.getReloadDelay() * 60)
                        {
                            currentWeapon.setCurrentAmmo(currentWeapon.getMaxAmmo());
                            reloadDelayCounter = 0;
                            currentWeapon.setSpareAmmo(currentWeapon.getSpareAmmo() - 1);
                        }
                    }

                    if (addBullet)
                    {
                        BulletFactory.SpawnBullet(position + getOffset(), directionVector, game.bulletTex, currentWeapon.getID(), currentWeapon.getDamage(), currentWeapon.getBulletSpeed(), currentWeapon.getRange(), currentWeapon.getAccuracy(), currentWeapon.getNumOfShots(), game.bulletList, game.RNG, game);
                        addBullet = false;
                    }
                    
                    /*
                    if (keyboardState.IsKeyDown(Keys.R) && currentWeapon.getCurrentAmmo() < currentWeapon.getMaxAmmo())
                        currentWeapon.setCurrentAmmo(0);
                    */

                    switch (currentWeapon.getID())
                    {
                        case 1000:
                            setAnimation("1");
                            break;
                        case 2000:
                            break;
                        case 3000:
                            setAnimation("3");
                            break;
                        case 4000:
                            setAnimation("4");
                            break;
                        case 5000:
                            setAnimation("5");
                            break;
                        case 6000:
                            setAnimation("6");
                            break;
                    }

                    updateMovement();
                }
            }

            prevKeyboardState = keyboardState;
            prevMouseState = mouseState;
            base.Update(gameTime);
        }

        private void updateMovement()
        {
            bool checkLeft = !walls.Any(w => (w != game.getCurrentMap().wallTiles[((int)position.Y / 15) + 1][((int)position.X / 15) - 1])),
                 checkRight = !walls.Any(w => (w != game.getCurrentMap().wallTiles[((int)position.Y / 15) + 1][((int)position.X / 15) + 2])),
                 checkTop = !walls.Any(w => (w != game.getCurrentMap().wallTiles[((int)position.Y / 15) - 1][((int)position.X / 15) + 1])),
                 checkBottom = !walls.Any(w => (w != game.getCurrentMap().wallTiles[((int)position.Y / 15) + 2][((int)position.X / 15) + 1])),
                 checkWallBottom = game.getCurrentMap().wallTiles[((int)position.Y / 15) + 2][((int)position.X / 15) + 1] == '0' && !walls.Any(w => (w != game.getCurrentMap().wallTiles[((int)position.Y / 15) + 2][((int)position.X / 15)])),
                 checkWallTop = game.getCurrentMap().wallTiles[((int)position.Y / 15) - 2][((int)position.X / 15) + 1] == '0' && !walls.Any(w => (w != game.getCurrentMap().wallTiles[((int)position.Y / 15)][((int)position.X / 15)])),
                 checkBGLeft = objects.Any(o => o == game.getCurrentMap().objectTiles[((int)position.Y / 30)][(((int)position.X - 15) / 30)]),
                 checkBGRight = objects.Any(o => o == game.getCurrentMap().objectTiles[((int)position.Y / 30)][(((int)position.X) / 30) + 1]),
                 checkBGTop = objects.Any(o => o == game.getCurrentMap().objectTiles[(((int)position.Y - 15) / 30)][((int)position.X / 30)]),
                 checkBGBottom = objects.Any(o => o == game.getCurrentMap().objectTiles[((int)position.Y / 30) + 1][((int)position.X / 30)]);
            

            if (direction.Y == 1)
            {
                if (position.X % 15 == 0 && position.Y % 15 == 0)
                    direction.Y = 0;

                if ((int)((position.Y - speed.Y) / 15) < (int)(position.Y / 15))
                    position.Y -= position.Y % 15;
            }

            if (keyboardState.IsKeyDown(Keys.S))
            {
                if (position.X % 15 == 0 && position.Y % 15 == 0 && !checkBottom && !checkWallBottom && !checkBGBottom)
                    direction.Y = 1;

            }
             
            if (direction.Y == -1)
            {
                if (position.X % 15 == 0 && position.Y % 15 == 0)
                    direction.Y = 0;

                if ((int)((position.Y - speed.Y) / 15) < (int)(position.Y / 15))
                    position.Y -= position.Y % 15;
            }

            if (keyboardState.IsKeyDown(Keys.W))
            {
                if (position.X % 15 == 0 && position.Y % 15 == 0 && !checkTop && !checkWallTop && !checkBGTop)
                    direction.Y = -1;
            }

            if (direction.X == 1)
            {
                if (position.X % 15 == 0 && position.Y % 15 == 0)
                    direction.X = 0;

                if ((int)((position.X - speed.X) / 15) < (int)(position.X / 15))
                    position.X -= position.X % 15;
            }

            if (keyboardState.IsKeyDown(Keys.D))
            {
                if (position.X % 15 == 0 && position.Y % 15 == 0 && !checkRight && !checkBGRight)
                    direction.X = 1;
            }

            if (direction.X == -1)
            {
                if (position.X % 15 == 0 && position.Y % 15 == 0)
                    direction.X = 0;

                if ((int)((position.X - speed.X) / 15) < (int)(position.X / 15) && direction.X != 0)
                    position.X -= position.X % 15;
            }

            if (keyboardState.IsKeyDown(Keys.A))
            {
                if (position.X % 15 == 0 && position.Y % 15 == 0 && !checkLeft && !checkBGLeft)
                    direction.X = -1;
            }
              
            /*
            speed = new Vector2(0, 0);

            if (keyboardState.IsKeyDown(Keys.W) && !checkTop && directionVector.Y < 0)
                speed.Y = 3;
            else if (keyboardState.IsKeyDown(Keys.S) && !checkBottom && directionVector.Y > 0)
                speed.Y = 3;
            else if (keyboardState.IsKeyDown(Keys.W) && !checkTop && directionVector.Y > 0)
                speed.Y = -1;
            else if (keyboardState.IsKeyDown(Keys.S) && !checkBottom && directionVector.Y < 0)
                speed.Y = -1;
            else
                speed.Y = 0;

            if (keyboardState.IsKeyDown(Keys.A) && !checkLeft && directionVector.X < 0)
                speed.X = 3;
            else if (keyboardState.IsKeyDown(Keys.D) && !checkRight && directionVector.X > 0)
                speed.X = 3;
            else if (keyboardState.IsKeyDown(Keys.A) && !checkLeft && directionVector.X > 0)
                speed.X = -1;
            else if (keyboardState.IsKeyDown(Keys.D) && !checkRight && directionVector.X < 0)
                speed.X = -1;
            else
                speed.X = 0;

            position += directionVector * speed;
             */
            position += direction * speed;
        }

        #endregion

        #region Overrides(Animation)

        public override void addAnimations(Texture2D tex)
        {
            addAnimation("1", tex, new Point(45, 30), new Point(0, 0), new Point(0, 0), new Point(0, 0), 0, Color.White);
            addAnimation("3", tex, new Point(45, 30), new Point(0, 0), new Point(2, 0), new Point(2, 0), 0, Color.White);
            addAnimation("4", tex, new Point(45, 30), new Point(0, 0), new Point(3, 0), new Point(3, 0), 0, Color.White);
            addAnimation("5", tex, new Point(45, 30), new Point(0, 0), new Point(0, 1), new Point(0, 1), 0, Color.White);
            addAnimation("6", tex, new Point(45, 30), new Point(0, 0), new Point(1, 1), new Point(1, 1), 0, Color.White);
            setAnimation("1");
        }

        #endregion

        #region Methods

        public void setWeapon(int ID)
        {
            previousWeapon = currentWeapon;

            foreach (WeaponFactory w in game.weaponList)
            {
                if (w.getUID() == ID)
                    currentWeapon = w;
            }
        }

        public void damagePlayer(int damage)
        {
            hp -= damage;
        }

        public void healPlayer(int heal)
        {
            hp += heal;
        }

        public int getHP()
        {
            return hp;
        }

        public Vector2 getOffset()
        {
            return rotationCenter;
        }

        public Vector2 getDirection()
        {
            return directionVector;
        }

        public WeaponFactory getCurrentWeapon()
        {
            return currentWeapon;
        }      

        #endregion

    }
}
