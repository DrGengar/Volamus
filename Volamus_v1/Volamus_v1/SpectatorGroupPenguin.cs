using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class SpectatorGroupPenguin : SpectatorGroup
    {
        List<SpectatorPenguin> GroupPenguin;


        public SpectatorGroupPenguin(Vector3 pos, int c, Random rnd) : base(pos, c, rnd)
        {
            Position = pos;
            Count = c;

            GroupPenguin = new List<SpectatorPenguin>();

            if (Position.X < 0)
            {
                rotation = 0;
            }
            else
            {
                rotation = 180;
            }

            for (int i = 0; i < Count; i++)
            {
                int xTemp = rnd.Next(-50, 50);
                int yTemp = 6;

                if (Position.Y < 0)
                {
                    yTemp *= -1;
                }

                Vector3 p = new Vector3(Position.X + (xTemp / 10), Position.Y - yTemp * i, pos.Z);
                Vector3 scale = new Vector3(3, 3, 3);
                SpectatorPenguin temp = new SpectatorPenguin(p, scale);
                GroupPenguin.Add(temp);
            }
        }

        public new void LoadContent()
        {
            for (int i = 0; i < Count; i++)
            {
                GroupPenguin[i].LoadContent();
            }
        }

        public new void UnloadContent()
        {
            for (int i = 0; i < Count; i++)
            {
                GroupPenguin[i].UnloadContent();
            }
        }

        public new void Update()
        {
            for (int i = 0; i < Count; i++)
            {
                GroupPenguin[i].Update();
            }
        }

        public new void Draw(Camera camera)
        {
            for (int i = 0; i < Count; i++)
            {
                GroupPenguin[i].Draw(camera, rotation);
            }
        }

        public new void SetCheering()
        {
            for (int i = 0; i < Count; i++)
            {
                GroupPenguin[i].Cheering = true;
            }
        }
    }
}
