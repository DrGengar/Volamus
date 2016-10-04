using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Grass
    {
        VertexPositionNormalTexture[] vertices;
        Texture2D grassTexture;
        Effect grassEffect;
        int nrOfLayers = 30;
        float maxGrassLength = 2.0f;
        float density = 0.25f;
        Texture2D grassColorTexture;
        Vector3 gravity = new Vector3(0, 0, 1.0f);
        Vector3 forceDirection = Vector3.Zero;
        Vector3 displacement;
        SpriteBatch spriteBatch;
        Vector3 PositionOne, PositionTwo;

        public Grass(Vector3 posOne, Vector3 posTwo)
        {
            PositionOne = posOne;
            PositionTwo = posTwo;
        }

        public void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GameStateManager.Instance.GraphicsDevice);
            //generate the geometry
            GenerateGeometry();
            //load the effect
            grassEffect = GameStateManager.Instance.Content.Load<Effect>("Effects/GrassEffect");
            //create the texture
            int temp = 216 * (int)(PositionTwo.X / 50 + 0.5f);
            int temp2 = 216 * (int)(PositionTwo.Y / 50 + 0.5f);
            grassTexture = new Texture2D(GameStateManager.Instance.GraphicsDevice, temp, temp2);
            //fill the texture
            FillGrassTexture(grassTexture, density);
            grassColorTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/grass");
        }

        public void Draw(Camera camera)
        {
            forceDirection.X = (float)Math.Sin(GameStateManager.Instance.GameTime.TotalGameTime.TotalSeconds) * 0.5f;
            displacement = gravity + forceDirection;
            grassEffect.Parameters["Displacement"].SetValue(displacement);

            grassEffect.Parameters["World"].SetValue(Matrix.CreateTranslation(0, 0, -0.05f));
            grassEffect.Parameters["View"].SetValue(camera.ViewMatrix);
            grassEffect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
            grassEffect.Parameters["MaxHairLength"].SetValue(maxGrassLength);
            grassEffect.Parameters["FurTexture"].SetValue(grassTexture);
            grassEffect.Parameters["Texture"].SetValue(grassColorTexture);

            for (int i = 0; i < nrOfLayers; i++)
            {
                grassEffect.Parameters["CurrentLayer"].SetValue((float)i / nrOfLayers);
                grassEffect.CurrentTechnique.Passes[0].Apply();
                GameStateManager.Instance.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, vertices, 0, 2);
            }
        }

        private void FillGrassTexture(Texture2D furTexture, float density)
        {
            //read the width and height of the texture
            int width = furTexture.Width;
            int height = furTexture.Height;
            int totalPixels = width * height;

            //an array to hold our pixels
            Color[] colors;
            colors = new Color[totalPixels];

            //random number generator
            Random rand = new Random();

            //initialize all pixels to transparent black
            for (int i = 0; i < totalPixels; i++)
                colors[i] = Color.TransparentBlack;

            //compute the number of opaque pixels = nr of hair strands
            int nrStrands = (int)(density * totalPixels);

            //compute the number of strands that stop at each layer
            int strandsPerLayer = nrStrands / nrOfLayers;

            //fill texture with opaque pixels
            for (int i = 0; i < nrStrands; i++)
            {
                int x, y;
                //random position on the texture
                x = rand.Next(height);
                y = rand.Next(width);

                //compute max layer
                int max_layer = i / strandsPerLayer;
                //normalize into [0..1] range
                float max_layer_n = (float)max_layer / (float)nrOfLayers;

                //put color (which has an alpha value of 255, i.e. opaque)
                //max_layer_n needs to be multiplied by 255 to achieve a color in [0..255] range
                colors[x * width + y] = new Color((byte)(max_layer_n * 255), 0, 0, 255);
            }

            //set the pixels on the texture.
            furTexture.SetData<Color>(colors);
        }

        private void GenerateGeometry()
        {
            vertices = new VertexPositionNormalTexture[6];
            vertices[0] = new VertexPositionNormalTexture(PositionOne, Vector3.UnitZ, new Vector2(0, 0));
            vertices[1] = new VertexPositionNormalTexture(PositionTwo, Vector3.UnitZ, new Vector2(1, 1));
            vertices[2] = new VertexPositionNormalTexture(new Vector3(PositionOne.X, PositionTwo.Y, PositionOne.Z), Vector3.UnitZ, new Vector2(0, 1));

            vertices[3] = vertices[0];
            vertices[4] = new VertexPositionNormalTexture(new Vector3(PositionTwo.X, PositionOne.Y, PositionOne.Z), Vector3.UnitZ, new Vector2(1, 0));
            vertices[5] = vertices[1];
        }
    }
}
