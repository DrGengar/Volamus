using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class PickSizeMinus
    {
        Effect effect;
        List<Drop> drops;
        Model dr;

        public PickSizeMinus()
        {
            this.drops = new List<Drop>();
        }

        public void LoadContent()
        {
            dr = GameStateManager.Instance.Content.Load<Model>("Models/drop");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTest");
        }

        public void Update(Random rnd)
        {
            int total = rnd.Next(3);

            while(drops.Count < total)
            {
                drops.Add(Generate(rnd));
            }

            for (int Drop = 0; Drop < drops.Count; Drop++)
            {
                drops[Drop].Update();

                if (drops[Drop].ttl <= 0)
                {
                    drops.RemoveAt(Drop);
                }

                if (Drop < drops.Count && (Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerOne, drops[Drop]) || Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerTwo, drops[Drop])))
                {
                    drops.RemoveAt(Drop);

                    if (Ball.Instance.BoundingSphereRadius > 1.5)
                    {
                        Ball.Instance.BoundingSphereRadius -= 0.1f;
                    }
                }
            }

            if (GameScreen.Instance.Match.IsFinished)
            {
                drops.RemoveAll(item => item.ttl != 0);  //alle Drops die aktuell noch leben werden entfernt
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

            for (int index = 0; index < drops.Count; index++)
            {
                drops[index].Draw(camera, effect);
            }

        }
    }
}
