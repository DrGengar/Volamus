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
    class Spieler
    {
        Vector3 position;
        int height;
        Model player_model;

        public Spieler(Vector3 pos)
        {
            position = pos;
        }

        public void LoadContent(ContentManager content)
        {
            player_model = content.Load<Model>("3DAcaLogo");
        }

        public void Draw(Kamera camera, GraphicsDeviceManager graphics)
        {
            Matrix[] transforms = new Matrix[player_model.Bones.Count];
            player_model.CopyAbsoluteBoneTransformsTo(transforms);

            // Draw the model. A model can have multiple meshes, so loop.
            foreach (ModelMesh mesh in player_model.Meshes)
            {
                // This is where the mesh orientation is set, as well 
                // as our camera and projection.
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                        Matrix.CreateScale(0.2f, 0.2f, 0.2f)
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
