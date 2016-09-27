using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class PointLight
    {
        public Vector3 Position
        {
            get { return position; }
        }

        public Vector4 Ambient
        {
            get { return ambient; }
        }

        public Vector4 Diffuse
        {
            get { return diffuse; }
        }

        public Vector4 Specular
        {
            get { return specular; }
        }

        public float Radius
        {
            get { return radius; }
        }

        Vector3 position;
        Vector4 ambient;
        Vector4 diffuse;
        Vector4 specular;
        float radius;

        public PointLight(Vector3 pos, Vector4 amb, Vector4 dif, Vector4 spec, float r)
        {
            position = pos;
            ambient = amb;
            diffuse = dif;
            specular = spec;
            radius = r;
        }
    }
}
