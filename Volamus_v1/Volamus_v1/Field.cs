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
    class Field
    {
        VertexPositionTexture[] fieldVertices = new VertexPositionTexture[12];

        int width;
        int length;
        int net_height;

        //Netz
        Model net;

        //Texturen
        BasicEffect effect;
        Texture2D texture;

        public VertexPositionTexture[] FieldVertices
        {
            get { return fieldVertices; }
        }

        public int Width
        {
            get { return width; }
        }

        public int Length
        {
            get { return length; }
        }

        public Field(int w, int l, int n_h) : base()
        {
            width = w;
            length = l;
            net_height = n_h;
        }

        public void Initialize()
        {
            fieldVertices[0].Position = new Vector3(width / 2, -length / 2, 0);
            fieldVertices[1].Position = new Vector3(-width / 2, -length / 2, 0);
            fieldVertices[2].Position = new Vector3(-width / 2, 0, 0);
            fieldVertices[3].Position = fieldVertices[0].Position;
            fieldVertices[4].Position = fieldVertices[2].Position;
            fieldVertices[5].Position = new Vector3(width / 2, 0, 0);

            fieldVertices[0].TextureCoordinate = new Vector2(0, 0);
            fieldVertices[1].TextureCoordinate = new Vector2(0, 1);
            fieldVertices[2].TextureCoordinate = new Vector2(1, 0);

            fieldVertices[3].TextureCoordinate = fieldVertices[1].TextureCoordinate;
            fieldVertices[4].TextureCoordinate = new Vector2(1, 1);
            fieldVertices[5].TextureCoordinate = fieldVertices[2].TextureCoordinate;

            fieldVertices[6].Position = fieldVertices[5].Position;
            fieldVertices[7].Position = fieldVertices[2].Position;
            fieldVertices[8].Position = new Vector3(-width / 2, length / 2, 0);
            fieldVertices[9].Position = fieldVertices[5].Position;
            fieldVertices[10].Position = fieldVertices[8].Position;
            fieldVertices[11].Position = new Vector3(width / 2, length / 2, 0);

            fieldVertices[6].TextureCoordinate = new Vector2(0, 0);
            fieldVertices[7].TextureCoordinate = new Vector2(0, 1);
            fieldVertices[8].TextureCoordinate = new Vector2(1, 0);

            fieldVertices[9].TextureCoordinate = fieldVertices[1].TextureCoordinate;
            fieldVertices[10].TextureCoordinate = new Vector2(1, 1);
            fieldVertices[11].TextureCoordinate = fieldVertices[2].TextureCoordinate;

            effect = new BasicEffect(GameStateManager.Instance.GraphicsDevice);
        }

        public void LoadContent()
        {
            net = GameStateManager.Instance.Content.Load<Model>("Netzv2");

            texture = GameStateManager.Instance.Content.Load<Texture2D>("sand");
        }

        public void Draw(Camera camera)
        {
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            effect.TextureEnabled = true;
            effect.Texture = texture;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GameStateManager.Instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, fieldVertices, 0, 4);
            }

            DrawNet(camera);
            
        }

        public void DrawNet(Camera camera)
        {
            Matrix[] transforms = new Matrix[net.Bones.Count];
            net.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in net.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.06f, 0.05f, 0.05f)
                        * Matrix.CreateTranslation(new Vector3(0, 0, net_height));
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
