using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    //für Flugbahn
    //mehrere Flugbahnen?
    public class Parabel
    {
        private float t = 0;
        private float g = 9.81f;

        private float x;
        private float y;
        private float z;

        private float velocity;
        private int direction;
        private Vector3 position;
        float alpha; //Winkel in Winkelmaß -> Umrechnung in Radiant in Funktion
        float gamma; //Winkel in Winkelmaß -> Umrechnung in Radiant in Funktion
        float betta; //Winkel in Winkelmaß -> Umrechnung in Radiant in Funktion

        Vector3 lastPosition;
        Vector3 hitdirection;


        public Vector3 Hit_Direction
        {
            get { return hitdirection; }
        }

        public Vector3 Angles
        {
            get { return new Vector3(alpha, betta, gamma); }
        }

        public float Velocity
        {
            get { return velocity; }
        }

        public int Direction
        {
            get { return direction; }
        }

        public Parabel(Vector3 pos, float al, float ga, float be, float v, int dir)
        {
            position = pos;
            alpha = al;
            betta = be;
            gamma = ga;
            velocity = v;
            direction = dir;

            x = position.X;
            y = position.Y;
            z = position.Z;

            lastPosition = pos;
        }

        /*  x: rechts/links;  y: vorne/hinten,   z: oben/unten
         *  v0: Abwurfgeschwindigkeit
         *  alpha: wie flach/steil   betta: wie weit in y Richtung    gamma: wie weit in x Richtung
         *  t: Zeit, bzw Darstellungsgeschwindigkeit ("Ballgeschwindigkeit" aber ohne Flugbahn zu verändern)
         *  g: Erdanziehungskraft 
         * 
         * */
        public Vector3 Flug()
        {
            position.Z = z + velocity * (float)Math.Sin(MathHelper.ToRadians(alpha)) * t - (g / 2) * t * t;
            position.Y = y + (direction) * velocity * (float)Math.Cos(MathHelper.ToRadians(betta)) * t;
            position.X = x + velocity * (float)Math.Sin(MathHelper.ToRadians(gamma)) * t;
            t = t + 0.05f;

            hitdirection = position - lastPosition;
            
            lastPosition = position;

            return position;
        }

        public void Reset_t()
        {
            t = 0;
        }
    }
}
