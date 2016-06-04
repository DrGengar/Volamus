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
    class Ball
    {
        Vector3 position;
        Model model;

        private float t = 0;
        bool isflying = false;
        private float x;
        private float y;
        private float z;
        private float v0;
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

        public Ball(Vector3 pos, float al, float ga, float be)
        {
            position = pos;
            alpha = al;
            betta = be;
            gamma = ga;
        }


        public void LoadContent()
        {
            model = GameStateManager.Instance.Content.Load<Model>("BeachBall");
        }


        public void Update(Player player)
        {
            KeyboardState state = Keyboard.GetState();
   

            // Wenn er nicht fliegt, ist er immer an der Position des Spielers
            if (!isflying)
            {
                position = player.Position + new Vector3(0, 0, 15);
                x = position.X;
                y = position.Y;
                z = position.Z;

                // Richtung rechts-links
                // man kann nicht nach hinten werfen
                if (state.IsKeyDown(Keys.Right) && gamma <= MathHelper.ToRadians(90))
                {
                    gamma += 0.01f;
                }
                if (state.IsKeyDown(Keys.Left) && gamma >= MathHelper.ToRadians(-90))
                {
                    gamma -= 0.01f;
                }
            }

            //Ball wird geworfen
            // Q leichter Wurf,  E starker Wurf
            if (!isflying && state.IsKeyDown(Keys.E))
            {
                v0 = 25f;
                isflying = true;
                Flugbahn(player);
            }

            if (!isflying && state.IsKeyDown(Keys.Q))
            {
                v0 = 20f;
                isflying = true;
                Flugbahn(player);
            }

            if (isflying)
            {
                if (position.Z < 0 && anzahlHuepfer <=2)
                {
                    anzahlHuepfer += 1;
                    x = position.X;
                    y = position.Y;
                    z = 0;
                    v0 -= 8;
                    t = 0;
                    Flugbahn(player);
                }
                if (anzahlHuepfer > 2)
                {
                    anzahlHuepfer = 0;
                    t = 0;
                    isflying = false;
                }
                else Flugbahn(player);
            }
        }

        /*  x: rechts/links;  y: vorne/hinten,   z: oben/unten
 *  v0: Abwurfgeschwindigkeit
 *  alpha: wie flach/steil   betta: wie weit in y Richtung    gamma: wie weit in x Richtung
 *  t: Zeit, bzw Darstellungsgeschwindigkeit ("Ballgeschwindigkeit" aber ohne Flugbahn zu verändern)
 *  g: Erdanziehungskraft 
 * 
 * */
        private void Flugbahn(Player player)
        {
            position.Z = z + v0 * (float)Math.Sin(alpha) * t - (g / 2) * t * t;
            position.Y = y + (player.Direction)* v0 * (float)Math.Cos(betta) * t;
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
