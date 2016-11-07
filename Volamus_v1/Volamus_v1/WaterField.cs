using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class WaterField : Field 
    {
        Ocean ocean;
        Texture2D skyTexture;
        Model gummyboat;
        Texture2D gummyboatTexture;
        
        SpectatorGroupDolphin groupOne, groupTwo;

        ConfettiFish confetti;

        public ConfettiFish Confetti
        {
            get { return confetti; }
            set { confetti = value; }
        }

        public SpectatorGroupDolphin GroupOne
        {
            get { return groupOne; }
        }

        public SpectatorGroupDolphin GroupTwo
        {
            get { return groupTwo; }
        }

        public WaterField(int w, int l, int n_h, Random rnd) : base(w, l, n_h)
        {
            groupOne = new SpectatorGroupDolphin(new Vector3(-w / 2 - 10, -l / 2 - 5, 0), 5, rnd);
            groupTwo = new SpectatorGroupDolphin(new Vector3(w / 2 + 10, l / 2 + 5, 0), 5, rnd);
        }

        public new void LoadContent()
        {
            groupOne.LoadContent();
            groupTwo.LoadContent();

            skyTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/himmelrot");
            skydome = new Skydome(25f, false, skyTexture);
            skydome.Load();

            netTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/netTextureSeaWeed");

            ocean = new Ocean();
            ocean.LoadContent();

            gummyboat = GameStateManager.Instance.Content.Load<Model>("Models/schlauchboot");
            gummyboatTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/orange");

            base.LoadContent();
        }

        public void UnloadContent()
        {
            groupOne.UnloadContent();
            groupTwo.UnloadContent();
        }

        public void Update(Random rnd)
        {
            groupOne.Update();
            groupTwo.Update();
            if (confetti != null)
            {
                confetti.Update(rnd);
            }
        }

        public new void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[gummyboat.Bones.Count];
            gummyboat.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in gummyboat.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)) *
                                   Matrix.CreateScale(0.05f, 0.075f, 0.04f) * Matrix.CreateTranslation(new Vector3(77, 0, 0));
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
                    effect.Parameters["colorMapTexture"].SetValue(gummyboatTexture);
                }
                mesh.Draw();
            }

            ocean.Draw(GameStateManager.Instance.GameTime, camera, skyTexture, new Vector3(0, 0, -0.05f));
            base.Draw(camera);

            groupOne.Draw(camera);
            groupTwo.Draw(camera);

            if(confetti != null)
            {
                confetti.Draw(camera);
            }

        }
    }
}
