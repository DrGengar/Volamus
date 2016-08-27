using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace Volamus_v1
{
    public class Wind
    {
        float strength;

        Vector2 standard_direction;

        float angle; //Radians oder Degree?

        public Wind(int index) //index is {0,1,2}
        {
            //Stärke des Winds: 0 kein Wind, 1 leichter Wind, 2 starker Wind
            strength = ((float)index)/2; //scale: 10

            standard_direction = new Vector2(0, 1);

            Random rnd = new Random();
            angle = (float)rnd.NextDouble() * MathHelper.TwoPi; // erstellt angle mit Werten zwischen 0 und 2*PI (0 Grad and 360 Grad)
        }

        public void Update()
        {
            //Winkel random ändern
            Random rnd = new Random();
            int decision = rnd.Next(2);

            if (decision == 0)
            {
                if (angle != 0.0f)
                {
                    angle -= (MathHelper.Pi / 90);
                }
            }
            else
            {
                if (angle != MathHelper.TwoPi)
                {
                    angle += (MathHelper.Pi / 90);
                }
            }
        }

        public float Direction()
        {
            //Winkel Updaten
            //Update();

            //Winkel und standard_direction -> wirkliche direction
            float x = standard_direction.X;
            float y = standard_direction.Y;

            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);

            float x2 = x * cos - y * sin;
            float y2 = x * sin + y * cos;

            //return new Vector2(strength * x2, strength * y2);

            float result = sin * strength;

            return sin * strength; //strength?
        }
    }
}

