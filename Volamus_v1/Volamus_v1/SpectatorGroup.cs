using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class SpectatorGroup
    {
        List<Spectator> Group;
        Vector3 Position;
        int Count;
        int rotation;
        bool cheering;


        public SpectatorGroup(Vector3 pos, int c, Random rnd)
        {
            Position = pos;
            Count = c;

            Group = new List<Spectator>();

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
                int yTemp = rnd.Next(5, 10);

                if (Position.Y < 0)
                {
                    yTemp *= -1;
                }

                Vector3 p = new Vector3(Position.X + (xTemp / 10), Position.Y - yTemp * i, 0);
                Vector3 scale = new Vector3(3, 3, 3);
                Spectator temp = new Spectator(p, scale);
                Group.Add(temp);
            }
        }

        public void LoadContent()
        {
            for (int i = 0; i < Count; i++)
            {
                Group[i].LoadContent();
            }
        }

        public void UnloadContent()
        {
            for (int i = 0; i < Count; i++)
            {
                Group[i].UnloadContent();
            }
        }

        public void Update()
        {
            for (int i = 0; i < Count; i++)
            {
                Group[i].Update();
            }
        }

        public void Draw(Camera camera)
        {
            for (int i = 0; i < Count; i++)
            {
                Group[i].Draw(camera, rotation);
            }
        }

        public void SetCheering()
        {
            if (cheering)
            {
                cheering = false;
                for (int i = 0; i < Count; i++)
                {
                    Group[i].Cheering = false;
                }
            }
            else
            {
                cheering = true;
                for (int i = 0; i < Count; i++)
                {
                    Group[i].Cheering = true;
                }
            }
        }
    }
}
