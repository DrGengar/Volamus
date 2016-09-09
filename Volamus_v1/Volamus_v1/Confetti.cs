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
    class Confetti
    {
        Effect effect;
        List<Drop> confetti;
        Model dr;
        Texture2D texture;
        int direction;

        //Konfetti an den Ecken des Gewinners

        public Confetti(int dir)
        {
            confetti = new List<Drop>();
            direction = dir;

            dr = GameStateManager.Instance.Content.Load<Model>("Models/confetti2");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTestWithTexture");

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
        }

        public void Update(Random rnd)
        {
            int total = rnd.Next(100);

            while (confetti.Count < total)
            {
                if (direction == 1)
                {
                    Vector3 location = new Vector3(-50, -45, 0);
                    Vector3 location2 = new Vector3(+50, -45, 0);
                    confetti.Add(Generate(location, rnd));
                    confetti.Add(Generate(location2, rnd));
                }
                else
                {
                    Vector3 location = new Vector3(-50, +45, 0);
                    Vector3 location2 = new Vector3(+50, +45, 0);
                    confetti.Add(Generate(location, rnd));
                    confetti.Add(Generate(location2, rnd));
                }
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
                        effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                                Matrix.CreateScale(1.50f, 1.50f, 2.0f)
                                * Matrix.CreateTranslation(confetti[index].Position));
                        effect.Parameters["View"].SetValue(camera.ViewMatrix);
                        effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                        Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                                Matrix.CreateScale(1.5f, 1.5f, 1.5f)
                                * Matrix.CreateTranslation(confetti[index].Position)));
                        effect.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                        effect.Parameters["ModelTexture"].SetValue(confetti[index].Texture);

                        Vector3 viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                        viewVector.Normalize();
                        effect.Parameters["ViewVector"].SetValue(viewVector);
                    }
                    mesh.Draw();
                }
            }

        }

    }
}
