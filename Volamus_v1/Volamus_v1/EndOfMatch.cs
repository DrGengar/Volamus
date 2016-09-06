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
    class EndOfMatch
    {
        Effect effect;
        List<Drop> confetti;

        //Konfetti an den Ecken des Gewinners

        public EndOfMatch()
        {
            this.confetti = new List<Drop>();
        }

        public void Update(Player winner)
        {
            Random rand = new Random();
            int total = rand.Next(100);
            if (confetti.Count < total)
            {
                if (winner.Direction == 1)
                {
                    Vector3 location = new Vector3(-50, -45, 0);
                    Vector3 location2 = new Vector3(+50, -45, 0);
                    confetti.Add(Generate(winner, location));
                    confetti.Add(Generate(winner, location2));
                }
                else
                {
                    Vector3 location = new Vector3(-50, +45, 0);
                    Vector3 location2 = new Vector3(+50, +45, 0);
                    confetti.Add(Generate(winner, location));
                    confetti.Add(Generate(winner, location2));
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

        private Drop Generate(Player winner, Vector3 loc)
        {
            Random rnd = new Random();

            Model dr = GameStateManager.Instance.Content.Load<Model>("confetti2");
            int timeToLive = 80 + rnd.Next(40);
            Vector3 velo = new Vector3(rnd.Next(-10, 10),   //zufällige Geschwindigkeit und Richtung der Konfetties
                rnd.Next(1, 10),
                 rnd.Next(1, 10));
            velo = velo / 100;

            return new Drop(loc, timeToLive, dr, velo);
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
