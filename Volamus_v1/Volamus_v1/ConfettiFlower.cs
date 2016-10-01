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
    class ConfettiFlower
    {
        Effect effect;
        List<Drop> confetti;
        Model dr;
        Texture2D texture;
        int direction;
        float rotate;

        //Konfetti an den Ecken des Gewinners

        public ConfettiFlower(int dir)
        {
            confetti = new List<Drop>();
            direction = dir;

            dr = GameStateManager.Instance.Content.Load<Model>("Models/flower");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shader");

            int temp = new Random().Next(1, 10);
            switch (temp)
            {
                case 1:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV1");
                    break;
                case 2:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV2");
                    break;
                case 3:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV3");
                    break;
                case 4:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV4");
                    break;
                case 5:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV5");
                    break;
                case 6:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV6");
                    break;
                case 7:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV7");
                    break;
                case 8:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV8");
                    break;
                default:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV9");
                    break;
            }
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

            rotate = rnd.Next(360); 

            int temp = new Random().Next(1, 11);
            switch (temp)
            {
                case 1:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV1");
                    break;
                case 2:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV2");
                    break;
                case 3:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV3");
                    break;
                case 4:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV4");
                    break;
                case 5:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV5");
                    break;
                case 6:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV6");
                    break;
                case 7:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV7");
                    break;
                case 8:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV8");
                    break;
                default:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Flowers/flowerUV9");
                    break;
            }

            return new Drop(loc, timeToLive, dr, velo, texture, rotate);
        }

        public void Draw(Camera camera)
        {

            for (int index = 0; index < confetti.Count; index++)
            {
                //hjfbhd
                confetti[index].DrawFlower(camera, effect, 90, dr);
                }
            }

        }

    }

