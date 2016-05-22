using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class Ball
    {
        Vector3 position;
        //Rest

        Model ball_model;

        public Ball(Vector3 pos)
        {
            position = pos;
            //Rest
        }

        public void LoadContent(ContentManager content)
        {
            ball_model = content.Load<Model>("ball");
        }

        public void Update(Spieler player_one)
        {
            position = player_one.get_position() + new Vector3(0, 0, 15);
        }

        public void Draw(Kamera camera, GraphicsDeviceManager graphics)
        {
            Matrix[] transforms = new Matrix[ball_model.Bones.Count];
            ball_model.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in ball_model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                        Matrix.CreateScale(0.03f, 0.03f, 0.03f)
                        * Matrix.CreateTranslation(position);
                    effect.View = camera.get_View();
                    effect.Projection = camera.get_Projection();
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }
    }
}
