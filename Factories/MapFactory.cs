// MapFactory.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Quarter4Project.Entities;
using Quarter4Project.Libraries;
using Quarter4Project.Managers;
using Quarter4Project.Sprite_Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Quarter4Project.Factories
{
    static class MapFactory
    {

        public static List<Map> maps { get; private set; }

        /// <summary>
        /// Add a new map.
        /// </summary>
        /// <param name="id">Map id.</param>
        /// <param name="floor">Map floor.</param>
        public static void addMap(int id, int floor)
        {
            if (maps == null)
                maps = new List<Map>();

            maps.Add(new Map(id, floor));
        }

        public class Map
        {

            public List<string> backgroundTiles { get; private set; }
            public List<string> objectTiles { get; private set; }
            public List<string> wallTiles { get; private set; }
            public List<string> portalTiles { get; private set; }
            public List<string> lightTiles { get; private set; }
            public List<string> enemyTiles { get; private set; }
            public List<Tiles> mapTiles { get; private set; }
            public List<Enemy> enemyList { get; private set; }
            public int id { get; private set; }
            public int floor { get; private set; }

            /// <summary>
            /// Constructs mapTiles and loads tiles from external files.
            /// </summary>
            /// <param name="id"></param>
            /// <param name="floor"></param>
            public Map(int id, int floor)
            {
                mapTiles = new List<Tiles>();
                loadMap(id, floor);
            }

            /// <summary>
            /// Loads tiles from .txt files.
            /// </summary>
            /// <param name="id">Map name for organization.</param>
            /// <param name="floor">Floor number of a map id.</param>
            private void loadMap(int id, int floor)
            {
                backgroundTiles = new List<string>();
                objectTiles = new List<string>();
                wallTiles = new List<string>();
                portalTiles = new List<string>();
                lightTiles = new List<string>();
                enemyTiles = new List<string>();
                enemyList = new List<Enemy>();
                this.id = id;
                this.floor = floor;

                using (StreamReader sr = new StreamReader(@"Content/Maps/" + id + "/Background/" + floor + ".txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        backgroundTiles.Add(line);
                }

                using (StreamReader sr = new StreamReader(@"Content/Maps/" + id + "/Objects/" + floor + ".txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        objectTiles.Add(line);
                }

                using (StreamReader sr = new StreamReader(@"Content/Maps/" + id + "/Walls/" + floor + ".txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        wallTiles.Add(line);
                }

                using (StreamReader sr = new StreamReader(@"Content/Maps/" + id + "/Portals/" + floor + ".txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        portalTiles.Add(line);
                }

                using (StreamReader sr = new StreamReader(@"Content/Maps/" + id + "/Lights/" + floor + ".txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        lightTiles.Add(line);
                }

                using (StreamReader sr = new StreamReader(@"Content/Maps/" + id + "/Enemy/" + floor + ".txt"))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        enemyTiles.Add(line);
                }

            }

        }

        public static class buildMap
        {
            /// <summary>
            /// Adds tiles to the mapTiles in the map.
            /// </summary>
            /// <param name="map">Map that has stored map data.</param>
            /// <param name="game">GameManager</param>
            /// <param name="backgroundTextures">Tile textures for background tiles.</param>
            /// <param name="objectTextures">Tile textures for object tiles.</param>
            /// <param name="wallTextures">Tile textures for wall tiles.</param>
            public static void addTiles(Map map, GameManager game, Texture2D backgroundTextures, Texture2D objectTextures, Texture2D wallTextures, Texture2D playerTextures, Texture2D enemyTextures)
            {
                map.mapTiles.Clear();

                for (int y = 0; y < map.backgroundTiles.Count; y++)
                {
                    for (int x = 0; x < map.backgroundTiles[0].Length; x++)
                    {
                        if (map.backgroundTiles[y][x] != '0')
                            map.mapTiles.Add(new Tiles(backgroundTextures, new Vector2(x * 30, y * 30), game, map.backgroundTiles[y][x].ToString(), map.backgroundTiles));
                    }
                }

                for (int y = 0; y < map.objectTiles.Count; y++)
                {
                    for (int x = 0; x < map.objectTiles[0].Length; x++)
                    {
                        if (map.objectTiles[y][x] != '0')
                            map.mapTiles.Add(new Tiles(objectTextures, new Vector2(x * 30, y * 30), game, map.objectTiles[y][x].ToString(), map.objectTiles));
                    }
                }

                for (int y = 0; y < map.wallTiles.Count; y++)
                {
                    for (int x = 0; x < map.wallTiles[0].Length; x++)
                    {
                        if (map.wallTiles[y][x] != '0')
                            map.mapTiles.Add(new Tiles(wallTextures, new Vector2(x * 15, y * 15), game, map.wallTiles[y][x].ToString(), map.wallTiles));
                    }
                }

                List<Point> spawnPoints = new List<Point>();

                for (int y = 0; y < map.portalTiles.Count; y++)
                {
                    for (int x = 0; x < map.portalTiles[0].Length; x++)
                    {
                        if (map.portalTiles[y][x] == '!')
                            spawnPoints.Add(new Point(x, y));

                        if (map.portalTiles[y][x] == 'T')
                            map.mapTiles.Add(new Tiles(wallTextures, new Vector2(x * 30, y * 30), game, map.portalTiles[y][x].ToString(), map.portalTiles));
                    }
                }

                // Select random spawn point and spawn player.
                Random r = new Random();
                int spawn = r.Next(spawnPoints.Count);
                game.setPlayer(new Player(playerTextures, new Vector2(spawnPoints[spawn].X * 30, spawnPoints[spawn].Y * 30), game, game.currentWeapon));

                for (int y = 0; y < map.enemyTiles.Count; y++)
                {
                    for (int x = 0; x < map.enemyTiles[0].Length; x++)
                    {
                        if (map.enemyTiles[y][x] == 'M')
                        {
                            map.enemyList.Add(new Enemy(enemyTextures, new Vector2(x * 30, y * 30), game, 1000, 1000));
                        }
                        else if (map.enemyTiles[y][x] == 'N')
                        {
                            map.enemyList.Add(new Enemy(enemyTextures, new Vector2(x * 30, y * 30), game, 1000, 2000));
                        }
                    }
                }

            }

            public static List<Tiles> getWalls(Map map)
            {
                List<Tiles> walls = new List<Tiles>();
                string[] wall = { "1", "2", "3", "4", "5", "6", "7", "8", "9" };

                foreach (Tiles t in map.mapTiles)
                {
                    if (wall.Any(w => w == t.getAnimName()))
                        walls.Add(t);
                }
                return walls;
            }

            /// <summary>
            /// Draws tiles from the map provided.
            /// </summary>
            /// <param name="gameTime">Provides a snapshot of timing values.</param>
            /// <param name="spriteBatch">Used to draw the tiles.</param>
            /// <param name="map">Map that has our stored map data.</param>
            public static void drawTiles(GameTime gameTime, SpriteBatch spriteBatch, Map map)
            {
                foreach (Tiles t in map.mapTiles)
                    t.Draw(gameTime, spriteBatch);
            }

            /// <summary>
            /// Updates tiles.
            /// </summary>
            /// <param name="gameTime"></param>
            public static void updateTiles(GameTime gameTime)
            {

            }

        }

    }
}