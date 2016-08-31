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

        Effect effect2;
        Effect effect;
        Vector3 viewVector;

        int width;
        int length;
        int net_height;

        //Eisscholle
        Model ice;
        private Texture2D iceTexture;

        //Schiedsrichter
        Model referee;
        Model trillerpf;


        //Netz
        Model net;
        private Texture2D netTexture;
        BoundingBox netBoundingBox;
        BoundingBox boundingBox;

        //Texturen
        BasicEffect e;
        Texture2D texture;

        DebugDraw d;

        Skydome skydome;

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

            e = new BasicEffect(GameStateManager.Instance.GraphicsDevice);

            d = new DebugDraw(GameStateManager.Instance.GraphicsDevice);

            skydome = new Skydome(5.0f);
            skydome.Initialize();
        }

        public void LoadContent()
        {
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTest");
            effect2 = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTestWithTexture");

            net = GameStateManager.Instance.Content.Load<Model>("Models/netzv4");
            netTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/netzvUV");

            ice = GameStateManager.Instance.Content.Load<Model>("Models/eisscholle");
            iceTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/iceTexture");

            referee = GameStateManager.Instance.Content.Load<Model>("Models/3DAcaLogo");

            trillerpf = GameStateManager.Instance.Content.Load<Model>("Models/trillerpfeife");

            texture = GameStateManager.Instance.Content.Load<Texture2D>("Images/field2");

            skydome.Load();

            CreateBoundingBox();

            CreateNetBoundingBox();
        }

        public void Draw(Camera camera)
        {
            DrawIce(camera);
            DrawNet(camera);
            DrawReferee(camera);
            DrawTrillerpf(camera);
            skydome.Draw(camera);
            
            e.View = camera.ViewMatrix;
            e.Projection = camera.ProjectionMatrix;

            e.TextureEnabled = true;
            e.Texture = texture;
            
            foreach (var pass in e.CurrentTechnique.Passes)
            {
                pass.Apply();

                GameStateManager.Instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, fieldVertices, 0, 4);
            }

            /*d.Begin(camera.ViewMatrix, camera.ProjectionMatrix);
            d.DrawWireBox(netBoundingBox, Color.White);
            d.DrawWireBox(boundingBox, Color.White);
            d.End();*/

        }


        private void DrawIce(Camera camera)
        {
            Matrix[] transforms = new Matrix[ice.Bones.Count];
            ice.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ice.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.1f, 0.15f, 0.01f)
                           * Matrix.CreateTranslation(new Vector3(0, 0, -0.75f)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.1f, 0.15f, 0.01f)
                           * Matrix.CreateTranslation(new Vector3(0, 0, -0.75f))));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    //hier kommt immer die warnung
                    effect2.Parameters["ModelTexture"].SetValue(iceTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }

            /*   foreach (ModelMesh mesh in ice.Meshes)
               {
                   foreach (BasicEffect effect in mesh.Effects)
                   {
                       effect.EnableDefaultLighting();

                       effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.1f, 0.15f, 0.01f)
                           * Matrix.CreateTranslation(new Vector3(0, 0, -0.75f));
                       effect.View = camera.ViewMatrix;
                       effect.Projection = camera.ProjectionMatrix;
                   }
                   mesh.Draw();
               }*/
        }



        private void DrawNet(Camera camera)
        {
            Matrix[] transforms = new Matrix[net.Bones.Count];
            net.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in net.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.05f, 0.025f, 0.025f)
                            * Matrix.CreateTranslation(new Vector3(0, 0, 0)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.05f, 0.025f, 0.025f)
                            * Matrix.CreateTranslation(new Vector3(0, 0, 0)));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);
                    effect2.Parameters["ModelTexture"].SetValue(netTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }

            /*    foreach (ModelMesh mesh in net.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.05f, 0.025f, 0.025f)
                            * Matrix.CreateTranslation(new Vector3(0, 0, 0));
                        effect.View = camera.ViewMatrix;
                        effect.Projection = camera.ProjectionMatrix;
                    }
                    mesh.Draw();
                }*/
        }



        private void DrawReferee(Camera camera)
        {
            Matrix[] transforms = new Matrix[referee.Bones.Count];
            referee.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in referee.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                          * Matrix.CreateTranslation(new Vector3(70, 0, 0)));
                    effect.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                          * Matrix.CreateTranslation(new Vector3(70, 0, 0))));
                    effect.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }

            /*  foreach (ModelMesh mesh in referee.Meshes)
              {
                  foreach (BasicEffect effect in mesh.Effects)
                  {
                      effect.EnableDefaultLighting();

                      effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                          * Matrix.CreateTranslation(new Vector3(70, 0, 0));
                      effect.View = camera.ViewMatrix;
                      effect.Projection = camera.ProjectionMatrix;
                  }
                  mesh.Draw();
              }*/
        }

        private void DrawTrillerpf(Camera camera)
        {
            Matrix[] transforms = new Matrix[trillerpf.Bones.Count];
            trillerpf.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in trillerpf.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90))
                           * Matrix.CreateRotationY(MathHelper.ToRadians(-10)) * Matrix.CreateScale(0.03f, 0.03f, 0.03f)
                           * Matrix.CreateTranslation(new Vector3(66.5f, 0, 4)));
                    effect.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90))
                           * Matrix.CreateRotationY(MathHelper.ToRadians(-10)) * Matrix.CreateScale(0.03f, 0.03f, 0.03f)
                           * Matrix.CreateTranslation(new Vector3(66.5f, 0, 4))));
                    effect.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }


            /*   foreach (ModelMesh mesh in trillerpf.Meshes)
               {
                   foreach (BasicEffect effect in mesh.Effects)
                   {
                       effect.EnableDefaultLighting();

                       effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) 
                           * Matrix.CreateRotationY(MathHelper.ToRadians(-10)) * Matrix.CreateScale(0.03f, 0.03f, 0.03f)
                           * Matrix.CreateTranslation(new Vector3(66.5f, 0, 4));
                       effect.View = camera.ViewMatrix;
                       effect.Projection = camera.ProjectionMatrix;
                   }
                   mesh.Draw();
               }*/
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

            netBoundingBox = new BoundingBox(new Vector3(-54,-0.2f, 10), new Vector3(54, 0.2f, 20));
        }

        private void CreateBoundingBox()
        {
            boundingBox = new BoundingBox(new Vector3(-(width/2), -(length/2), 0), new Vector3(width/2, length/2, 40));
        }
    }
}
