using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class Skydome
    {
        float diameter;
        Model skydome;

        public Skydome(float diameter)
        {
            this.diameter = diameter;
        }


        public void Initialize()
        {
        }


        public void Load()
        {
            skydome = GameStateManager.Instance.Content.Load<Model>("Models/skydome");
        }

        public void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[skydome.Bones.Count];
            skydome.CopyAbsoluteBoneTransformsTo(transforms);


            foreach (ModelMesh mesh in skydome.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                        Matrix.CreateScale(diameter, diameter, diameter)
                        * Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }

    }
}