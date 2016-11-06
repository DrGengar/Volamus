using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Confetti
    {
        Effect effect;
        List<Drop> confetti;
        Model dr;
        Texture2D texture;
        Vector3 positionOne, positionTwo;

        public Confetti(Vector3 one, Vector3 two)
        {
            confetti = new List<Drop>();
            positionOne = one;
            positionTwo = two;

            dr = GameStateManager.Instance.Content.Load<Model>("Models/confetti2");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shader");
        }

        public void Update(Random rnd)
        {
            int total = rnd.Next(100);

            while (confetti.Count < total)
            {

                confetti.Add(Generate(positionOne, rnd));
                confetti.Add(Generate(positionTwo, rnd));
            }


            for (int Drop = 0; Drop < confetti.Count; Drop++)
            {
                confetti[Drop].UpdateEnd();

                if (confetti[Drop].ttl <= 0)
                {
                    confetti.RemoveAt(Drop);
                    Drop--;
                }
            }
        }

        private Drop Generate(Vector3 loc, Random rnd)
        {
            int timeToLive = 100 + rnd.Next(40);
            Vector3 velo = new Vector3(rnd.Next(-5, 5), rnd.Next(-5, 5), rnd.Next(5, 10));
            velo = velo / 100;

            int temp = new Random().Next(1, 11);
            switch (temp)
            {
                case 1:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/AcaTexture");
                    break;
                case 2:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/blau");
                    break;
                case 3:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/blau2");
                    break;
                case 4:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green");
                    break;
                case 5:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green2");
                    break;
                case 6:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green3");
                    break;
                case 7:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/red");
                    break;
                case 8:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/pink");
                    break;
                case 9:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/lila");
                    break;
                case 10:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/gelb");
                    break;
                default:
                    texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/BeachBallTexture");
                    break;
            }

            return new Drop(loc, timeToLive, dr, velo, texture);
        }

        public void Draw(Camera camera)
        {
            for (int index = 0; index < confetti.Count; index++)
            {
                Matrix[] transforms = new Matrix[dr.Bones.Count];
                dr.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in dr.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effect;

                        Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                                       Matrix.CreateScale(1.50f, 1.50f, 2.0f) * Matrix.CreateTranslation(confetti[index].Position);
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

                        effect.Parameters["colorMapTexture"].SetValue(confetti[index].Texture);
                    }
                    mesh.Draw();
                }
            }
        }
    }
}
