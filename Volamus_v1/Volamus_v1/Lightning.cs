using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class Lightning
    {
        Vector3 diffuseColor; //Light
        Vector3 direction; //Direction from where it comes
        Vector3 specularColor; //highlight colour

        public Vector3 DiffuseColor
        {
            get { return diffuseColor; }
        }

        public Vector3 Direction
        {
            get { return direction; }
        }

        public Vector3 SpecularColor
        {
            get { return specularColor; }
        }

        public Lightning()
        {
            diffuseColor = new Vector3(0.0f, 0.0f, 0.0f);
            direction = new Vector3(0.0f, 0.0f, -1.0f);
            specularColor = new Vector3(0.0f, 0.0f, 0.0f);
        }

        public Lightning(Vector3 dif, Vector3 dir, Vector3 spe)
        {
            diffuseColor = dif;
            direction = dir;
            specularColor = spe;
        }
    }
}
