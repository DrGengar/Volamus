using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class SpectatorBumbleBee : Spectator
    {
        Model leftWing, rightWing;
        Texture leftwingTexture, rightwingTexture;

        Vector3 leftWingPosition, rightWingPosition;

        int rotateWing1, rotateWing2;
        float angleWing;

        bool wingfalling;

        public SpectatorBumbleBee(Vector3 pos, Vector3 s) : base(pos, s)
        {
            position = pos;
            scale = s;
        }

        public new void LoadContent()
        {
            texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/hummelUV");
            model = GameStateManager.Instance.Content.Load<Model>("Models/hummel");

            is_falling = false;
            wingfalling = false;

            if (position.Y < 0)
            {
                rotateWing1 = 180;
                rotateWing2 = 0;
            }
            else
            {
                rotateWing2 = 180;
                rotateWing1 = 0;
            }

            rightWingPosition = new Vector3(position.X, position.Y - 1, position.Z + 2);
            leftWingPosition = new Vector3(position.X, position.Y + 1, position.Z + 2);

            leftWing = GameStateManager.Instance.Content.Load<Model>("Models/hummelWing3");
            leftwingTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/HummelWingUV");
            rightWing = GameStateManager.Instance.Content.Load<Model>("Models/hummelWing3");
            rightwingTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/HummelWingUV");

            base.LoadContent();
        }

        public new void UnloadContent()
        {

        }

        public new void Update()
        {
            if (cheering)
            {
                //cheering
            }

            if (angleWing < 1 && !wingfalling)
            {
                angleWing += 0.04f;
            }
            else
            {
                if (angleWing > 0.5)
                {
                    angleWing -= 0.04f;
                    wingfalling = true;
                }
                else
                {
                    wingfalling = false;
                }
            }

            if (position.Z < 7 && !is_falling)
            {
                position = position + new Vector3(0, 0, 0.05f);
            }
            else
            {
                if (position.Z > 5)
                {
                    position = position - new Vector3(0, 0, 0.05f);
                    is_falling = true;
                }
                else
                {
                    is_falling = false;
                }
            }

            rightWingPosition = new Vector3(position.X, position.Y - 1, position.Z + 2);
            leftWingPosition = new Vector3(position.X, position.Y + 1, position.Z + 2);
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
                                   Matrix.CreateScale(new Vector3(2, 2, 2)) * Matrix.CreateTranslation(position);
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
            WingDrawL(camera, rotation);
            WingDrawR(camera, rotation);
        }

        public void WingDrawL(Camera camera, int rotation)
        {
            Matrix[] transforms = new Matrix[leftWing.Bones.Count];
            leftWing.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in leftWing.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationY(MathHelper.ToRadians(20) + angleWing) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotateWing1 + rotation))
                        * Matrix.CreateScale(new Vector3(3, 3, 3)) * Matrix.CreateTranslation(leftWingPosition);
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

                    effect.Parameters["colorMapTexture"].SetValue(leftwingTexture);
                }
                mesh.Draw();
            }
        }

        public void WingDrawR(Camera camera, int rotation)
        {
            Matrix[] transforms = new Matrix[rightWing.Bones.Count];
            rightWing.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in rightWing.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationY(MathHelper.ToRadians(20) + angleWing) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotateWing2 + rotation))
                        * Matrix.CreateScale(new Vector3(3, 3, 3)) * Matrix.CreateTranslation(rightWingPosition);
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

                    effect.Parameters["colorMapTexture"].SetValue(rightwingTexture);
                }
                mesh.Draw();
            }
        }
    }
}
