using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Ocean
    {
        // the ocean's required content
        private Effect oceanEffect;
        private Texture2D[] OceanNormalMaps;

        // The two-triangle generated model for the ocean
        private VertexPositionNormalTexture[] OceanVerts;

        /// <summary>
        /// Creates an Ocean object
        /// </summary>
        public Ocean()
        {

        }

        public void LoadContent()
        {
            // load the shader
            oceanEffect = GameStateManager.Instance.Content.Load<Effect>("Effects/OceanShader");
            // load the normal maps
            OceanNormalMaps = new Texture2D[4];
            for (int i = 0; i < 4; i++)
                OceanNormalMaps[i] = GameStateManager.Instance.Content.Load<Texture2D>("Textures/Ocean" + (i + 1) + "_N");

            // generate the geometry
            OceanVerts = new VertexPositionNormalTexture[6];
            OceanVerts[0] = new VertexPositionNormalTexture(new Vector3(-1000, 0, -1000), new Vector3(0, 1, 0), new Vector2(0, 0));
            OceanVerts[1] = new VertexPositionNormalTexture(new Vector3(1000, 0, -1000), new Vector3(0, 1, 0), new Vector2(1, 0));
            OceanVerts[2] = new VertexPositionNormalTexture(new Vector3(-1000, 0, 1000), new Vector3(0, 1, 0), new Vector2(0, 1));
            OceanVerts[3] = OceanVerts[2];
            OceanVerts[4] = OceanVerts[1];
            OceanVerts[5] = new VertexPositionNormalTexture(new Vector3(1000, 0, 1000), new Vector3(0, 1, 0), new Vector2(1, 1));
        }

        public void Draw(GameTime gameTime, Camera cam, Texture2D skyTexture, Vector3 position)
        {
            Player enemy = new Player();

            if(GameScreen.Instance.Match.One.Camera == cam)
            {
                enemy = GameScreen.Instance.Match.One.Enemy;
            }
            else
            {
                if (GameScreen.Instance.Match.Two.Camera == cam)
                {
                    enemy = GameScreen.Instance.Match.Two.Enemy;
                }
                else
                {
                    enemy.Camera = cam;
                }
            }
            // start the shader
            oceanEffect.CurrentTechnique.Passes[0].Apply();

            // set the transforms
            oceanEffect.Parameters["World"].SetValue(Matrix.CreateRotationX(MathHelper.ToRadians(90)) * Matrix.CreateTranslation(position));
            oceanEffect.Parameters["View"].SetValue(Matrix.CreateLookAt(enemy.Camera.Position, enemy.Camera.View, enemy.Camera.Up));
            oceanEffect.Parameters["Projection"].SetValue(enemy.Camera.ProjectionMatrix);
            oceanEffect.Parameters["EyePos"].SetValue(enemy.Camera.Position);

            // choose and set the ocean textures
            int oceanTexIndex = ((int)(gameTime.TotalGameTime.TotalSeconds) % 4);
            oceanEffect.Parameters["normalTex"].SetValue(OceanNormalMaps[(oceanTexIndex + 1) % 4]);
            oceanEffect.Parameters["normalTex2"].SetValue(OceanNormalMaps[(oceanTexIndex) % 4]);

            oceanEffect.Parameters["textureLerp"].SetValue((((((float)gameTime.TotalGameTime.TotalSeconds) - (int)(gameTime.TotalGameTime.TotalSeconds)) * 2 - 1) * 0.5f) + 0.5f);

            // set the time used for moving waves
            oceanEffect.Parameters["time"].SetValue((float)gameTime.TotalGameTime.TotalSeconds * 0.02f);

            // set the sky texture
            oceanEffect.Parameters["cubeTex"].SetValue(skyTexture);

            // draw our geometry
            GameStateManager.Instance.GraphicsDevice.DrawUserPrimitives<VertexPositionNormalTexture>(PrimitiveType.TriangleList, OceanVerts, 0, 2);

            // and we're done!
        }
    }
}
