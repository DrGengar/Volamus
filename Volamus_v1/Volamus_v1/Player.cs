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
        int points;
        SpriteFont points_font;

        Camera camera;
        Model model;

        int direction; //1, if Position.Y < 0, -1 if Position.Y > 0
        bool is_jumping = false;
        bool is_falling = false;

        Vector2[] box = new Vector2[2];

        //Get-Methods
        public int Direction
        {
            get { return direction; }
        }

        public Camera Camera
        {
            get { return camera; }
        }

        public Vector3 Position
        {
            get { return position; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public SpriteFont Font
        {
            get { return points_font; }
        }

        public Player(Vector3 pos,int m_j_height, float j_velo,float mvp, Field field)
        {
            position = pos;

            if(position.Y < 0)
            {
                direction = 1;
                box[0] = new Vector2(-field.Width / 2, -field.Length / 2);
                box[1] = new Vector2(field.Width / 2, 0);
            }
            else
            {
                direction = -1;
                box[0] = new Vector2(field.Width / 2, field.Length / 2);
                box[1] = new Vector2(-field.Width / 2, 0);
            }

            camera = new Camera(new Vector3(0, direction*(-60), 20), new Vector3(0, 0, 0), new Vector3(0, direction*1, 1));

            max_jump_height = m_j_height;
            jump_velocity = j_velo;
            movespeed = mvp;

            points = 0;

            points_font = GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Standard");
        }

        public void LoadContent()
        {
            model = GameStateManager.Instance.Content.Load<Model>("3DAcaLogo");
        }

        public void Update(Field field, Keys Up, Keys Down, Keys Left, Keys Right, Keys Jump)
        {
            KeyboardState state = Keyboard.GetState();

            camera.Update();

            if (!is_jumping)
            {
                if (state.IsKeyDown(Up))
                {
                    if (direction == 1)
                    {
                        if (position.Y < box[1].Y)
                        {
                            position.Y += movespeed;
                        }
                    }
                    else if(direction == -1)
                    {
                        if (position.Y > box[1].Y)
                        {
                            position.Y -= movespeed;
                        }
                    } 
                }

                if (state.IsKeyDown(Left))
                {
                    if (direction == 1)
                    {
                        if (position.X > box[0].X)
                        {
                            position.X -= movespeed;
                            camera.AddPosition(new Vector3(-movespeed, 0, 0));
                            camera.AddView(new Vector3(-movespeed, 0, 0));
                        }
                    }
                    else if (direction == -1)
                    {
                        if (position.X < box[0].X)
                        {
                            position.X += movespeed;
                            camera.AddPosition(new Vector3(movespeed, 0, 0));
                            camera.AddView(new Vector3(movespeed, 0, 0));
                        }
                    }
                }

                if (state.IsKeyDown(Down))
                {
                    if (direction == 1)
                    {
                        if (position.Y > box[0].Y)
                        {
                            position.Y -= movespeed;
                        }
                    }
                    else if (direction == -1)
                    {
                        if (position.Y < box[0].Y)
                        {
                            position.Y += movespeed;
                        }
                    }
                }

                if (state.IsKeyDown(Right))
                {
                    if (direction == 1)
                    {
                        if (position.X < box[1].X)
                        {
                            position.X += movespeed;
                            camera.AddPosition(new Vector3(movespeed,0,0));
                            camera.AddView(new Vector3(movespeed, 0, 0));
                        }
                    }
                    else if (direction == -1)
                    {
                        if (position.X > box[1].X)
                        {
                            position.X -= movespeed;
                            camera.AddPosition(new Vector3(-movespeed, 0, 0));
                            camera.AddView(new Vector3(-movespeed, 0, 0));
                        }
                    }
                }

                if (state.IsKeyDown(Jump))
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

        public void Draw(Camera camera)
        {

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();

                    effect.World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                        Matrix.CreateScale(0.075f, 0.075f, 0.075f)
                        * Matrix.CreateTranslation(position);
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }
        }
    }
}
