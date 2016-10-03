using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class SpectatorGroupBumbleBee : SpectatorGroup
    {
        List<SpectatorBumbleBee> GroupBumbleBee;


        public SpectatorGroupBumbleBee(Vector3 pos, int c, Random rnd) : base(pos, c, rnd)
        {
            Position = pos;
            Count = c;

            GroupBumbleBee = new List<SpectatorBumbleBee>();

            if (Position.X < 0)
            {
                rotation = -90;
            }
            else
            {
                rotation = 90;
            }

            for (int i = 0; i < Count; i++)
            {
                int xTemp = rnd.Next(-50, 50);
                int yTemp = rnd.Next(5, 10);

                if (Position.Y < 0)
                {
                    yTemp *= -1;
                }

                Vector3 p = new Vector3(Position.X + (xTemp / 10), Position.Y - yTemp * i, pos.Z);
                Vector3 scale = new Vector3(2, 2, 2);
                SpectatorBumbleBee temp = new SpectatorBumbleBee(p, scale);
                GroupBumbleBee.Add(temp);
            }
        }

        public new void LoadContent()
        {
            for (int i = 0; i < Count; i++)
            {
                GroupBumbleBee[i].LoadContent();
            }
        }

        public new void UnloadContent()
        {
            for (int i = 0; i < Count; i++)
            {
                GroupBumbleBee[i].UnloadContent();
            }
        }

        public new void Update()
        {
            for (int i = 0; i < Count; i++)
            {
                GroupBumbleBee[i].Update();
            }
        }

        public new void Draw(Camera camera)
        {
            for (int i = Count - 1; i >= 0; i--)
            {
                GroupBumbleBee[i].Draw(camera, rotation);
            }
        }

        public new void SetCheering()
        {
            cheering = true;
            for (int i = 0; i < Count; i++)
            {
                GroupBumbleBee[i].Cheering = true;
            }
        }
    }
}
