using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class BumbleBee : Player
    {
        int rotation;
        int rotateWing1;
        int rotateWing2;
        float angleWing;
        Model wingLeft;
        Model wingRight;
        Texture wingTextureL;
        Texture wingTextureR;
        bool falling;
        bool wingfalling;
        //Jubel:
        float rotate = 90f;
        float rotateModel;
        float test;
        float x;
        float y;

        public BumbleBee(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field) : base(pos, ma_j_height, mi_j_height, j_velo, mvp, field) { }

        public BumbleBee(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field, PlayerIndex i) : base(pos, ma_j_height, mi_j_height, j_velo, mvp, field, i) { }

        public new void LoadContent()
        {
            falling = false;
            wingfalling = false;
            Texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/hummelUV");
            model = GameStateManager.Instance.Content.Load<Model>("Models/hummel");

            if (Direction == 1)
            {
                rotateWing1 = 180;
                rotateWing2 = 0;
                rotation = 0;
            }
            else
            {
                rotateWing2 = 180;
                rotateWing1 = 0;
                rotation = 180;
            }

            wingLeft = GameStateManager.Instance.Content.Load<Model>("Models/hummelWing3");
            wingTextureL = GameStateManager.Instance.Content.Load<Texture2D>("Textures/HummelWingUV");
            wingRight = GameStateManager.Instance.Content.Load<Model>("Models/hummelWing3");
            wingTextureR = GameStateManager.Instance.Content.Load<Texture2D>("Textures/HummelWingUV");

            CreateBoundingBoxes();
            base.LoadContent();
        }

        public void UpdateAnim()
        {
            if (Position.Z < 7 && !falling)
            {
                Position = Position + new Vector3(0, 0, 0.05f);
                MovingBoundingBoxes(new Vector3(0, 0, 0.05f));
            }
            else
            {
                if (Position.Z > 5)
                {
                    Position = Position - new Vector3(0, 0, 0.05f);
                    MovingBoundingBoxes(new Vector3(0, 0, -0.05f));
                    falling = true;
                }
                else
                {
                    falling = false;
                }
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
        }

        public void CheeringB()
        {
            Gamma = 0;

            if (this == GameScreen.Instance.Match.Winner)
            {
                //Startposition der Rotation bestimmen
                if (rotate == 90)
                {
                    rotate = 90;
                    if (GameScreen.Instance.Match.Winner.Direction == 1)
                    {
                        rotate = 270; // cos 0, sin -1
                        test = rotate + 2 * MathHelper.Pi; //360° = 2Pi
                    }
                    else
                    {
                        rotate = 90;  //cos 0, sin 1
                        test = rotate + 2 * MathHelper.Pi;
                    }
                }

                Position = new Vector3(x, y, Position.Z);
                x = 15 * (float)Math.Cos(rotate);
                y = -Direction * 25 + 15 * (float)Math.Sin(rotate);
                rotate += 0.03f;
                rotateModel += 0.03f;
            }
        }

        private void CreateBoundingBoxes()
        {
            Vector3 min = new Vector3(-3f, -5f, 3);
            Vector3 max = new Vector3(3f, 5f, 8);

            Vector3 mid = new Vector3((max.X + min.X) / 2, (Direction) * (max.Y + min.Y) / 2, min.Z);
            Vector3 translate = mid - Position;

            min.X -= translate.X;
            max.X -= translate.X;

            min.Y += Position.Y;
            max.Y += Position.Y;

            innerBoundingBox = new BoundingBox(min, max);

            //äußere BoundingBox
            Vector3 offset = new Vector3(1.5f * Ball.Instance.BoundingSphere.Radius, 1.5f * Ball.Instance.BoundingSphere.Radius, 0);
            outerBoundingBox = new BoundingBox((innerBoundingBox.Min - offset),
                innerBoundingBox.Max + offset);
            outerBoundingBox.Max.Z += 1.5f * Ball.Instance.BoundingSphere.Radius;
        }

        public new void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation + Direction * (-Gamma)) + rotateModel) *
                                   Matrix.CreateScale(new Vector3(2, 2, 2)) * Matrix.CreateTranslation(Position);
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

                    effect.Parameters["colorMapTexture"].SetValue(Texture);
                }
                mesh.Draw();
            }
            WingDrawL(camera);
            WingDrawR(camera);
        }

        public void WingDrawL(Camera camera)
        {
            Matrix[] transforms = new Matrix[wingLeft.Bones.Count];
            wingLeft.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in wingLeft.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationY(MathHelper.ToRadians(20) + angleWing) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotateWing1 + rotation + Direction * (-Gamma)) + rotateModel) *
                                   Matrix.CreateScale(new Vector3(3, 3, 3)) * Matrix.CreateTranslation(new Vector3(Position.X - 1, Position.Y, Position.Z + 2));
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

                    effect.Parameters["colorMapTexture"].SetValue(wingTextureL);
                }
                mesh.Draw();
            }
        }

        public void WingDrawR(Camera camera)
        {
            Matrix[] transforms = new Matrix[wingRight.Bones.Count];
            wingRight.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in wingRight.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationY(MathHelper.ToRadians(20) + angleWing) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotateWing2 + rotation + Direction * (-Gamma)) + rotateModel) *
                                   Matrix.CreateScale(new Vector3(3, 3, 3)) * Matrix.CreateTranslation(new Vector3(Position.X + 1, Position.Y, Position.Z + 2));
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

                    effect.Parameters["colorMapTexture"].SetValue(wingTextureR);
                }
                mesh.Draw();
            }
        }

        public new void DrawArrow(Camera camera)
        {
            float temp = 0.0f;

            if (Direction == -1)
            {
                temp = 180.0f;
            }

            Matrix[] transforms = new Matrix[Pfeil.Bones.Count];
            Pfeil.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in Pfeil.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(temp + (Direction) * (-Gamma))) *
                                   Matrix.CreateScale(0.03f, 0.04f, 0.01f) * Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, Position.Z - 2.25f));
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

                    if (CanHit)
                    {
                        effect.Parameters["colorMapTexture"].SetValue(arrowTexture2);
                    }
                    else
                    {
                        effect.Parameters["colorMapTexture"].SetValue(arrowTexture);
                    }
                }
                mesh.Draw();
            }
        }
    }
}
