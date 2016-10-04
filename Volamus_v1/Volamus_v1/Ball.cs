using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Volamus_v1
{
    public class Ball
    {
        float effectDrop = 1f;
        float rotate = 0f;
        Effect effect;
        Vector3 viewVector;

        Vector3 position;
        Model model;
        BoundingSphere boundingSphere;
        Texture2D ballShadow;
        Texture2D ballTexture;
        float originalRadius;

        VertexPositionTexture[] shadowVertices = new VertexPositionTexture[6];
        BasicEffect e;

        private static Ball instance;

        bool isflying = false;

        Parabel active;

        DebugDraw d;

        Wind wind;

        public float EffectDrop
        {
            get { return effectDrop; }
            set { effectDrop = value; }
        }
        public float OriginalRadius
        {
            get { return originalRadius; }

        }

        public Vector3 Position
        {
            get { return position; }

            set
            {
                position = value;
                boundingSphere.Center = value;
            }
        }

        public Model Model
        {
            get { return model; }
        }

        public bool IsFlying
        {
            get { return isflying; }
            set { isflying = value; }
        }

        public Parabel Active
        {
            get { return active; }
            set { active = value; }
        }

        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }
        public float BoundingSphereRadius
        {
            get { return BoundingSphere.Radius; }
            set { boundingSphere.Radius = value; }
        }

        public Wind Wind
        {
            get { return wind; }
        }

        public static Ball Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Ball(new Vector3(0, -10, 15));
                }

                return instance;
            }
        }

        private Ball(Vector3 pos)
        {
            position = pos;
            d = new DebugDraw(GameStateManager.Instance.GraphicsDevice);
        }

        public void Initialize()
        {
            float r = boundingSphere.Radius;
            shadowVertices[0].Position = new Vector3(position.X - r, position.Y - r, 0.02f);
            shadowVertices[1].Position = new Vector3(position.X - r, position.Y + r, 0.02f);
            shadowVertices[2].Position = new Vector3(position.X + r, position.Y - r, 0.02f);

            shadowVertices[3].Position = shadowVertices[1].Position;
            shadowVertices[4].Position = new Vector3(position.X + r, position.Y + r, 0.02f);
            shadowVertices[5].Position = shadowVertices[2].Position;

            shadowVertices[0].TextureCoordinate = new Vector2(0, 0);
            shadowVertices[1].TextureCoordinate = new Vector2(0, 1);
            shadowVertices[2].TextureCoordinate = new Vector2(1, 0);

            shadowVertices[3].TextureCoordinate = shadowVertices[1].TextureCoordinate;
            shadowVertices[4].TextureCoordinate = new Vector2(1, 1);
            shadowVertices[5].TextureCoordinate = shadowVertices[2].TextureCoordinate;


            e = new BasicEffect(GameStateManager.Instance.GraphicsDevice);
        }

        public void LoadContent(Wind _wind)
        {
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shader");

            model = GameStateManager.Instance.Content.Load<Model>("Models/BeachBall");
            ballTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/BeachBallTexture");
            ballShadow = GameStateManager.Instance.Content.Load<Texture2D>("Images/BallShadow");

            //BoundingSphere erstellen
            boundingSphere = new BoundingSphere();

            foreach (ModelMesh mesh in model.Meshes)
            {
                if (boundingSphere.Radius == 0)
                    boundingSphere = mesh.BoundingSphere;
                else
                    boundingSphere = BoundingSphere.CreateMerged(boundingSphere, mesh.BoundingSphere);
            }

            boundingSphere.Radius *= 1.0f;
            originalRadius = boundingSphere.Radius;

            boundingSphere.Center = position;

            wind = _wind;

            Initialize();
        }

        public void UnloadContent()
        {
            instance = null;
        }

        public void Update()
        {
   
            
            if (!isflying)
            {
                //Setze t auf 0 zurück
                if (active != null)
                {
                    active.Reset_t();
                }
            }

            if (isflying && active != null)
            {
                
                rotate += 0.05f;
                //Position Updaten nach Flugbahn
                position = active.Flug(wind);
            }

            //BoundingSphere Position updaten auf aktuelle neue Position
            boundingSphere.Center = position;

            float r = boundingSphere.Radius;
            shadowVertices[0].Position = new Vector3(position.X - r, position.Y - r, 0.01f);
            shadowVertices[1].Position = new Vector3(position.X - r, position.Y + r, 0.01f);
            shadowVertices[2].Position = new Vector3(position.X + r, position.Y - r, 0.01f);

            shadowVertices[3].Position = shadowVertices[1].Position;
            shadowVertices[4].Position = new Vector3(position.X + r, position.Y + r, 0.01f);
            shadowVertices[5].Position = shadowVertices[2].Position;


            boundingSphere.Radius = BoundingSphereRadius;
        }

        public void Draw(Camera camera)
        {
            e.View = camera.ViewMatrix;
            e.Projection = camera.ProjectionMatrix;

            e.TextureEnabled = true;
            e.Texture = ballShadow;

            foreach (var pass in e.CurrentTechnique.Passes)
            {
                pass.Apply();

                GameStateManager.Instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, shadowVertices, 0, 2);
            }

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;

                    Matrix World = transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90 * rotate * Collision.Instance.LastTouched.Direction)) *
                                   Matrix.CreateScale(1.0f * effectDrop, 1.0f * effectDrop, 1.0f * effectDrop)
                                   * Matrix.CreateTranslation(position);
                    Matrix Projection = camera.ProjectionMatrix;
                    Matrix View = camera.ViewMatrix;
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(World));

                    effect.Parameters["worldMatrix"].SetValue(World);
                    effect.Parameters["worldInverseTransposeMatrix"].SetValue(WorldInverseTransposeMatrix);
                    effect.Parameters["worldViewProjectionMatrix"].SetValue(World * View * Projection);

                    effect.Parameters["cameraPos"].SetValue(camera.Position);
                    effect.Parameters["globalAmbient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["numLights"].SetValue(4);

                    effect.Parameters["PointLightpos"].SetValue(GameScreen.Instance.Match.LightsPosition);
                    effect.Parameters["PointLightambient"].SetValue(GameScreen.Instance.Match.LightsAmbient);
                    effect.Parameters["PointLightdiffuse"].SetValue(GameScreen.Instance.Match.LightsDiffuse);
                    effect.Parameters["PointLightspecular"].SetValue(GameScreen.Instance.Match.LightsSpecular);
                    effect.Parameters["PointLightradius"].SetValue(GameScreen.Instance.Match.LightsRadius);

                    effect.Parameters["Materialambient"].SetValue(new Vector4(0.2f, 0.2f, 0.2f, 1.0f));
                    effect.Parameters["Materialdiffuse"].SetValue(new Vector4(0.8f, 0.8f, 0.8f, 1.0f));
                    effect.Parameters["Materialspecular"].SetValue(new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
                    effect.Parameters["Materialshininess"].SetValue(32.0f);

                    effect.Parameters["colorMapTexture"].SetValue(ballTexture);
                    /*
                    part.Effect = effect2;
                    effect2.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90*rotate*Collision.Instance.LastTouched.Direction)) * 
                            Matrix.CreateScale(1.0f * effectDrop, 1.0f * effectDrop, 1.0f * effectDrop)
                            * Matrix.CreateTranslation(position));
                    effect2.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect2.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90 * rotate * Collision.Instance.LastTouched.Direction)) *
                            Matrix.CreateScale(1.0f, 1.0f, 1.0f)
                            * Matrix.CreateTranslation(position)));
                    effect2.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    effect2.Parameters["ModelTexture"].SetValue(ballTexture);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect2.Parameters["ViewVector"].SetValue(viewVector);
                    */
                }
                mesh.Draw();
            }
        }
    }
}
