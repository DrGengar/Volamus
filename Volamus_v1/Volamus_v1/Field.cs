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

        //Schiedsrichter + Acamännchen
        Model referee;
        private Texture2D refTexture;
        Model trillerpf;
        private Texture2D trillerTexture;
        Model aca1;
        Model aca2;
        private Texture2D aca1Texture;
        private Texture2D aca2Texture;


        //Netz
        Model net;
        private Texture2D netTexture;
        BoundingBox netBoundingBox;

        //Texturen
        BasicEffect e;
        Texture2D texture;

        Skydome skydome;
        Texture2D skyTexture;
        Skydome skydome2;
        Texture2D skyTexture2;

        Model banner;
        Texture2D bannerTexture;
        float rotation;


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

            skydome = new Skydome(25f, false);
            skydome.Initialize();

            skydome2 = new Skydome(5f, true);
            skydome2.Initialize();
        }

        public void LoadContent()
        {
            effect2 = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTestWithTexture");

            net = GameStateManager.Instance.Content.Load<Model>("Models/netzv4");
            netTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/iceTexture");

            ice = GameStateManager.Instance.Content.Load<Model>("Models/eisscholle");
            iceTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/iceTexture");

            referee = GameStateManager.Instance.Content.Load<Model>("Models/3DAcaLogo");
            refTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/AcaTexture");
            aca1 = GameStateManager.Instance.Content.Load<Model>("Models/3DAcaLogo");
            aca1Texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/AcaTexture");
            aca2 = GameStateManager.Instance.Content.Load<Model>("Models/3DAcaLogo");
            aca2Texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/AcaTexture");

            trillerpf = GameStateManager.Instance.Content.Load<Model>("Models/trillerpfeife");
            trillerTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/AcaTexture");

            texture = GameStateManager.Instance.Content.Load<Texture2D>("Images/field2");

            skydome.Load();
            skyTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/skydome");
            skydome2.Load();
            skyTexture2 = GameStateManager.Instance.Content.Load<Texture2D>("Textures/skydomeCloud1");

            banner = GameStateManager.Instance.Content.Load<Model>("Models/fahne");
            bannerTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/fahne_textur");

            CreateNetBoundingBox();
        }

        public void Draw(Camera camera)
        {
            rotation = Ball.Instance.Wind.Angle;


            DrawBanner(camera);
            DrawIce(camera);
            DrawNet(camera);
            DrawReferee(camera);
            DrawTrillerpf(camera);
            DrawAca1(camera);
            DrawAca2(camera);

            skydome.Update(0.0025f);
            skydome.Draw(camera, skyTexture);
            //skydome2.Update(0.01f);
            //skydome2.Draw(camera, skyTexture2);

            e.View = camera.ViewMatrix;
            e.Projection = camera.ProjectionMatrix;

            e.TextureEnabled = true;
            e.Texture = texture;

            foreach (var pass in e.CurrentTechnique.Passes)
            {
                pass.Apply();

                GameStateManager.Instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, fieldVertices, 0, 4);
            }
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
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.2f, 0.3f, 0.01f)
                           * Matrix.CreateTranslation(new Vector3(0, 0, -0.75f)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.2f, 0.3f, 0.01f)
                           * Matrix.CreateTranslation(new Vector3(0, 0, -0.75f))));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);
                    effect2.Parameters["ModelTexture"].SetValue(iceTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }
        }

        private void DrawBanner(Camera camera)
        {
            Matrix[] transforms = new Matrix[banner.Bones.Count];
            banner.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in banner.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(270) - rotation) * Matrix.CreateScale(0.04f, 0.04f, 0.04f)
                           * Matrix.CreateTranslation(new Vector3(-50, 0, 0)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(270) - rotation) * Matrix.CreateScale(0.04f, 0.04f, 0.04f)
                           * Matrix.CreateTranslation(new Vector3(-50, 0, 0))));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    effect2.Parameters["ModelTexture"].SetValue(bannerTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();

                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(270) - rotation) * Matrix.CreateScale(0.04f, 0.04f, 0.04f)
                           * Matrix.CreateTranslation(new Vector3(50, 0, 0)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(270) - rotation) * Matrix.CreateScale(0.04f, 0.04f, 0.04f)
                           * Matrix.CreateTranslation(new Vector3(50, 0, 0))));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    effect2.Parameters["ModelTexture"].SetValue(bannerTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
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
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
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
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                          * Matrix.CreateTranslation(new Vector3(70, 0, 0)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                          * Matrix.CreateTranslation(new Vector3(70, 0, 0))));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    effect2.Parameters["ModelTexture"].SetValue(refTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }
        }

        private void DrawAca1(Camera camera)
        {
            Matrix[] transforms = new Matrix[aca1.Bones.Count];
            aca1.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in aca1.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                          * Matrix.CreateTranslation(new Vector3(80, -8, 0)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                          * Matrix.CreateTranslation(new Vector3(80, -8, 0))));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    effect2.Parameters["ModelTexture"].SetValue(refTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }
        }
        private void DrawAca2(Camera camera)
        {
            Matrix[] transforms = new Matrix[aca2.Bones.Count];
            aca2.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in aca2.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                          * Matrix.CreateTranslation(new Vector3(80, 8, 0)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                          * Matrix.CreateTranslation(new Vector3(80, 8, 0))));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    effect2.Parameters["ModelTexture"].SetValue(refTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }
        }

        private void DrawTrillerpf(Camera camera)
        {
            Matrix[] transforms = new Matrix[trillerpf.Bones.Count];
            trillerpf.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in trillerpf.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90))
                           * Matrix.CreateRotationY(MathHelper.ToRadians(-10)) * Matrix.CreateScale(0.03f, 0.03f, 0.03f)
                           * Matrix.CreateTranslation(new Vector3(66.5f, 0, 4)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90))
                           * Matrix.CreateRotationY(MathHelper.ToRadians(-10)) * Matrix.CreateScale(0.03f, 0.03f, 0.03f)
                           * Matrix.CreateTranslation(new Vector3(66.5f, 0, 4))));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);
                    effect2.Parameters["ModelTexture"].SetValue(trillerTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }
        }

        private void CreateNetBoundingBox()
        {
            netBoundingBox = new BoundingBox(new Vector3(-54, -0.2f, 10), new Vector3(54, 0.2f, 20));
        }
    }
}
