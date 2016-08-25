using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class Pick
    {
        List<Drop> drops;
        // Camera camera;


        public Pick()
        {
            this.drops = new List<Drop>();
        }

        //Liste wird mit Drops aufgefüllt, bzw Drops deren Zeit abgelaufen ist werden entfernt
        public void Update()
        {
            Random rand = new Random();
            int total = rand.Next(4);
            if (drops.Count < total)
            {
                drops.Add(Generate());
            }
            /*   for (int i =0; i <= total; i++)
               {
                   drops.Add(Generate());
               }*/

            for (int Drop = 0; Drop < drops.Count; Drop++)
            {
                drops[Drop].Update();
                if (drops[Drop].ttl <= 0)
                {
                    drops.RemoveAt(Drop);
                    Drop--;
                }
            }
        }

        //generiert neue Drops an einer zufälligen Position
        private Drop Generate()
        {
            Random rnd = new Random();
            float x = rnd.Next(-50, 50);
            float y = rnd.Next(-45, 46);
            Model dr = GameStateManager.Instance.Content.Load<Model>("drop");
            int timeToLive = 200 + rnd.Next(80);

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
