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
    public static class VertexElementExtractor
    {
        public static Vector3[] GetVertexElement(ModelMeshPart meshPart, VertexElementUsage usage)
        {
            VertexDeclaration vd = meshPart.VertexBuffer.VertexDeclaration;
            VertexElement[] elements = vd.GetVertexElements();

            Func<VertexElement, bool> elementPredicate = ve => ve.VertexElementUsage == usage && ve.VertexElementFormat == VertexElementFormat.Vector3;
            if (!elements.Any(elementPredicate))
                return null;

            VertexElement element = elements.First(elementPredicate);

            Vector3[] vertexData = new Vector3[meshPart.NumVertices];
            meshPart.VertexBuffer.GetData((meshPart.VertexOffset * vd.VertexStride) + element.Offset,
                vertexData, 0, vertexData.Length, vd.VertexStride);

            return vertexData;
        }
    }

    public class BoundingBoxBuffers
    {
        public VertexBuffer Vertices;
        public int VertexCount;
        public IndexBuffer Indices;
        public int PrimitiveCount;
    }

    public class Player
    {
        Vector3 position;
        int max_jump_height;
        float jump_velocity;
        float movespeed;
        float gamma;
        int points;

        SpriteFont points_font;
        BoundingBox innerBoundingBox, outerBoundingBox;
        Vector3 scale;

        Camera camera;
        Model model;

        int direction; //1, if Position.Y < 0, -1 if Position.Y > 0
        bool is_jumping = false;
        bool is_falling = false;
        bool can_hit = false;
        bool is_serving = false;

        Player enemy;

        Vector2[] box = new Vector2[2];

        DebugDraw d;

        Texture2D circle_angle, arrow;

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

        public Player Enemy
        {
            get { return enemy; }
            set
            {
                enemy = value;
            }
        }

        public SpriteFont Font
        {
            get { return points_font; }
        }

        public Vector2[] Box
        {
            get { return box; }
        }

        public BoundingBox InnerBoundingBox
        {
            get { return innerBoundingBox; }
        }

        public BoundingBox OuterBoundingBox
        {
            get { return outerBoundingBox; }
        }

        public Model Model
        {
            get { return model; }
        }

        public Texture2D Circle
        {
            get { return circle_angle; }
        }

        public Texture2D Arrow
        {
            get { return arrow; }
        }

        public float Gamma
        {
            get { return MathHelper.ToRadians(gamma); }
        }

        public bool CanHit
        {
            get { return can_hit; }
            set { can_hit = value; }
        }

        public bool IsServing
        {
            get { return is_serving; }
            set { is_serving = value; }
        }

        public Player(Vector3 pos, int m_j_height, float j_velo, float mvp, Field field)
        {
            position = pos;

            if (position.Y < 0)
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

            camera = new Camera(new Vector3(0, direction * (-60), 20), new Vector3(0, 0, 0), new Vector3(0, direction * 1, 1));
            //camera = new Camera(new Vector3(0, 0, 50), position, new Vector3(0, direction, 0));

            max_jump_height = m_j_height;
            jump_velocity = j_velo;
            movespeed = mvp;

            points = 0;
            gamma = 0.0f;

            scale = new Vector3(0.075f, 0.075f, 0.075f);

            d = new DebugDraw(GameStateManager.Instance.GraphicsDevice);
        }

        public void LoadContent()
        {
            model = GameStateManager.Instance.Content.Load<Model>("3DAcaLogo");

            points_font = GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Standard");

            circle_angle = GameStateManager.Instance.Content.Load<Texture2D>("Images/winkel");
            arrow = GameStateManager.Instance.Content.Load<Texture2D>("Images/pfeil");

            CreateBoundingBoxes();
        }

        public void Update(Field field, Keys Up, Keys Down, Keys Left, Keys Right, Keys Jump, Keys weak, Keys strong, Keys l, Keys r)
        {
            KeyboardState state = Keyboard.GetState();

            camera.Update();

            //every player needs a gamma
            if (direction == 1)
            {
                if (state.IsKeyDown(r) && gamma <= 90)
                {
                    gamma += (direction) * 1.0f; //Ein Grad mehr/weniger
                }
                if (state.IsKeyDown(l) && gamma >= -90)
                {
                    gamma -= (direction) * 1.0f; //Ein Grad mehr/weniger
                }
            }
            else
            {
                if(direction ==-1)
                {
                    if (state.IsKeyDown(r) && gamma >= -90)
                    {
                        gamma -= 1.0f; //Ein Grad mehr/weniger
                    }
                    if (state.IsKeyDown(l) && gamma <= 90)
                    {
                        gamma += 1.0f; //Ein Grad mehr/weniger
                    }
                }
            }

            if (is_serving)
            {
                WeakThrow(weak);
                StrongThrow(strong);

                float offset = position.Y - box[0].Y;
                position.Y -= offset;
                MovingBoundingBoxes(new Vector3(0, -offset, 0));

                if (!is_jumping)
                {
                    if (state.IsKeyDown(Left))
                    {
                        if (direction == 1)
                        {
                            if (position.X > box[0].X) //position.X > box[0].X
                            {
                                position.X -= movespeed;
                                MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));
                                camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                camera.AddView(new Vector3(-movespeed, 0, 0));
                            }
                        }
                        else if (direction == -1)
                        {
                            if (position.X < box[0].X) //position.X < box[0].X
                            {
                                position.X += movespeed;
                                MovingBoundingBoxes(new Vector3(movespeed, 0, 0));
                                camera.AddPosition(new Vector3(movespeed, 0, 0));
                                camera.AddView(new Vector3(movespeed, 0, 0));
                            }
                        }
                    }

                    if (state.IsKeyDown(Right))
                    {
                        if (direction == 1)
                        {
                            if (position.X < box[1].X) //position.X < box[1].X
                            {
                                position.X += movespeed;
                                MovingBoundingBoxes(new Vector3(movespeed, 0, 0));
                                camera.AddPosition(new Vector3(movespeed, 0, 0));
                                camera.AddView(new Vector3(movespeed, 0, 0));
                            }
                        }
                        else if (direction == -1)
                        {
                            if (position.X > box[1].X) //position.X > box[1].X
                            {
                                position.X -= movespeed;
                                MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));
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
                        MovingBoundingBoxes(new Vector3(0, 0, jump_velocity));
                    }
                    else
                    {
                        if (position.Z > 0)
                        {
                            position.Z -= jump_velocity;
                            MovingBoundingBoxes(new Vector3(0, 0, -jump_velocity));
                            is_falling = true;
                        }
                        else
                        {
                            is_jumping = false;
                        }
                    }
                }

                if (direction == 1)
                {
                    Ball.Instance.Position = new Vector3((OuterBoundingBox.Max.X + OuterBoundingBox.Min.X) / 2, OuterBoundingBox.Max.Y, OuterBoundingBox.Max.Z);
                }
                else
                {
                    if (direction == -1)
                    {
                        Ball.Instance.Position = new Vector3((OuterBoundingBox.Max.X + OuterBoundingBox.Min.X) / 2, OuterBoundingBox.Min.Y, OuterBoundingBox.Max.Z);
                    }
                }
            }
            else
            {
                WeakThrow(weak);
                StrongThrow(strong);

                if (!is_jumping)
                {
                    if (state.IsKeyDown(Up))
                    {
                        if (direction == 1)
                        {
                            if (!Collision.Instance.PlayerWithNet(this, field)) //Collision Spieler mit Netz
                            {
                                position.Y += movespeed;

                                MovingBoundingBoxes(new Vector3(0, movespeed, 0));
                            }
                        }
                        else if (direction == -1)
                        {
                            if (!Collision.Instance.PlayerWithNet(this, field)) //Collision Spieler mit Netz
                            {
                                position.Y -= movespeed;
                                MovingBoundingBoxes(new Vector3(0, -movespeed, 0));
                            }
                        }
                    }

                    if (state.IsKeyDown(Left))
                    {
                        if (direction == 1)
                        {
                            if (position.X > box[0].X) //position.X > box[0].X
                            {
                                position.X -= movespeed;
                                MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));
                                camera.AddPosition(new Vector3(-movespeed, 0, 0));
                                camera.AddView(new Vector3(-movespeed, 0, 0));
                            }
                        }
                        else if (direction == -1)
                        {
                            if (position.X < box[0].X) //position.X < box[0].X
                            {
                                position.X += movespeed;
                                MovingBoundingBoxes(new Vector3(movespeed, 0, 0));
                                camera.AddPosition(new Vector3(movespeed, 0, 0));
                                camera.AddView(new Vector3(movespeed, 0, 0));
                            }
                        }
                    }

                    if (state.IsKeyDown(Down))
                    {
                        if (direction == 1)
                        {
                            if (position.Y > box[0].Y) //position.Y > box[0].Y
                            {
                                position.Y -= movespeed;
                                MovingBoundingBoxes(new Vector3(0, -movespeed, 0));
                            }
                        }
                        else if (direction == -1)
                        {
                            if (position.Y < box[0].Y) //position.Y < box[0].Y
                            {
                                position.Y += movespeed;
                                MovingBoundingBoxes(new Vector3(0, movespeed, 0));
                            }
                        }
                    }

                    if (state.IsKeyDown(Right))
                    {
                        if (direction == 1)
                        {
                            if (position.X < box[1].X) //position.X < box[1].X
                            {
                                position.X += movespeed;
                                MovingBoundingBoxes(new Vector3(movespeed, 0, 0));
                                camera.AddPosition(new Vector3(movespeed, 0, 0));
                                camera.AddView(new Vector3(movespeed, 0, 0));
                            }
                        }
                        else if (direction == -1)
                        {
                            if (position.X > box[1].X) //position.X > box[1].X
                            {
                                position.X -= movespeed;
                                MovingBoundingBoxes(new Vector3(-movespeed, 0, 0));
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
                        MovingBoundingBoxes(new Vector3(0, 0, jump_velocity));
                    }
                    else
                    {
                        if (position.Z > 0)
                        {
                            position.Z -= jump_velocity;
                            MovingBoundingBoxes(new Vector3(0, 0, -jump_velocity));
                            is_falling = true;
                        }
                        else
                        {
                            is_jumping = false;
                        }
                    }
                }
            }
        }

        public void WeakThrow(Keys weakthrow)
        {
            if (Keyboard.GetState().IsKeyDown(weakthrow) && can_hit)
            {
                if (is_serving)
                {
                    is_serving = false;
                }

                Collision.Instance.LastTouched = this;
                Ball.Instance.IsFlying = true;
                Parabel weak = new Parabel(Ball.Instance.Position,45.0f,0.0f, 45.0f, 20.0f, direction);
                Ball.Instance.Active = weak;
            }
        }

        public void StrongThrow(Keys strongthrow)
        {
            if (Keyboard.GetState().IsKeyDown(strongthrow) && can_hit)
            {
                if (is_serving)
                {
                    is_serving = false;
                }

                Collision.Instance.LastTouched = this;
                Ball.Instance.IsFlying = true;
                Parabel strong = new Parabel(Ball.Instance.Position, 45.0f, gamma, 45.0f, 25.0f, direction);
                Ball.Instance.Active = strong;
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
                        Matrix.CreateScale(scale)
                        * Matrix.CreateTranslation(position);
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }

            d.Begin(camera.ViewMatrix,camera.ProjectionMatrix);
            d.DrawWireBox(innerBoundingBox, Color.White);
            d.DrawWireBox(outerBoundingBox, Color.Black);
            d.End();
        }

        private void CreateBoundingBoxes()
        {

            //Innere BoundingBox

            // Initialize minimum and maximum corners of the bounding box to max and min values
            Vector3 min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            // For each mesh of the model
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    // Vertex buffer parameters
                    int vertexStride = meshPart.VertexBuffer.VertexDeclaration.VertexStride;
                    int vertexBufferSize = meshPart.NumVertices * vertexStride;

                    // Get vertex data as float
                    float[] vertexData = new float[vertexBufferSize / sizeof(float)];
                    meshPart.VertexBuffer.GetData<float>(vertexData);

                    // Iterate through vertices (possibly) growing bounding box, all calculations are done in world space
                    for (int i = 0; i < vertexBufferSize / sizeof(float); i += vertexStride / sizeof(float))
                    {
                        Vector3 transformedPosition = Vector3.Transform(new Vector3(vertexData[i], vertexData[i + 1], vertexData[i + 2]), Matrix.CreateScale(scale.X));

                        min = Vector3.Min(min, transformedPosition);
                        max = Vector3.Max(max, transformedPosition);
                    }
                }
            }

            // Create and return bounding box

            Vector3 mid = new Vector3((max.X + min.X)/2, (direction) * (max.Y + min.Y)/2, min.Z);
            Vector3 translate = mid - position;

            min.X -= translate.X;
            max.X -= translate.X;

            min.Y += position.Y;
            max.Y += position.Y;

            min.Z -= translate.Z;
            max.Z -= translate.Z;

            innerBoundingBox = new BoundingBox(min, max);

            //äußere BoundingBox
            Vector3 offset = new Vector3(Ball.Instance.BoundingSphere.Radius, Ball.Instance.BoundingSphere.Radius, 0);
            outerBoundingBox = new BoundingBox((innerBoundingBox.Min - offset),
                innerBoundingBox.Max + offset);
        }

        private void MovingBoundingBoxes(Vector3 offset)
        {
            innerBoundingBox.Min += offset;
            innerBoundingBox.Max += offset;

            outerBoundingBox.Min += offset;
            outerBoundingBox.Max += offset;
        }
    }
}
