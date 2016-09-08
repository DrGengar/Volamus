using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class PickVeloMinus
    {
        Effect effect;
        List<Drop> dropsVelo;
        Model dr;
        Texture2D texture;


        // Einsammeln verringert die Laufgeschwindigkeit des Gegeners, bis der Ball den Boden berührt
        public PickVeloMinus()
        {
            this.dropsVelo = new List<Drop>();
        }

        public void LoadContent()
        {
            dr = GameStateManager.Instance.Content.Load<Model>("Models/PUveloM");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTestWithTexture");
            texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green2");
        }

        public void Update(Random rnd)
        {   //hinzufügen neuer Drops

            int total = rnd.Next(3);

            while (dropsVelo.Count < total)
            {
                dropsVelo.Add(Generate(rnd));
            }

            //aktualisieren der Drops
            for (int Drop = 0; Drop < dropsVelo.Count; Drop++)
            {

                dropsVelo[Drop].UpdateVelo();

                if (dropsVelo[Drop].ttl <= 0)
                {
                    dropsVelo.RemoveAt(Drop);
                }

                if (Drop < dropsVelo.Count && Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerOne, dropsVelo[Drop]))
                {
                    dropsVelo.RemoveAt(Drop);
                    

                    GameScreen.Instance.Match.PlayerOne.Enemy.Movespeed -= 0.1f;
                }

                if (Drop < dropsVelo.Count && Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerTwo, dropsVelo[Drop]))
                {
                    dropsVelo.RemoveAt(Drop);

                    GameScreen.Instance.Match.PlayerTwo.Enemy.Movespeed -= 0.1f;
                }
            }

            if (GameScreen.Instance.Match.IsFinished)
            {
                dropsVelo.RemoveAll(item => item.ttl != 0);  //alle Drops die aktuell noch leben werden entfernt
            }
        }

        private Drop Generate(Random rnd)
        {
            float x = rnd.Next(-50, 50);
            float y = rnd.Next(-45, 46);
            int zufall = rnd.Next(1, 11);
            int timeToLive = 200 + rnd.Next(80);

            return new Drop(new Vector3(x, y, 0.5f), timeToLive, dr, texture);
        }

        public void Draw(Camera camera)
        {
            for (int index = 0; index < dropsVelo.Count; index++)
            {
                dropsVelo[index].Draw(camera, effect);
            }

        }
    }
}
