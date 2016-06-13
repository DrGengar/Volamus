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
    public class Field
    {
        VertexPositionTexture[] fieldVertices = new VertexPositionTexture[12];

        int width;
        int length;
        int net_height;

        //Eisscholle
        Model ice;

        //Schiedsrichter
        Model referee;


        //Netz
        Model net;
        BoundingBox netBoundingBox;
        BoundingBox boundingBox;

        //Texturen
        BasicEffect effect;
        Texture2D texture;

        DebugDraw d;

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

        public BoundingBox NetBoundingBox
        {
            get { return netBoundingBox; }
        }

        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
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

            d = new DebugDraw(GameStateManager.Instance.GraphicsDevice);
        }

        public void LoadContent()
        {
            net = GameStateManager.Instance.Content.Load<Model>("netzv4");

            ice = GameStateManager.Instance.Content.Load<Model>("eisschollev1");

            referee = GameStateManager.Instance.Content.Load<Model>("3DAcaLogo");

            texture = GameStateManager.Instance.Content.Load<Texture2D>("sand");

            CreateBoundingBox();

            CreateNetBoundingBox();
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

            DrawIce(camera);
            DrawNet(camera);
            DrawReferee(camera);

            d.Begin(camera.ViewMatrix, camera.ProjectionMatrix);
            d.DrawWireBox(netBoundingBox, Color.White);
            d.DrawWireBox(boundingBox, Color.White);
            d.End();

        }


        private void DrawIce(Camera camera)
        {
            Matrix[] transforms = new Matrix[ice.Bones.Count];
            ice.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in ice.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.05f, 0.15f, 0.01f)
                        * Matrix.CreateTranslation(new Vector3(0, 0, -0.75f));
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }



        private void DrawNet(Camera camera)
        {
            Matrix[] transforms = new Matrix[net.Bones.Count];
            net.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in net.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.025f, 0.025f, 0.025f)
                        * Matrix.CreateTranslation(new Vector3(0, 0, 0));
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }



        private void DrawReferee(Camera camera)
        {
            Matrix[] transforms = new Matrix[referee.Bones.Count];
            referee.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in referee.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                        * Matrix.CreateTranslation(new Vector3(40, 0, 0));
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

        private void CreateNetBoundingBox()
        {
            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in net.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), Matrix.Identity);

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            netBoundingBox = new BoundingBox(new Vector3(-27,-0.2f, 10), new Vector3(27, 0.2f, 20));
        }

        private void CreateBoundingBox()
        {
            boundingBox = new BoundingBox(new Vector3(-(width/2), -(length/2), 0), new Vector3(width/2, length/2, 40));
        }
    }
}
