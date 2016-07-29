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
        Vector3 position;
        Model model;
        BoundingSphere boundingSphere;

        private static Ball instance;

        bool isflying = false;

        Parabel active;

        DebugDraw d;

        Wind wind;

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
            set { isflying = value;  }
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

        public void LoadContent(Wind _wind)
        {
            model = GameStateManager.Instance.Content.Load<Model>("BeachBall");

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

            boundingSphere.Center = position;

            wind = _wind;
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

            if (isflying && active!=null)
            {
                //Position Updaten nach Flugbahn
                position = active.Flug(wind);
            }

            //BoundingSphere Position updaten auf aktuelle neue Position
            boundingSphere.Center = position;
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
                        Matrix.CreateScale(1.0f, 1.0f, 1.0f)
                        * Matrix.CreateTranslation(position);
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }
                mesh.Draw();
            }

            SpriteFont font = GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Standard");
            String text = Wind.Direction().ToString();

            GameStateManager.Instance.SpriteBatch.DrawString(font, text,
                new Vector2((GameStateManager.Instance.dimensions.X - font.MeasureString(text).X)/2, 0), Color.Black);

            /*d.Begin(camera.ViewMatrix, camera.ProjectionMatrix);
            d.DrawWireSphere(boundingSphere, Color.White);
            d.End();*/
        }
    }
}
