using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class PickVeloPlus
    {
        Effect effect;
        List<Drop> dropsVelo;
        Model dr;
        Texture2D texture;


        // Einsammeln verringert die Laufgeschwindigkeit des Gegeners, bis der Ball den Boden berührt
        public PickVeloPlus()
        {
            this.dropsVelo = new List<Drop>();
        }

        public void LoadContent()
        {
            dr = GameStateManager.Instance.Content.Load<Model>("Models/PUvelo");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTestWithTexture");
            texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/green3");
        }

        public void Update(Random rnd)
        {   //hinzufügen neuer Drops

            int total = rnd.Next(3);

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

                    if (GameScreen.Instance.Match.PlayerOne.Movespeed < 1.5f)
                    {
                        GameScreen.Instance.Match.PlayerOne.Movespeed += 0.1f;
                    }
                }

                if (Drop < dropsVelo.Count && Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerTwo, dropsVelo[Drop]))
                {
                    dropsVelo.RemoveAt(Drop);

                    if (GameScreen.Instance.Match.PlayerTwo.Movespeed < 2.0f)
                    {
                        GameScreen.Instance.Match.PlayerTwo.Movespeed += 0.1f;
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

            return new Drop(new Vector3(x, y, 0.5f), timeToLive, dr, texture);
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
                        effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                                Matrix.CreateScale(1.50f, 1.50f, 2.0f)
                                * Matrix.CreateTranslation(dropsVelo[index].Position));
                        effect.Parameters["View"].SetValue(camera.ViewMatrix);
                        effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                        Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                                Matrix.CreateScale(1.5f, 1.5f, 1.5f)
                                * Matrix.CreateTranslation(dropsVelo[index].Position)));
                        effect.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                        effect.Parameters["ModelTexture"].SetValue(texture);

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
