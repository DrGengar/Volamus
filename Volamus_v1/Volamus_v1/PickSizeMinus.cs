﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class PickSizeMinus
    {
        Effect effect;
        List<Drop> drops;
        Model dr;
        Texture2D texture;

        public PickSizeMinus()
        {
            this.drops = new List<Drop>();
        }

        public void LoadContent()
        {
            dr = GameStateManager.Instance.Content.Load<Model>("Models/PUsizeM");
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shader");
            texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/blau");
        }

        public void Update(Random rnd)
        {
            int total = rnd.Next(2);

            while (drops.Count < total)
            {
                drops.Add(Generate(rnd));
            }

            for (int Drop = 0; Drop < drops.Count; Drop++)
            {
                drops[Drop].Update();

                if (drops[Drop].ttl <= 0)
                {
                    drops.RemoveAt(Drop);
                }

                if (Drop < drops.Count && (Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerOne, drops[Drop]) || Collision.Instance.PlayerWithDrop(GameScreen.Instance.Match.PlayerTwo, drops[Drop])))
                {
                    drops.RemoveAt(Drop);

                    if (Ball.Instance.BoundingSphereRadius > 1.5)
                    {
                        float temp = Ball.Instance.BoundingSphereRadius;

                        Ball.Instance.BoundingSphereRadius -= 0.25f;
                        Ball.Instance.EffectDrop = Ball.Instance.BoundingSphereRadius / Ball.Instance.OriginalRadius;

                    }
                }
            }

            if (GameScreen.Instance.Match.IsFinished)
            {
                drops.RemoveAll(item => item.ttl != 0);  //alle Drops die aktuell noch leben werden entfernt
            }
        }

        private Drop Generate(Random rnd)
        {
            float x = rnd.Next(-50, 50);
            float y = rnd.Next(-45, 46);
            int zufall = rnd.Next(1, 11);
            int timeToLive = 200 + rnd.Next(80);

            if(GameScreen.Instance.Match.WhichField == 2)
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

            for (int index = 0; index < drops.Count; index++)
            {
                Matrix[] transforms = new Matrix[dr.Bones.Count];
                dr.CopyAbsoluteBoneTransformsTo(transforms);

                foreach (ModelMesh mesh in dr.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = effect;

                        Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                                       Matrix.CreateScale(1.50f, 1.50f, 2.0f) * Matrix.CreateTranslation(drops[index].Position);
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
