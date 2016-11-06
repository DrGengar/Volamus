using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class PickVeloMinus
    {
        Effect effect;
        List<Drop> dropsVelo;
        Model dr;
        Texture2D texture;


        // Einsammeln verringert die Laufgeschwindigkeit des Gegeners, bis der Ball den Boden berührt
        public PickVeloMinus()
        {
            this.dropsVelo = new List<Drop>();
        }

        public void LoadContent()
        {
            dr = GameStateManager.Instance.Content.Load<Model>("Models/PUveloM");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shader");
            texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green2");
        }

        public void Update(Random rnd)
        {   //hinzufügen neuer Drops

            int total = rnd.Next(2);

            while (dropsVelo.Count < total)
            {
                dropsVelo.Add(Generate(rnd));
            }

            //aktualisieren der Drops
            for (int Drop = 0; Drop < dropsVelo.Count; Drop++)
            {

                dropsVelo[Drop].UpdateVelo();

                if (dropsVelo[Drop].ttl <= 0)
                {
                    dropsVelo.RemoveAt(Drop);
                }

                if (Drop < dropsVelo.Count && Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerOne, dropsVelo[Drop]))
                {
                    dropsVelo.RemoveAt(Drop);

                    if (GameScreen.Instance.Match.PlayerOne.Enemy.Movespeed > 0.5f)
                    {
                        GameScreen.Instance.Match.PlayerOne.Enemy.Movespeed -= 0.1f;
                    }
                }

                if (Drop < dropsVelo.Count && Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerTwo, dropsVelo[Drop]))
                {
                    dropsVelo.RemoveAt(Drop);

                    if (GameScreen.Instance.Match.PlayerTwo.Enemy.Movespeed > 0.5f)
                    {
                        GameScreen.Instance.Match.PlayerTwo.Enemy.Movespeed -= 0.1f;
                    }
                }
            }

            if (GameScreen.Instance.Match.IsFinished)
            {
                dropsVelo.RemoveAll(item => item.ttl != 0);  //alle Drops die aktuell noch leben werden entfernt
            }
        }

        private Drop Generate(Random rnd)
        {
            float x = rnd.Next(-50, 50);
            float y = rnd.Next(-45, 46);
            int zufall = rnd.Next(1, 11);
            int timeToLive = 200 + rnd.Next(80);

            if (GameScreen.Instance.Match.WhichField == 2)
            {
                return new Drop(new Vector3(x, y, 2.0f), timeToLive, dr, texture);
            }
            else
            {
                return new Drop(new Vector3(x, y, 0.5f), timeToLive, dr, texture);
            }
        }

        public void Draw(Camera camera)
        {
            for (int index = 0; index < dropsVelo.Count; index++)
            {
                Matrix[] transforms = new Matrix[dr.Bones.Count];
                dr.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in dr.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effect;
                        Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(-125 + 180)) *
                                       Matrix.CreateScale(1.50f, 1.50f, 2.0f) * Matrix.CreateTranslation(dropsVelo[index].Position + new Vector3(0, 0, 1));
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
            }

        }
    }
}
