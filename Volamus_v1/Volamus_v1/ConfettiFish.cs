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
    public class ConfettiFish
    {
        Effect effect;
        List<Drop> confetti;
        Model dr;
        Texture2D texture;
        Vector3 positionOne, positionTwo;
        float rotate;

        //Konfetti an den Ecken des Gewinners

        public ConfettiFish(Vector3 one, Vector3 two)
        {
            confetti = new List<Drop>();
            positionOne = one;
            positionTwo = two;

            dr = GameStateManager.Instance.Content.Load<Model>("Models/fisch");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shader");
        }

        public void Update(Random rnd)
        {
            int total = rnd.Next(100);

            while (confetti.Count < total)
            {

                confetti.Add(Generate(positionOne, rnd));
                confetti.Add(Generate(positionTwo, rnd));
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

            rotate = rnd.Next(360);

            int temp = new Random().Next(1, 10);
            switch (temp)
            {
                case 1:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/gelb");
                    break;
                case 2:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green");
                    break;
                case 3:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green3");
                    break;
                case 4:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/lila");
                    break;
                case 5:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/red");
                    break;
                case 6:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/pink");
                    break;
                case 7:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/blau");
                    break;
                case 8:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/blau2");
                    break;
                default:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green2");
                    break;
            }

            return new Drop(loc, timeToLive, dr, velo, texture, rotate);
        }

        public void Draw(Camera camera)
        {

            for (int index = 0; index < confetti.Count; index++)
            {
                confetti[index].DrawFish(camera, effect, 90, dr);
            }
        }
    }
}

