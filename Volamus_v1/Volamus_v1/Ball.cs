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
        Collision collision;

        private static Ball instance;

        private float t = 0;
        bool isflying = false;
        private float x;
        private float y;
        private float z;
        private float v0;
        private int dir;
        float alpha;
        float gamma;
        float betta;
        private float g=9.81f;
        private int anzahlHuepfer = 0;

        public Vector3 Position
        {
            get { return position; }
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

        public float Gamma
        {
            get { return gamma; }
            set { gamma = value; }
        }

        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }

        public static Ball Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Ball(new Vector3(0, -10, 15), MathHelper.ToRadians(45), MathHelper.ToRadians(0), MathHelper.ToRadians(45));
                }

                return instance;
            }
        }

        private Ball(Vector3 pos, float al, float ga, float be)
        {
            gamma = 0;
            collision = new Collision();
            position = pos;
            alpha = al;
            betta = be;
            gamma = ga;
        }


        public void LoadContent()
        {
            model = GameStateManager.Instance.Content.Load<Model>("BeachBall");


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


        }


        public void Update(Player player,Field field)
        {
            KeyboardState state = Keyboard.GetState();

            boundingSphere.Center = position;

            if (!isflying)
            {
                /*position = player.Position + new Vector3(0, 0, 15);
                x = position.X;
                y = position.Y;
                z = position.Z;*/
            }

            if (isflying)
            {
                if (collision.CollisionMethod() && anzahlHuepfer <=2)
                {
                    anzahlHuepfer += 1;
                    x = position.X;
                    y = position.Y;
                    z = boundingSphere.Radius;
                    v0 -= 4;
                    t = 0;
                    Flugbahn(v0,dir);
                }

                if (anzahlHuepfer > 2)
                {
                    anzahlHuepfer = 0;
                    t = 0;
                    isflying = false;
                }
                else
                Flugbahn(v0,dir);
            }

        }

        /*  x: rechts/links;  y: vorne/hinten,   z: oben/unten
 *  v0: Abwurfgeschwindigkeit
 *  alpha: wie flach/steil   betta: wie weit in y Richtung    gamma: wie weit in x Richtung
 *  t: Zeit, bzw Darstellungsgeschwindigkeit ("Ballgeschwindigkeit" aber ohne Flugbahn zu verändern)
 *  g: Erdanziehungskraft 
 * 
 * */
        public void Flugbahn(float velo,int direction)
        {
            v0 = velo;
            dir = direction;
            position.Z = z + v0 * (float)Math.Sin(alpha) * t - (g / 2) * t * t;
            position.Y = y + (direction)* v0 * (float)Math.Cos(betta) * t;
            position.X = x + v0 * (float)Math.Sin(gamma) * t;
            t = t + 0.05f;

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
        }
    }
}
