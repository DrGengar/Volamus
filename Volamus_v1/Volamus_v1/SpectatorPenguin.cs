using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class SpectatorPenguin : Spectator
    {
        Model leftWing, rightWing;
        Texture wingTexture;

        Vector3 leftWingPosition, rightWingPosition;
        int hitAngleHigh;

        public SpectatorPenguin(Vector3 pos, Vector3 s) : base(pos, s)
        {
            position = pos;
            scale = s;

            leftWingPosition = new Vector3(position.X, position.Y - 1, position.Z - 3);
            rightWingPosition = new Vector3(position.X, position.Y + 1, position.Z - 3);
        }

        public new void LoadContent()
        {
            model = GameStateManager.Instance.Content.Load<Model>("Models/pinguin");

            leftWing = GameStateManager.Instance.Content.Load<Model>("Models/wingLeft");
            rightWing = GameStateManager.Instance.Content.Load<Model>("Models/wingRight");

            texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/pinguinUV");
            wingTexture = GameStateManager.Instance.Content.Load<Texture2D>("Models/PingWingUV");

            base.LoadContent();
        }

        public new void UnloadContent()
        {
            base.UnloadContent();
        }

        public new void Update()
        {
            if (cheering)
            {
                //Flügel gehen nach oben
                hitAngleHigh = 40;

                //hüpft
                if (position.Z < 7 && !is_falling)
                {
                    position.Z += 0.2f;
                }
                else
                {
                    if (position.Z > 0)
                    {
                        position.Z -= 0.2f;
                        is_falling = true;
                    }
                    else
                    {
                        is_falling = false;
                    }
                }

                leftWingPosition = new Vector3(position.X, position.Y + 6, position.Z - 2);
                rightWingPosition = new Vector3(position.X, position.Y - 6, position.Z - 2);
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
                    effect.Parameters["numLights"].SetValue(GameScreen.Instance.Match.LightsNumber);

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

            DrawWingLeft(camera, leftWing, leftWingPosition);
            DrawWingRight(camera, rightWing, rightWingPosition);
        }

        public void DrawWingRight(Camera camera, Model wing, Vector3 position)
        {
            Matrix[] transforms = new Matrix[wing.Bones.Count];
            wing.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in wing.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90 - hitAngleHigh)) * Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                                   Matrix.CreateScale(scale * 5) * Matrix.CreateTranslation(position);
                    Matrix Projection = camera.ProjectionMatrix;
                    Matrix View = camera.ViewMatrix;
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

                    effect.Parameters["worldMatrix"].SetValue(World);
                    effect.Parameters["worldInverseTransposeMatrix"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["worldViewProjectionMatrix"].SetValue(World * View * Projection);

                    effect.Parameters["cameraPos"].SetValue(camera.Position);
                    effect.Parameters["globalAmbient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["numLights"].SetValue(GameScreen.Instance.Match.LightsNumber);

                    effect.Parameters["PointLightpos"].SetValue(GameScreen.Instance.Match.LightsPosition);
                    effect.Parameters["PointLightambient"].SetValue(GameScreen.Instance.Match.LightsAmbient);
                    effect.Parameters["PointLightdiffuse"].SetValue(GameScreen.Instance.Match.LightsDiffuse);
                    effect.Parameters["PointLightspecular"].SetValue(GameScreen.Instance.Match.LightsSpecular);
                    effect.Parameters["PointLightradius"].SetValue(GameScreen.Instance.Match.LightsRadius);

                    effect.Parameters["Materialambient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["Materialdiffuse"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                    effect.Parameters["Materialspecular"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                    effect.Parameters["Materialshininess"].SetValue(32.0f);

                    effect.Parameters["colorMapTexture"].SetValue(wingTexture);
                }
                mesh.Draw();
            }
        }

        // für linken Flügel
        public void DrawWingLeft(Camera camera, Model wing, Vector3 position)
        {
            Matrix[] transforms = new Matrix[wing.Bones.Count];
            wing.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in wing.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90 + hitAngleHigh)) * Matrix.CreateRotationZ(MathHelper.ToRadians(0)) *
                          Matrix.CreateScale(scale * 5) * Matrix.CreateTranslation(position);
                    Matrix Projection = camera.ProjectionMatrix;
                    Matrix View = camera.ViewMatrix;
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

                    effect.Parameters["worldMatrix"].SetValue(World);
                    effect.Parameters["worldInverseTransposeMatrix"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["worldViewProjectionMatrix"].SetValue(World * View * Projection);

                    effect.Parameters["cameraPos"].SetValue(camera.Position);
                    effect.Parameters["globalAmbient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["numLights"].SetValue(GameScreen.Instance.Match.LightsNumber);

                    effect.Parameters["PointLightpos"].SetValue(GameScreen.Instance.Match.LightsPosition);
                    effect.Parameters["PointLightambient"].SetValue(GameScreen.Instance.Match.LightsAmbient);
                    effect.Parameters["PointLightdiffuse"].SetValue(GameScreen.Instance.Match.LightsDiffuse);
                    effect.Parameters["PointLightspecular"].SetValue(GameScreen.Instance.Match.LightsSpecular);
                    effect.Parameters["PointLightradius"].SetValue(GameScreen.Instance.Match.LightsRadius);

                    effect.Parameters["Materialambient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["Materialdiffuse"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                    effect.Parameters["Materialspecular"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                    effect.Parameters["Materialshininess"].SetValue(32.0f);

                    effect.Parameters["colorMapTexture"].SetValue(wingTexture);
                }
                mesh.Draw();
            }
        }
    }
}
