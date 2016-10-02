using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class SpectatorDolphin : Spectator
    {
        Model fin;
        Texture finTexture;
        float angleFin;
        bool finFalling;
        int wingDirection;
        Vector3 wingPosition;

        public SpectatorDolphin(Vector3 pos, Vector3 s) : base(pos, s)
        {
            position = pos;
            scale = s;

            if(pos.Y < 0)
            {
                wingDirection = 1;
                wingPosition = position + new Vector3(-5, 0, -1);
            }
            else
            {
                wingDirection = -1;
                wingPosition = position + new Vector3(5, 0, -1);
            }
        }

        public new void LoadContent()
        {
            texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/delfinUV");
            model = GameStateManager.Instance.Content.Load<Model>("Models/delfin");

            fin = GameStateManager.Instance.Content.Load<Model>("Models/delfinFlosse2");
            finTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/delfinFlosseUV");

            finFalling = false;

            base.LoadContent();
        }

        public new void UnloadContent()
        {

        }

        public new void Update()
        {
            if (cheering)
            {
                //Cheering
            }

            if (angleFin < -0.5 && !finFalling)
            {
                angleFin += 0.03f;
            }
            else
            {
                if (angleFin > -1)
                {
                    angleFin -= 0.03f;
                    finFalling = true;
                }
                else
                {
                    finFalling = false;
                }
            }
        }

        public new void Draw(Camera camera, int rotation)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) *
                                   Matrix.CreateScale(scale) * Matrix.CreateTranslation(position);
                    Matrix Projection = camera.ProjectionMatrix;
                    Matrix View = camera.ViewMatrix;
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

                    effect.Parameters["worldMatrix"].SetValue(World);
                    effect.Parameters["worldInverseTransposeMatrix"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["worldViewProjectionMatrix"].SetValue(World * View * Projection);

                    effect.Parameters["cameraPos"].SetValue(camera.Position);
                    effect.Parameters["globalAmbient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["numLights"].SetValue(4);

                    effect.Parameters["PointLightpos"].SetValue(GameScreen.Instance.Match.LightsPosition);
                    effect.Parameters["PointLightambient"].SetValue(GameScreen.Instance.Match.LightsAmbient);
                    effect.Parameters["PointLightdiffuse"].SetValue(GameScreen.Instance.Match.LightsDiffuse);
                    effect.Parameters["PointLightspecular"].SetValue(GameScreen.Instance.Match.LightsSpecular);
                    effect.Parameters["PointLightradius"].SetValue(GameScreen.Instance.Match.LightsRadius);

                    effect.Parameters["Materialambient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["Materialdiffuse"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                    effect.Parameters["Materialspecular"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                    effect.Parameters["Materialshininess"].SetValue(32.0f);

                    effect.Parameters["colorMapTexture"].SetValue(texture);
                }
                mesh.Draw();
            }

            FinDraw(camera, rotation);
        }

        public void FinDraw(Camera camera, int rotation)
        {
            Matrix[] transforms = new Matrix[fin.Bones.Count];
            fin.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in fin.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) * Matrix.CreateRotationY(wingDirection * angleFin) *
                                   Matrix.CreateScale(new Vector3(1, 1, 1)) * Matrix.CreateTranslation(wingPosition);
                    Matrix Projection = camera.ProjectionMatrix;
                    Matrix View = camera.ViewMatrix;
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

                    effect.Parameters["worldMatrix"].SetValue(World);
                    effect.Parameters["worldInverseTransposeMatrix"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["worldViewProjectionMatrix"].SetValue(World * View * Projection);

                    effect.Parameters["cameraPos"].SetValue(camera.Position);
                    effect.Parameters["globalAmbient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["numLights"].SetValue(4);

                    effect.Parameters["PointLightpos"].SetValue(GameScreen.Instance.Match.LightsPosition);
                    effect.Parameters["PointLightambient"].SetValue(GameScreen.Instance.Match.LightsAmbient);
                    effect.Parameters["PointLightdiffuse"].SetValue(GameScreen.Instance.Match.LightsDiffuse);
                    effect.Parameters["PointLightspecular"].SetValue(GameScreen.Instance.Match.LightsSpecular);
                    effect.Parameters["PointLightradius"].SetValue(GameScreen.Instance.Match.LightsRadius);

                    effect.Parameters["Materialambient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["Materialdiffuse"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                    effect.Parameters["Materialspecular"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                    effect.Parameters["Materialshininess"].SetValue(32.0f);

                    effect.Parameters["colorMapTexture"].SetValue(finTexture);
                }
                mesh.Draw();
            }
        }
    }
}
