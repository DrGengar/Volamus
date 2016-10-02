using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class MeadowField : Field
    {
        Model ice;
        Texture2D iceTexture;

        SpectatorGroupBumbleBee groupOne, groupTwo;

        public SpectatorGroupBumbleBee GroupOne
        {
            get { return groupOne; }
        }

        public SpectatorGroupBumbleBee GroupTwo
        {
            get { return groupTwo; }
        }

        public MeadowField(int w, int l, int n_h, Random rnd) : base(w, l, n_h)
        {
            groupOne = new SpectatorGroupBumbleBee(new Vector3(-w / 2 - 10, -l / 2 - 5, 5), 5, rnd);
            groupTwo = new SpectatorGroupBumbleBee(new Vector3(w / 2 + 10, l / 2 + 5, 5), 5, rnd);
        }

        public new void LoadContent()
        {
            groupOne.LoadContent();
            groupTwo.LoadContent();

            skydome = new Skydome(25f, false, GameStateManager.Instance.Content.Load<Texture2D>("Textures/wolken"));
            skydome.Load();

            ice = GameStateManager.Instance.Content.Load<Model>("Models/eisscholle");
            netTexture = iceTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/iceTexture");

            base.LoadContent();
        }

        public void UnloadContent()
        {
            groupOne.UnloadContent();
            groupTwo.UnloadContent();
        }

        public new void Update()
        {
            groupOne.Update();
            groupTwo.Update();
        }

        public new void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[ice.Bones.Count];
            ice.CopyAbsoluteBoneTransformsTo(transforms);
            foreach (ModelMesh mesh in ice.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.2f, 0.3f, 0.01f)
                                   * Matrix.CreateTranslation(new Vector3(0, 0, -0.75f));
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

                    effect.Parameters["colorMapTexture"].SetValue(iceTexture);
                }
                mesh.Draw();
            }

            base.Draw(camera);

            groupOne.Draw(camera);
            groupTwo.Draw(camera);

        }
    }
}
