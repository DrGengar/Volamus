using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class MeadowField : Field
    {
        Grass grass;

        SpectatorGroupBumbleBee groupOne, groupTwo;

        VertexPositionTexture[] vertices;

        ConfettiFlower confetti;

        public ConfettiFlower Confetti
        {
            get { return confetti; }
            set { confetti = value; }
        }

        public SpectatorGroupBumbleBee GroupOne
        {
            get { return groupOne; }
        }

        public SpectatorGroupBumbleBee GroupTwo
        {
            get { return groupTwo; }
        }

        public MeadowField(int w, int l, int n_h, Random rnd) : base(w, l, n_h)
        {
            groupOne = new SpectatorGroupBumbleBee(new Vector3(-w / 2 - 10, -l / 2 - 5, 5), 5, rnd);
            groupTwo = new SpectatorGroupBumbleBee(new Vector3(w / 2 + 10, l / 2 + 5, 5), 5, rnd);
            grass = new Grass(new Vector3(-1000, -1000, 0), new Vector3(1000, 1000, 0));

            vertices = new VertexPositionTexture[6];
            vertices[0].Position = new Vector3(-1000, -1000, -0.01f);
            vertices[1].Position = new Vector3(-1000, 1000, -0.01f);
            vertices[2].Position = new Vector3(1000, -1000, -0.01f);
            vertices[3].Position = vertices[1].Position;
            vertices[4].Position = new Vector3(1000, 1000, -0.01f);
            vertices[5].Position = vertices[2].Position;

            vertices[0].TextureCoordinate = new Vector2(0, 0);
            vertices[1].TextureCoordinate = new Vector2(0, 1);
            vertices[2].TextureCoordinate = new Vector2(1, 0);

            vertices[3].TextureCoordinate = vertices[1].TextureCoordinate;
            vertices[4].TextureCoordinate = new Vector2(1, 1);
            vertices[5].TextureCoordinate = vertices[2].TextureCoordinate;
        }

        public new void LoadContent()
        {
            groupOne.LoadContent();
            groupTwo.LoadContent();

            skydome = new Skydome(25f, false, GameStateManager.Instance.Content.Load<Texture2D>("Textures/wolken"));
            skydome.Load();

            netTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/iceTexture");

            grass.LoadContent();

            base.LoadContent();
        }

        public void UnloadContent()
        {
            groupOne.UnloadContent();
            groupTwo.UnloadContent();
        }

        public new void Update(Random rnd)
        {
            groupOne.Update();
            groupTwo.Update();

            if(confetti != null)
            {
                confetti.Update(rnd);
            }
        }

        public new void Draw(Camera camera)
        {
            base.Draw(camera);

            grass.Draw(camera);

            if (confetti != null)
            {
                confetti.Draw(camera);
            }

            groupOne.Draw(camera);
            groupTwo.Draw(camera);
        }

        public void DrawField(Camera camera)
        {
            BasicEffect e = new BasicEffect(GameStateManager.Instance.GraphicsDevice);
            e.View = camera.ViewMatrix;
            e.Projection = camera.ProjectionMatrix;

            e.TextureEnabled = true;
            e.Texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/grass");

            foreach (var pass in e.CurrentTechnique.Passes)
            {
                pass.Apply();

                GameStateManager.Instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, 2);
            }
        }
    }
}
