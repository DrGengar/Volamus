using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Pick
    {
        List<Drop> drops;



        public Pick()
        {
            this.drops = new List<Drop>();
        }

        public void Update(Random rnd)
        {
            int total = rnd.Next(3);
            if (drops.Count < total)
            {
                drops.Add(Generate(rnd));
            }

            for (int Drop = 0; Drop < drops.Count; Drop++)
            {
                drops[Drop].Update();
                if (drops[Drop].ttl <= 0)
                {
                    drops.RemoveAt(Drop);
                    Drop--;
                }
            }
            if (Collision.Instance.matchIsFinish)
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

            Model dr = GameStateManager.Instance.Content.Load<Model>("Models/drop");
            return new Drop(new Vector3(x, y, 2), timeToLive, dr);
        }

        public void Draw(Camera camera, Effect effect)
        {

            for (int index = 0; index < drops.Count; index++)
            {

                drops[index].Draw(camera, effect);
            }

        }
    }
}
