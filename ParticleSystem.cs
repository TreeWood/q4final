using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Q4project
{
    static class ParticleSystem
    {
        public static void CreateParticles(Vector2 pos, Texture2D tex, Random rng, List<Particle> particlelist, int redmin, int redmax, int greenmin, int greenmax, int bluemin, int bluemax, int minparticles, int maxparticles, int minangle, int maxangle, int minlifespan, int maxlifespan, int minspeed, int maxspeed)
        {
            Color washColor = new Color(rng.Next(redmin, redmax), rng.Next(greenmin, greenmax), rng.Next(bluemin, bluemax));
            for (int i = 0; i < rng.Next(minparticles, maxparticles); i++)
            {
                particlelist.Add(new Particle(tex, pos, rng.Next(minlifespan, maxlifespan), rng.Next(minspeed, maxspeed), washColor, (float)(rng.NextDouble() * (maxangle - minangle) + minangle)));
            }
        }

    }
}
