using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class Player
    {
        Vector3 position;
        int max_jump_height;
        float jump_velocity;
        float movespeed;
        Camera camera;
        int direction; //1, if Position.Y < 0, -1 if Position.Y > 0

        Model player_model;
        bool is_jumping = false;
        bool is_falling = false;

        //Get-Methods
        public int Direction
        {
            get { return direction; }
        }

        public Player(Vector3 pos,int m_j_height, float j_velo,float mvp)
        {
            position = pos;

            if(position.Y < 0)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            max_jump_height = m_j_height;
            jump_velocity = j_velo;
            movespeed = mvp;
        }

        public void LoadContent(ContentManager content)
        {
            player_model = content.Load<Model>("3DAcaLogo");
        }

        public void Update(Field field)
        {
            KeyboardState state = Keyboard.GetState();

            if (!is_jumping)
            {
                if (state.IsKeyDown(Keys.W))
                {
                    if (position.Y < -1)
                    {
                        position.Y += movespeed;
                    }
                }

                if (state.IsKeyDown(Keys.A))
                {
                    if (!(position.X < (-field.get_width() / 2)))
                    {
                        position.X -= movespeed;
                    }
                }

                if (state.IsKeyDown(Keys.S))
                {
                    if (!(position.Y < (-field.get_length() / 2)))
                    {
                        position.Y -= movespeed;
                    }
                }

                if (state.IsKeyDown(Keys.D))
                {
                    if (!(position.X > (field.get_width() / 2)))
                    {
                        position.X += movespeed;
                    }
                }

                if (state.IsKeyDown(Keys.Space))
                {
                    is_jumping = true;
                    is_falling = false;
                }
            }
            else
            {
                if (position.Z <= max_jump_height && !is_falling)
                {
                    position.Z += jump_velocity;
                }
                else
                {
                    if (position.Z > 0)
                    {
                        position.Z -= jump_velocity;
                        is_falling = true;
                    }
                    else
                    {
                        is_jumping = false;
                    }
                }

            }
        }

        public void Draw(Camera camera, GraphicsDeviceManager graphics)
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
                        Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                        * Matrix.CreateTranslation(position);
                    effect.View = camera.get_View();
                    effect.Projection = camera.get_Projection();
                }
                // Draw the mesh, using the effects set above.
                mesh.Draw();
            }
        }

        public Vector3 get_position()
        {
            return position;
        }
    }
}
