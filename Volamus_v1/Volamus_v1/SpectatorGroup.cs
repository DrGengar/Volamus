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
        public Vector3 Position;
        public int Count;
        public int rotation;
        public bool cheering;


        public SpectatorGroup(Vector3 pos, int c, Random rnd)
        {
            Position = pos;
            Count = c;

            if (Position.X < 0)
            {
                rotation = 0;
            }
            else
            {
                rotation = 180;
            }
        }

        public void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }

        public void Update()
        {

        }

        public void Draw(Camera camera)
        {

        }

        public void SetCheering()
        {

        }
    }
}
