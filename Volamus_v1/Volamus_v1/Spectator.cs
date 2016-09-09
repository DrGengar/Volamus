using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Spectator
    {
        Model model, leftWing, rightWing;
        Texture texture;
        Effect effect, effect2;
        Vector3 position, leftWingPosition, rightWingPosition, scale;
        int hitAngleHigh;
        bool is_falling;

        public Spectator(Vector3 pos, Vector3 s)
        {
            position = pos;
            scale = s;
        }

        public void LoadContent()
        {
            model = GameStateManager.Instance.Content.Load<Model>("Models/pinguin");
            leftWing = GameStateManager.Instance.Content.Load<Model>("Models/leftneu2");
            rightWing = GameStateManager.Instance.Content.Load<Model>("Models/rightneu2");

            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTest");
            effect2 = GameStateManager.Instance.Content.Load<Effect>("Effects/shaderTestWithTexture");

            texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/pinguinUV");
        }

        public void UnloadContent()
        {
            
        }

        public void Update(GameTime gameTime)
        {
            //Flügel gehen nach oben
            leftWingPosition = new Vector3(position.X - 2, position.Y, position.Z - 1);
            rightWingPosition = new Vector3(position.X + 2, position.Y, position.Z - 1);
            hitAngleHigh = 20;

            //hüpft
            if (position.Z < 7 && !is_falling)
            {
                position.Z += 0.2f;
            }
            else
            {
                if (position.Z > 0)
                {
                    position.Z -= 0.2f ;
                    is_falling = true;
                }
                else
                {
                    is_falling = false;
                }
            }
        }

        public void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)) *
                          Matrix.CreateScale(scale)
                          * Matrix.CreateTranslation(position));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)) *
                          Matrix.CreateScale(scale)
                          * Matrix.CreateTranslation(position)));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    effect2.Parameters["ModelTexture"].SetValue(texture);

                    Vector3 viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);


                }
                mesh.Draw();

                DrawWingLeft(camera, leftWing, leftWingPosition);
                DrawWingRight(camera, rightWing, rightWingPosition);
            }
        }

        public void DrawWingRight(Camera camera, Model wing, Vector3 position)
        {
            Matrix[] transforms = new Matrix[wing.Bones.Count];
            wing.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in wing.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90 - hitAngleHigh)) *
                          Matrix.CreateScale(scale * 5) //scale *4
                          * Matrix.CreateTranslation(position));
                    effect.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)) *
                          Matrix.CreateScale(scale)
                          * Matrix.CreateTranslation(position)));
                    effect.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);
                    //effect.Parameters["ModelTexture"].SetValue(wingTexture);

                    Vector3 viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }
        }

        // für linken Flügel
        public void DrawWingLeft(Camera camera, Model wing, Vector3 position)
        {
            Matrix[] transforms = new Matrix[wing.Bones.Count];
            wing.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in wing.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90 - hitAngleHigh)) *
                          Matrix.CreateScale(scale * 5) //scale *4
                          * Matrix.CreateTranslation(position));
                    effect.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateRotationZ(MathHelper.ToRadians(90)) *
                          Matrix.CreateScale(scale)
                          * Matrix.CreateTranslation(position)));
                    effect.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);
                    //effect.Parameters["ModelTexture"].SetValue(wingTexture);

                    Vector3 viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }
        }
    }
}
