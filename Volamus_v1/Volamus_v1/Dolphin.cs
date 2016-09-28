using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Dolphin(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field) : base(pos, ma_j_height, mi_j_height, j_velo, mvp, field){ }

        public Dolphin(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field, PlayerIndex i) : base(pos, ma_j_height, mi_j_height, j_velo, mvp, field, i) { }

        public new void LoadContent()
        {
            Texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/delfinUV");
            model = GameStateManager.Instance.Content.Load<Model>("Models/delfin");
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

        private void CreateBoundingBoxes()
        {

            Vector3 min = new Vector3(-2f, -6f, 0);
            Vector3 max = new Vector3(2f, 6f, 8);

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

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(rotation + Direction * (-Gamma))) *
                                   Matrix.CreateScale(new Vector3(5, 5, 5)) * Matrix.CreateTranslation(Position);
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
            DebugDraw d = new DebugDraw(GameStateManager.Instance.GraphicsDevice);
            d.Begin(camera.ViewMatrix, camera.ProjectionMatrix);
            d.DrawWireBox(innerBoundingBox, Color.Black);
            d.DrawWireBox(outerBoundingBox, Color.Black);
            d.End();
        }
    }
}
