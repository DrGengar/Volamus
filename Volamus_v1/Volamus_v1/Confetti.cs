using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class Confetti
    {
        Effect effect;
        List<Drop> confetti;
        Model dr;
        Texture2D texture;
        int direction;

        //Konfetti an den Ecken des Gewinners

        public Confetti(int dir)
        {
            confetti = new List<Drop>();
            direction = dir;

            dr = GameStateManager.Instance.Content.Load<Model>("Models/confetti2");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTest");
            texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/blau");
        }

        public void Update(Random rnd)
        {
            int total = rnd.Next(100);

            while (confetti.Count < total)
            {
                if (direction == 1)
                {
                    Vector3 location = new Vector3(-50, -45, 0);
                    Vector3 location2 = new Vector3(+50, -45, 0);
                    confetti.Add(Generate(location, rnd));
                    confetti.Add(Generate(location2, rnd));
                }
                else
                {
                    Vector3 location = new Vector3(-50, +45, 0);
                    Vector3 location2 = new Vector3(+50, +45, 0);
                    confetti.Add(Generate(location, rnd));
                    confetti.Add(Generate(location2, rnd));
                }
            }


            for (int Drop = 0; Drop < confetti.Count; Drop++)
            {
                confetti[Drop].UpdateEnd();

                if (confetti[Drop].ttl <= 0)
                {
                    confetti.RemoveAt(Drop);
                    Drop--;
                }
            }
        }

        private Drop Generate(Vector3 loc, Random rnd)
        {
            int timeToLive = 100 + rnd.Next(40);
            Vector3 velo = new Vector3(rnd.Next(-5, 5), rnd.Next(-5, 5), rnd.Next(5, 10));
            velo = velo / 100;

            return new Drop(loc, timeToLive, dr, velo, texture);
        }

        public void Draw(Camera camera)
        {

            for (int index = 0; index < confetti.Count; index++)
            {
                confetti[index].Draw(camera, effect);
            }

        }

    }
}
