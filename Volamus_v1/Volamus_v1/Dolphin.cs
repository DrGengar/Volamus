using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Dolphin : Player
    {
        int rotation;
        Model fin;
        Texture finTexture;
        float angleFin;
        bool finFalling;
        float rotateEndeOderMatch; //während Match 90, am ende 45

        //Jubel
        bool moving_right;
        bool moving_left;
        float rotateModel;
        float angleSin;
        bool auftauchen;
        bool tauchen;
        float rotateModelSin;
        float posYAlt;
        float zLooser;

        public Dolphin(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field) : base(pos, ma_j_height, mi_j_height, j_velo, mvp, field) { }

        public Dolphin(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field, PlayerIndex i) : base(pos, ma_j_height, mi_j_height, j_velo, mvp, field, i) { }

        public new void LoadContent()
        {
            zLooser = Position.Z-5;
            rotateEndeOderMatch = 90;
            auftauchen = true;
            tauchen = false;
            moving_left = false;
            moving_right = true;
            finFalling = false;
            Texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/delfinUV");
            model = GameStateManager.Instance.Content.Load<Model>("Models/delfin2");
            fin = GameStateManager.Instance.Content.Load<Model>("Models/delfinFlosse2");
            finTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/delfinFlosseUV");
            CreateBoundingBoxes();

            if (Direction == 1)
            {
                rotation = 0;
            }
            else
            {
                rotation = 180;
            }

            base.LoadContent();
        }

        public void UpdateAnim()
        {
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

        public void CheeringD()
        {
            Gamma = 0;
            
            if (this == GameScreen.Instance.Match.Winner)
            {
                rotateEndeOderMatch = 45;
                if (Position.X >= GameScreen.Instance.Match.Field.Width / 2) //an rechter Feldgrenze
                {
                    moving_right = false;
                    moving_left = true;
                }
                if (Position.X <= -GameScreen.Instance.Match.Field.Width / 2) //linker Rand
                {
                    moving_left = false;
                    moving_right = true;
                }
                if (moving_right)
                {
                    posYAlt = Position.Z;
                    Position = new Vector3(Position.X + 0.5f, Position.Y, 3 * (float)Math.Sin(angleSin / 4) - 1);
                    rotateModel = 90 * -Direction;
                }
                if (moving_left)
                {
                    posYAlt = Position.Z;
                    Position = new Vector3(Position.X - 0.5f, Position.Y, 3 * (float)Math.Sin(angleSin / 4) - 1);
                    rotateModel = -90 * -Direction;
                }
                if (posYAlt <= Position.Z && Position.Z >= -1)
                {
                    auftauchen = true;
                    tauchen = false;
                }
                if (posYAlt >= Position.Z && Position.Z <= -1)
                {
                    auftauchen = false;
                    tauchen = true;
                }

                if (auftauchen)
                {
                    rotateModelSin += 1f;
                }
                if (tauchen)
                {
                    rotateModelSin -= 1f;
                }
                angleSin += 0.4f; //0.5
            }
            else
            {
                rotateEndeOderMatch = 130;
                Position = new Vector3(Position.X, Position.Y,  zLooser);
            }
        }

        private void CreateBoundingBoxes()
        {

            Vector3 min = new Vector3(-3f, 0f, 0);
            Vector3 max = new Vector3(3f, 10f, 9);

            Vector3 mid = new Vector3((max.X + min.X) / 2, (Direction) * (max.Y + min.Y) / 2, min.Z);
            Vector3 translate = mid - Position;

            min.X -= translate.X;
            max.X -= translate.X;

            min.Y += Position.Y;
            max.Y += Position.Y;

            if(Direction == -1)
            {
                min.Y -= 10.0f;
                max.Y -= 10.0f;
            }

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

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(rotateEndeOderMatch - Alpha + rotateModelSin)) * Matrix.CreateRotationY(MathHelper.ToRadians(Betta)) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation + Direction * (-Gamma) + rotateModel)) *
                                   Matrix.CreateScale(new Vector3(3, 3, 3)) * Matrix.CreateTranslation(Position);
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

                    effect.Parameters["colorMapTexture"].SetValue(Texture);
                }
                mesh.Draw();
            }
            FinDraw(camera);
        }

        public void FinDraw(Camera camera)
        {
            Matrix[] transforms = new Matrix[fin.Bones.Count];
            fin.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in fin.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(MathHelper.ToRadians(-Betta)) * Matrix.CreateRotationX(MathHelper.ToRadians(90) + angleFin - Alpha) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation + Direction * (-Gamma)) + rotateModel) *
                                   Matrix.CreateScale(new Vector3(1, 1, 1)) * Matrix.CreateTranslation(new Vector3(Position.X, Position.Y - Direction * 1, Position.Z + 1));
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
