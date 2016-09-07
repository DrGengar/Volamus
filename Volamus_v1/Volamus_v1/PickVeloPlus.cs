using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class PickVeloPlus
    {
        Effect effect;
        List<Drop> dropsVelo;
        Model dr;


        // Einsammeln verringert die Laufgeschwindigkeit des Gegeners, bis der Ball den Boden berührt
        public PickVeloPlus()
        {
            this.dropsVelo = new List<Drop>();
        }

        public void LoadContent()
        {
            dr = GameStateManager.Instance.Content.Load<Model>("Models/dropGeschwindigkeit");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTest");
        }

        public void Update(Random rnd)
        {   //hinzufügen neuer Drops

            int total = rnd.Next(3);

            while(dropsVelo.Count < total)
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

                    GameScreen.Instance.Match.PlayerOne.Movespeed += 0.1f;
                }

                if (Drop < dropsVelo.Count && Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerTwo, dropsVelo[Drop]))
                {
                    dropsVelo.RemoveAt(Drop);

                    GameScreen.Instance.Match.PlayerTwo.Movespeed += 0.1f;
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

            return new Drop(new Vector3(x, y, 2), timeToLive, dr);
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
