using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Volamus_v1
{
    class Skydome
    {
        Effect effect2;
        Vector3 viewVector;

        float diameter;
        bool transparent;
        Model skydome;

        float ro;

        public Skydome(float diameter, bool transp)
        {
            this.diameter = diameter;
            transparent = transp;
        }


        public void Initialize()
        {
        }


        public void Load()
        {
            effect2 = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTestWithTexture");

            if (transparent == false)
            {
                skydome = GameStateManager.Instance.Content.Load<Model>("Models/skydome");
            }
            else
            {
                skydome = GameStateManager.Instance.Content.Load<Model>("Models/skydomeTransp");
            }

            ro = 0;
        }

        public void Update(float rox)
        {
            ro += rox;
            if (ro == 360)
            {
                ro = 0;
            }
        }


        public void Draw(Camera camera, Texture2D texture)
        {
            Matrix[] transforms = new Matrix[skydome.Bones.Count];
            skydome.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in skydome.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(270)) * Matrix.CreateRotationZ(MathHelper.ToRadians(ro)) * Matrix.CreateScale(diameter, diameter, diameter)
                            * Matrix.CreateTranslation(new Vector3(0, 0, -500)));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateScale(0.05f, 0.025f, 0.025f)
                            * Matrix.CreateTranslation(new Vector3(0, 0, 0)));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);
                    effect2.Parameters["ModelTexture"].SetValue(texture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }
        }

    }
}