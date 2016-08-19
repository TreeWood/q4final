using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Q4project;
using Quarter4Project.Entities;
using Quarter4Project.Event_Managers;
using Quarter4Project.Factories;
using Quarter4Project.Libraries;
using Quarter4Project.Sprite_Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quarter4Project.Managers
{
    /// <summary>
    /// Handles gameplay.
    /// </summary>
    class GameManager : DrawableGameComponent
    {

        #region Fields

        private Game1 game;
        private SpriteBatch spriteBatch;

        private SpriteFont Consolas,
                           Consolas12,
                           fixedSys;

        private KeyboardState keyboardState,
                              prevKeyboardState;

        private MapFactory.Map currentMap;
        private int maps;
        private Texture2D wallSheet,
                          tileSheet;

        private List<ButtonFactory.Default> btnList;

        private Camera cam;
        public Cursor cursor;
        private Texture2D cursorTex;
        public MouseState mouseState, prevMouseState;

        private Player player;
        private Texture2D playerSheet;

        public List<WeaponFactory> weaponList,
                                   constWeaponList;
        public WeaponFactory currentWeapon;
        public List<DropFactory> dropList;
        private float pickUpTimer = 1.0f;
        private Texture2D weaponSheet;
        public Texture2D bulletTex;
        public List<Bullet> bulletList;
        Color colorFlow, redFlash;
        float p, f, v;

        public Random RNG = new Random();

        private Texture2D flameThrower,
                          pistol,
                          assaultRifle,
                          rifle,
                          SMG,
                          shotgun;

        private Texture2D light, enemySheet;
        private float timer, ftimer;

        Random r = new Random();

        private RenderTarget2D nonlights;
        private RenderTarget2D lights;
        private Effect lighting;
        private int a, b, c, d, e;
        List<Blood> bloodList;
        Texture2D bloodTex;
        Texture2D particleTex;
        List<Particle> particleList;

        #endregion

        #region Initializiation

        public GameManager(Game1 g)
            : base(g)
        {
            game = g;
        }

        public override void Initialize()
        {
            // Initialize button list.
            btnList = new List<ButtonFactory.Default>();
            weaponList = new List<WeaponFactory>();
            constWeaponList = new List<WeaponFactory>();
            bulletList = new List<Bullet>();
            dropList = new List<DropFactory>();
            particleList = new List<Particle>();

            bloodList = new List<Blood>();
            // Get number of maps.
            maps = Directory.GetDirectories(@"Content/Maps/").Length;

            // Loop through number of maps, find number of floors, add maps/floors.
            for (int i = 1; i <= maps; i++)
            {
                int floorcount = Directory.GetFiles(@"Content/Maps/" + i + "/Background/").Length;
                for (int j = 1; j <= floorcount; j++)
                    MapFactory.addMap(i, j);
            }
            
            a = ((int)r.Next(2, 6) + 1);
            c = ((int)r.Next(0, 5) + 1);
            b = ((int)r.Next(0, 5) + 1);
            d = ((int)r.Next(0, 5) + 1);
            e = ((int)r.Next(0, 5) + 1);

            base.Initialize();

            // Add buttons.
            btnList.Add(new ButtonFactory.Default(new Vector2(10, 400), new Point(75, 55), new Color(255, 110, 110), GraphicsDevice, Consolas, "Death", new Color(255, 255, 255), new Vector2(2, 2), new Color(0, 0, 0), new Vector2(4, 4), new Color(0, 0, 0), 0, 2, 0));

            // Set map to first map.
            foreach (MapFactory.Map m in MapFactory.maps)
                if (m.id == 1 && m.floor == 1) currentMap = m;

            // Set camera
            cam = new Camera()
            {
                Limits = new Rectangle(0, 0, 0, 0)
            };

            cursor = new Cursor(cursorTex, new Vector2(mouseState.X, mouseState.Y), cam);
            
            // Add weapons
            weaponList.Add(new WeaponFactory(1000, weaponList.Count)); // Pistol
            weaponList.Add(new WeaponFactory(2000, weaponList.Count)); // SMG
            weaponList.Add(new WeaponFactory(3000, weaponList.Count)); // Shotgun
            weaponList.Add(new WeaponFactory(4000, weaponList.Count)); // Assault Rifle
            weaponList.Add(new WeaponFactory(5000, weaponList.Count)); // Rifle
            weaponList.Add(new WeaponFactory(6000, weaponList.Count)); // Flamethrower

            // Add tiles to map.
            MapFactory.buildMap.addTiles(currentMap, this, tileSheet, tileSheet, wallSheet, playerSheet, enemySheet);
            GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;

            player.setWeapon(2);

            dropList.Add(new DropFactory(weaponSheet, new Vector2(13 * 30, 60), this, weaponList[0]));
            dropList.Add(new DropFactory(weaponSheet, new Vector2(19 * 30, 60), this, weaponList[3]));
            dropList.Add(new DropFactory(weaponSheet, new Vector2(16 * 30, 60), this, weaponList[4]));
            dropList.Add(new DropFactory(weaponSheet, new Vector2(22 * 30, 60), this, weaponList[5]));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            // Sprite Sheets
            tileSheet = Game.Content.Load<Texture2D>(@"Sprites/tileSheet");
            wallSheet = Game.Content.Load<Texture2D>(@"Sprites/wallSheet");
            playerSheet = Game.Content.Load<Texture2D>(@"Sprites/enemySheet");
            enemySheet = Game.Content.Load<Texture2D>(@"Sprites/playerSheet");
            particleTex = Game.Content.Load<Texture2D>(@"Images/Particle");

            // Fonts
            Consolas = Game.Content.Load<SpriteFont>(@"Fonts/Consolas");
            Consolas12 = Game.Content.Load<SpriteFont>(@"Fonts/Consolas12");
            fixedSys = Game.Content.Load<SpriteFont>(@"Fonts/FixedSys");

            // Single Images
            bulletTex = Game.Content.Load<Texture2D>(@"Images/BulletPlaceHolder");
            cursorTex = Game.Content.Load<Texture2D>(@"Images/cursor");

            // Weapon UI
            flameThrower = Game.Content.Load<Texture2D>(@"Images/flameThrower");
            pistol = Game.Content.Load<Texture2D>(@"Images/pistol");
            assaultRifle = Game.Content.Load<Texture2D>(@"Images/assaultRifle");
            rifle = Game.Content.Load<Texture2D>(@"Images/rifle");
            shotgun = Game.Content.Load<Texture2D>(@"Images/shotgun");
            SMG = Game.Content.Load<Texture2D>(@"Images/smg");
            weaponSheet = Game.Content.Load<Texture2D>(@"Sprites/weaponSheet");
            bloodTex = Game.Content.Load<Texture2D>(@"Images\BloodTex");

            // Lighting
            light = Game.Content.Load<Texture2D>(@"Images/light");
            lighting = Game.Content.Load<Effect>(@"Effects/lighting");
            nonlights = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);
            lights = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            base.UnloadContent();
        }

        #endregion

        #region Update and Draw

        public override void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            
            // Flip through maps (Using Left Ctrl).
            if (keyboardState.IsKeyDown(Keys.LeftControl) && prevKeyboardState.IsKeyUp(Keys.LeftControl))
            {
                if (MapFactory.maps.Any(m => m.id == currentMap.id && m.floor == currentMap.floor + 1)) // Goes to next floor on current map unless max floor.
                {
                    foreach (MapFactory.Map m in MapFactory.maps)
                    {
                        if (m.id == currentMap.id && m.floor == (currentMap.floor + 1))
                        {
                            currentMap = m;
                            MapFactory.buildMap.addTiles(currentMap, this, tileSheet, tileSheet, wallSheet, playerSheet, enemySheet);
                            bulletList.Clear();
                        }
                    }
                }
                else if (currentMap.id == maps) // Reset last map/floor to first map.
                {
                    foreach (MapFactory.Map m in MapFactory.maps)
                    {
                        if (m.id == 1 && m.floor == 1)
                        {
                            currentMap = m;
                            MapFactory.buildMap.addTiles(currentMap, this, tileSheet, tileSheet, wallSheet, playerSheet, enemySheet);
                            bulletList.Clear();
                        }
                    }
                }
                else // Goes to next map when at last floor.
                {
                    foreach (MapFactory.Map m in MapFactory.maps)
                    {
                        if (m.id == (currentMap.id + 1) && m.floor == 1)
                        {
                            currentMap = m;
                            MapFactory.buildMap.addTiles(currentMap, this, tileSheet, tileSheet, wallSheet, playerSheet, enemySheet);
                            bulletList.Clear();
                            break;
                        }
                    }
                }
            }

            foreach (Tiles t in currentMap.mapTiles)
            {
                if (t.getAnimName() == "T")
                {
                    if (t.getPos().X == player.getPos().X && t.getPos().Y == player.getPos().Y)
                    {
                        foreach (MapFactory.Map m in MapFactory.maps)
                        {
                            if (m.id == 1 && m.floor == 2)
                            {

                                currentMap = m;
                                MapFactory.buildMap.addTiles(currentMap, this, tileSheet, tileSheet, wallSheet, playerSheet, enemySheet);
                                bulletList.Clear();
                            }
                        }
                    }
                }
            }
            

            updateBullets(gameTime);
            ButtonEvents.Update(gameTime, btnList, game);
            MapFactory.buildMap.updateTiles(gameTime);
            player.Update(gameTime);
            currentWeapon = player.getCurrentWeapon();
            foreach (Enemy e in currentMap.enemyList)
                e.Update(gameTime);
            // Camera movement must go after player update
            cam.Position = new Vector2(player.getPos().X - (GraphicsDevice.Viewport.Width / 2), player.getPos().Y - (GraphicsDevice.Viewport.Height / 2));
            cursor.Update(gameTime);

            pickUpTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            ftimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (timer >= 15.0f)
            {
                a = r.Next(0, 15) + 1;
                b = r.Next(0, 15) + 1;
                c = ((int)r.Next(0, 255) + 1);
                d = ((int)r.Next(0, 255) + 1);
                e = ((int)r.Next(0, 255) + 1);
                timer = 0.0f;
            }

            f += .001f;
            if (f < .1f)
                v = MathHelper.Lerp(1, 2, f);
            else if (f < .2f)
                v = MathHelper.Lerp(2, 3, f);
            else
                v = 0.0f;


            if (currentMap.id == 1 && currentMap.floor == 1)
            {
                // Pick up weapons
                foreach (DropFactory d in dropList)
                {
                    if ((player.getPos().X / 30) == (d.getPos().X / 30) && (player.getPos().Y / 30) == (d.getPos().Y / 30))
                    {
                        if (keyboardState.IsKeyDown(Keys.E) && prevKeyboardState.IsKeyDown(Keys.E) && pickUpTimer <= 0.0f)
                        {
                            dropList.Add(new DropFactory(weaponSheet, new Vector2(player.getPos().X, player.getPos().Y), this, player.getCurrentWeapon()));
                            player.setWeapon(d.getWep().getUID());
                            pickUpTimer = 1.0f;
                            for (int i = 0; i < dropList.Count; i++)
                            {
                                if (dropList[i].getWep().getUID() == player.getCurrentWeapon().getUID())
                                {
                                    dropList.RemoveAt(i);
                                    i--;
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }

            p += .05f;
            if (p < 1)
            {
                colorFlow = LerpColor(Color.Red, Color.Magenta, p);
                redFlash = LerpColor(new Color(255, 0, 0), new Color(0, 0, 0), p);
            }
            else if (p < 2)
            {
                colorFlow = LerpColor(Color.Magenta, Color.Blue, p - 1.0f);
                redFlash = LerpColor(new Color(0, 0, 0), new Color(255, 0, 0), p - 1.0f);
            }
            else if (p < 3)
            {
                colorFlow = LerpColor(Color.Blue, Color.Cyan, p - 2.0f);
                redFlash = new Color(255, 0, 0);
            }
            else if (p < 4)
            {
                colorFlow = LerpColor(Color.Cyan, Color.Green, p - 3.0f);
                redFlash = LerpColor(new Color(255, 0, 0), new Color(0, 0, 0), p - 3.0f);
            }
            else if (p < 5)
            {
                colorFlow = LerpColor(Color.Green, Color.Yellow, p - 4.0f);
                redFlash = new Color(0, 0, 0);
            }
            else if (p < 6)
            {
                colorFlow = LerpColor(Color.Yellow, Color.Red, p - 5.0f);
                redFlash = LerpColor(new Color(0, 0, 0), new Color(255, 0, 0), p - 5.0f);
            }
            else if (p > 6)
            {
                p = 0;
            }
            /*
            if (keyboardState.IsKeyDown(Keys.D1) && prevKeyboardState.IsKeyDown(Keys.D1))
                player.setWeapon(6);
            else if (keyboardState.IsKeyDown(Keys.D2) && prevKeyboardState.IsKeyDown(Keys.D2))
                player.setWeapon(2);
             */

            for (int i = 0; i < particleList.Count; i++)
            {
                if (particleList[i].deleteMe == true)
                {
                    particleList.RemoveAt(i);
                    i--;
                }
                else
                {
                    particleList[i].Update(gameTime);
                }
            }

            foreach (Blood b in bloodList)
                b.Update(gameTime);

            prevKeyboardState = keyboardState;
            prevMouseState = mouseState;
            base.Update(gameTime);
        }

        public void updateBullets(GameTime gameTime)
        {
            for (int i = 0; i < 1; i++)
            {
                string[] walls = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

                int pineapples = 0;

                for (int e = 0; e < currentMap.backgroundTiles.Count; e++)
                {
                    pineapples += e;
                }

                foreach (Bullet b in bulletList)
                {
                    if (((int)b.getPos().X / 15) <= 0 || ((int)b.getPos().Y / 15) <= 0 || ((int)b.getPos().Y >= (pineapples * 850) + 30) || ((int)b.getPos().X >= (currentMap.wallTiles.Count * 150)) || walls.Any(t => t == currentMap.wallTiles[(int)b.getPos().Y / 15][(int)b.getPos().X / 15].ToString()))
                        b.deleteMe = true;
                }
            }

                for (int j = 0; j < bulletList.Count; j++)
                {
                    if (bulletList[j].deleteMe)
                    {
                        bulletList.RemoveAt(j);
                        j--;
                    }
                    else
                    {
                        bulletList[j].Update(gameTime);
                    }
                }
            

            foreach (Bullet b in bulletList)
            {
                if (Collision.CheckCircleCircleCollision(new Collision.Circle(player.getPos() + player.getOffset(), player.getOffset().X), new Collision.Circle(b.getPos() + b.getOffsetVector(), b.currentSprite.frameSize.Y / 2)))
                {
                    player.damagePlayer(b.damage);
                    b.deleteMe = true;
                    ParticleSystem.CreateParticles(b.getPos(), particleTex, RNG, particleList, 200, 255, 0, 0, 0, 0, 5, 10, 0, 360, 200, 300, 1, 4);
                    bloodList.Add(new Blood(bloodTex, player.getPos(), this, RNG));
                }

                foreach (Enemy e in currentMap.enemyList)
                {
                    if (Collision.CheckCircleCircleCollision(new Collision.Circle(e.getPos() + e.GetOffsetVector(), e.GetOffsetVector().X), new Collision.Circle(b.getPos() + b.getOffsetVector(), b.getOffsetVector().X)))
                    {
                        e.DamageEnemy(b.damage);
                        b.deleteMe = true;
                        ParticleSystem.CreateParticles(b.getPos(), particleTex, RNG, particleList, 200, 255, 0, 0, 0, 0, 5, 10, 0, 360, 200, 300, 1, 4);
                        bloodList.Add(new Blood(bloodTex, e.getPos(), this, RNG));
                    }
                }
            }
        }

        /// <summary>
        /// Draw everything in the game.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of all timing values.</param>
        public override void Draw(GameTime gameTime)
        {

            GraphicsDevice.SetRenderTarget(nonlights);
            GraphicsDevice.Clear(new Color(0, 0, 0));
            // Moves with camera
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, cam.getMatrix(Vector2.One));

            // Draw map.
            MapFactory.buildMap.drawTiles(gameTime, spriteBatch, currentMap);

            foreach (Blood b in bloodList)
                b.Draw(gameTime, spriteBatch);

            foreach (Particle p in particleList)
                p.Draw(gameTime, spriteBatch);
            // Draw player.
            player.Draw(gameTime, spriteBatch);

            foreach (Enemy e in currentMap.enemyList)
                e.Draw(gameTime, spriteBatch);

            

            if (currentMap.id == 1 && currentMap.floor == 1)
            {
                foreach (DropFactory d in dropList)
                    d.Draw(gameTime, spriteBatch);
            }

            foreach (Bullet b in bulletList)
                b.Draw(gameTime, spriteBatch);


            if (currentMap.id == 1 && currentMap.floor == 1)
            {
                spriteBatch.DrawString(fixedSys, "Move around using WASD", new Vector2(-250, 0), Color.Cyan);
                spriteBatch.DrawString(fixedSys, "Stand over weapons and press 'E' to pick them up.", new Vector2(150, -50), Color.White);
            }
                        
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.SetRenderTarget(lights);
            GraphicsDevice.Clear(new Color(45, 45, 45));

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, null, null, null, cam.getMatrix(Vector2.One));
            
            /*
            // Random colorful lights
            for (int i = 0; i < currentMap.backgroundTiles.Count; i++)
            {
                for (int j = 0; j < currentMap.backgroundTiles[0].Length; j++)
                {
                    for (int g = 0; g < 1; g++)
                    {
                        if (i % (a) == 0 && j % (b) == 0 && currentMap.backgroundTiles[i][j] != '0' && currentMap.id == 1 && currentMap.floor == 1)
                        {
                            spriteBatch.Draw(light, new Rectangle((j * 30) - 105, (i * 30) - 105, 250, 250), colorFlow);
                            spriteBatch.Draw(light, new Rectangle((j * 30) - 105, (i * 30) - 105, 250, 250), colorFlow);
                        }
                        else if (i % a == 0 && j % b == 0 && currentMap.floor == 2)
                        {
                            spriteBatch.Draw(light, new Rectangle((j * 30) - 60, (i * 30) - 60, 150, 150), colorFlow);
                        }
                    }
                }
            }
             
            for (int i = 0; i < currentMap.backgroundTiles.Count; i++)
            {
                for (int j = 0; j < currentMap.backgroundTiles[0].Length; j++)
                {
                    if (i % 1 == 0 && j % 1 == 0 && currentMap.backgroundTiles[i][j] != '0')
                    {
                        spriteBatch.Draw(light, new Rectangle((j * 30), (i * 30), 45, 1), Color.White);
                        spriteBatch.Draw(light, new Rectangle((j * 30), (i * 30), 1, 45), Color.White);
                        spriteBatch.Draw(light, new Rectangle((j * 30) - 3, (i * 30) - 2, 45, 5), Color.White);
                        spriteBatch.Draw(light, new Rectangle((j * 30) - 3, (i * 30) - 2, 5, 45), Color.White);
                    }
                    
                    if (i % 1 == 0 && j % 1 == 0)
                    {
                        spriteBatch.Draw(light, new Rectangle((j * 30) - 20, (i * 30) - 20, 100, 100), new Color(255, 255, 255)); // 25, 0, 0
                    }
                }
            }
            */

            spriteBatch.Draw(light, new Rectangle(-200, -100, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(-300, -100, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(-400, -100, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(-500, -100, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(100, -150, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(50, -150, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(200, -150, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(300, -150, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(400, -150, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(500, -150, 300, 300), Color.White);
            spriteBatch.Draw(light, new Rectangle(600, -150, 300, 300), Color.White);

            for (int y = 0; y < currentMap.lightTiles.Count; y++)
            {
                for (int x = 0; x < currentMap.lightTiles[0].Length; x++)
                {
                    if (currentMap.lightTiles[y][x] == 'l')
                    {
                        spriteBatch.Draw(light, new Rectangle((x * 30) - 110, (y * 30) - 110, 250, 250), Color.White);
                    }
                }
            }

            foreach (Tiles t in currentMap.mapTiles)
            {
                if (t.getAnimName() == "E")
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X - 35, (int)t.getPos().Y - 35, 100, 100), new Color(135, 235, 255));
                else if (t.getAnimName() == "G")
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X - 10, (int)t.getPos().Y - 10, 50, 50), new Color(35, 155, 35));
                else if (t.getAnimName() == "F")
                {
                    //spriteBatch.Draw(light, new Rectangle((int)t.getPos().X - 10, (int)t.getPos().Y - 10, 50, 50), new Color(135, 235, 255));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X - 5, (int)t.getPos().Y + 19, 10, 10), new Color(135, 235, 255));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X, (int)t.getPos().Y + 19, 10, 10), new Color(135, 235, 255));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 5, (int)t.getPos().Y + 19, 10, 10), new Color(135, 235, 255));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 10, (int)t.getPos().Y + 19, 10, 10), new Color(135, 235, 255));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 15, (int)t.getPos().Y + 19, 10, 10), new Color(135, 235, 255));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 20, (int)t.getPos().Y + 19, 10, 10), new Color(135, 235, 255));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 25, (int)t.getPos().Y + 19, 10, 10), new Color(135, 235, 255));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X - 5, (int)t.getPos().Y + 19, 10, 10), new Color(135, 235, 255));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X, (int)t.getPos().Y, 15, 15), new Color(166, 139, 10));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 5, (int)t.getPos().Y, 15, 15), new Color(166, 139, 10));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 10, (int)t.getPos().Y, 15, 15), new Color(166, 139, 10));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 15, (int)t.getPos().Y, 15, 15), new Color(166, 139, 10));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 20, (int)t.getPos().Y, 15, 15), new Color(166, 139, 10));
                    spriteBatch.Draw(light, new Rectangle((int)t.getPos().X + 25, (int)t.getPos().Y, 15, 15), new Color(166, 139, 10));
                }
            }



                //spriteBatch.Draw(light, new Rectangle((int)player.getPos().X - 85, (int)player.getPos().Y - 85, 200, 200), Color.White);

                for (int i = 0; i < bulletList.Count; i++)
                {
                    if (i % 2 == 0)
                        spriteBatch.Draw(light, new Rectangle((int)bulletList[i].getPos().X - 42, (int)bulletList[i].getPos().Y - 42, 100, 100), new Color(c, d, e));
                    else
                        spriteBatch.Draw(light, new Rectangle((int)bulletList[i].getPos().X - 42, (int)bulletList[i].getPos().Y - 42, 100, 100), new Color(c, d, e));
                }

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(new Color(0, 0, 0));

            // Stays still 
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            lighting.Parameters["lights"].SetValue(lights);
            lighting.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(nonlights, new Vector2(0, 0), new Color(0, 0, 0));
            spriteBatch.End();

            spriteBatch.Begin();

            // Draw buttons.
            ButtonEvents.Draw(gameTime, spriteBatch, btnList);

            switch (player.getCurrentWeapon().getID())
            {
                case 1000: // Pistol
                    spriteBatch.Draw(pistol, new Rectangle(0, GraphicsDevice.Viewport.Height - 85, 280, 80), colorFlow);
                    break;
                case 2000: // SMG
                    spriteBatch.Draw(SMG, new Rectangle(0, GraphicsDevice.Viewport.Height - 85, 280, 80), colorFlow);
                    break;
                case 3000: // Shotgun
                    spriteBatch.Draw(shotgun, new Rectangle(0, GraphicsDevice.Viewport.Height - 85, 280, 80), colorFlow);
                    break;
                case 4000: // AR
                    spriteBatch.Draw(assaultRifle, new Rectangle(0, GraphicsDevice.Viewport.Height - 85, 280, 80), colorFlow);
                    break;
                case 5000: // Reg. Rifle
                    spriteBatch.Draw(rifle, new Rectangle(0, GraphicsDevice.Viewport.Height - 85, 280, 80), colorFlow);
                    break;
                case 6000: // Flame Thrower
                    spriteBatch.Draw(flameThrower, new Rectangle(0, GraphicsDevice.Viewport.Height - 85, 280, 80), colorFlow);
                    break;
            }

            spriteBatch.DrawString(fixedSys, "! Ammo: " + player.getCurrentWeapon().getCurrentAmmo().ToString() + "/" + player.getCurrentWeapon().getMaxAmmo().ToString(), new Vector2(280, GraphicsDevice.Viewport.Height - 100 + Consolas.MeasureString("1").Y), colorFlow);
            spriteBatch.DrawString(fixedSys, "! Clips: " + player.getCurrentWeapon().getSpareAmmo().ToString(), new Vector2(280, GraphicsDevice.Viewport.Height - 100 + (Consolas.MeasureString("1").Y * 2)), colorFlow);
            spriteBatch.DrawString(fixedSys, "F: " + f, new Vector2(0, 0), Color.White);
            
            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, cam.getMatrix(Vector2.One));
            
            cursor.Draw(gameTime, spriteBatch);

            spriteBatch.End();
            

            base.Draw(gameTime);
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Gets all maps.
        /// </summary>
        /// <returns>All maps in the game.</returns>
        public List<MapFactory.Map> getMaps()
        {
            return MapFactory.maps;
        }

        /// <summary>
        /// Gets current map.
        /// </summary>
        /// <returns>Current map.</returns>
        public MapFactory.Map getCurrentMap()
        {
            return currentMap;
        }

        /// <summary>
        /// Gets the player.
        /// </summary>
        /// <returns>Player</returns>
        public Player getPlayer()
        {
            return player;
        }

        /// <summary>
        /// Sets player.
        /// </summary>
        /// <param name="player">Player</param>
        public void setPlayer(Player player)
        {
            this.player = player;
        }

        private Color LerpColor(Color a, Color b, float percentage)
        {
            return new Color(
                (byte)MathHelper.Lerp(a.R, b.R, percentage),
                (byte)MathHelper.Lerp(a.G, b.G, percentage),
                (byte)MathHelper.Lerp(a.B, b.B, percentage),
                (byte)MathHelper.Lerp(a.A, b.A, percentage));
        }
        
        #endregion

    }
}