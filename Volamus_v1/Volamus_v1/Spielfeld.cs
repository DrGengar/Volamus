using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Volamus_v1
{
    class Spielfeld
    {
        VertexPositionTexture[] fieldVertices = new VertexPositionTexture[12];
        

        int width;
        int length;
        int net_height;

        //Netz
        Model net;

        //Texturen
        BasicEffect effect;

        public Spielfeld(int w,int l, int n_h) : base()
        {
            width = w;
            length = l;
            net_height = n_h;
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            fieldVertices[0].Position = new Vector3(width / 2, -length / 2, 0);
            fieldVertices[1].Position = new Vector3(-width / 2, -length / 2, 0);
            fieldVertices[2].Position = new Vector3(-width / 2, 0, 0);
            fieldVertices[3].Position = fieldVertices[0].Position;
            fieldVertices[4].Position = fieldVertices[2].Position;
            fieldVertices[5].Position = new Vector3(width / 2, 0, 0);
            fieldVertices[6].Position = fieldVertices[5].Position;
            fieldVertices[7].Position = fieldVertices[2].Position;
            fieldVertices[8].Position = new Vector3(-width / 2, length / 2, 0);
            fieldVertices[9].Position = fieldVertices[5].Position;
            fieldVertices[10].Position = fieldVertices[8].Position;
            fieldVertices[11].Position = new Vector3(width / 2, length / 2, 0);


            effect = new BasicEffect(graphics.GraphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            net = content.Load<Model>("Netzv1");
        }

        public BasicEffect get_effect()
        {
            return effect;
        }

        public VertexPositionTexture get_vertex(int index)
        {
            if(index > 12 || index < 0)
            {
                VertexPositionTexture zero = new VertexPositionTexture();
                zero.Position = new Vector3(0, 0, 0);

                return zero;
            }

            return fieldVertices[index];
        }

        public VertexPositionTexture[] get_field()
        {
            return fieldVertices;
        }


    }
}
