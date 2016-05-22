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
        VertexPositionTexture[] netVertices = new VertexPositionTexture[6];


        int width;
        int length;
        int net_height;

        //Netz
        Model net;

        //Texturen
        BasicEffect effect;

        public Spielfeld(int w, int l, int n_h) : base()
        {
            width = w;
            length = l;
            net_height = n_h;
        }

        public void Initialize(GraphicsDeviceManager graphics, ContentManager content)
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

            netVertices[0].Position = fieldVertices[4].Position;
            netVertices[1].Position = fieldVertices[4].Position + new Vector3(0, 0, 10);
            netVertices[2].Position = fieldVertices[6].Position;
            netVertices[3].Position = netVertices[1].Position;
            netVertices[4].Position = fieldVertices[6].Position + new Vector3(0, 0, 10);
            netVertices[5].Position = netVertices[2].Position;

            effect = new BasicEffect(graphics.GraphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            net = content.Load<Model>("Netzv2");
        }

        public BasicEffect get_effect()
        {
            return effect;
        }

        public VertexPositionTexture get_vertex(int index)
        {
            if (index > 6 || index < 0)
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

        public int get_width()
        {
            return width;
        }

        public int get_length()
        {
            return length;
        }

        public void Draw(Kamera camera, GraphicsDeviceManager graphics)
        {
            effect.View = camera.get_View();
            effect.Projection = camera.get_Projection();

            //effect.TextureEnabled = true;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                effect.View = camera.get_View();
                effect.Projection = camera.get_Projection();

                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, fieldVertices, 0, 4);

                //graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, netVertices, 0, 2);
            }

            Matrix[] transforms = new Matrix[net.Bones.Count];
            net.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in net.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.06f, 0.05f, 0.05f)
                        * Matrix.CreateTranslation(new Vector3(0, 0, net_height));
                    effect.View = camera.get_View();
                    effect.Projection = camera.get_Projection();
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }


    }
}
