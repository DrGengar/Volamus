﻿using Microsoft.Xna.Framework;
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
        Model ice;
        Texture2D iceTexture;

        Vector3 viewVector;

        public WaterField(int w, int l, int n_h) : base(w, l, n_h){}

        public new void LoadContent()
        {
            skydome = new Skydome(25f, false, GameStateManager.Instance.Content.Load<Texture2D>("Textures/skydome"));
            skydome.Load();

            ice = GameStateManager.Instance.Content.Load<Model>("Models/eisscholle");
            netTexture = iceTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/iceTexture");

            base.LoadContent();
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
                    effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.2f, 0.3f, 0.01f)
                           * Matrix.CreateTranslation(new Vector3(0, 0, -0.75f)));
                    effect.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.2f, 0.3f, 0.01f)
                           * Matrix.CreateTranslation(new Vector3(0, 0, -0.75f))));
                    effect.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["ModelTexture"].SetValue(iceTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }

            base.Draw(camera);
        }
    }
}