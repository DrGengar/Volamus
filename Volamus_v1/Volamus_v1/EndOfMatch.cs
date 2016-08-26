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
        List<Drop> confetti;

        //Jubel des Gewinners
        public void EndOfMatchWinner(Player winner)
        {
            //Flügel gehen nach oben
            winner.PositionLeftWing = new Vector3(winner.Position.X - 2, winner.Position.Y, winner.Position.Z - 1);
            winner.PositionRightWing = new Vector3(winner.Position.X + 2, winner.Position.Y, winner.Position.Z - 1);
            winner.HitAngleHigh = 20;

            //hüpft
            if (winner.Position.Z < 7 && !winner.IsFalling)
            {
                winner.PositionZ += winner.JumpVelocity;
            }
            else
            {
                if (winner.Position.Z > 0)
                {
                    winner.PositionZ -= winner.JumpVelocity;
                    winner.IsFalling = true;
                }
                else
                {
                    winner.IsFalling = false;
                }
            }
        }

        //Trauer des Verlieres
        public void EndOfMatchLoser(Player Loser)
        {
            Loser.PositionLeftWing = new Vector3(Loser.Position.X + 5, Loser.Position.Y, Loser.Position.Z + 2);
            Loser.PositionRightWing = new Vector3(Loser.Position.X - 5, Loser.Position.Y, Loser.Position.Z + 2);
            Loser.HitAngleHigh = -40;
        }


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

        public void Draw(Camera camera, Effect effect)
        {

            for (int index = 0; index < confetti.Count; index++)
            {
                confetti[index].Draw(camera, effect);
            }

        }

    }
}
