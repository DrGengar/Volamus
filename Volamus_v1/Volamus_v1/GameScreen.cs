using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Volamus_v1
{
    class GameScreen : GameState
    {
        GraphicsDeviceManager graphics;
        Camera camera;
        Field field;
        Player player_one;
        Ball ball;

        public GameScreen()
        {
            graphics = GameStateManager.Instance.GraphicsDeviceManager;
            content = GameStateManager.Instance.Content;

            field = new Field(50, 100, 15);
            camera = new Camera(new Vector3(0, -60, 20), new Vector3(0, 0, 0), new Vector3(0, 1, 1)); // 0,-60,20   0,0,0    0,1,1
            player_one = new Player(new Vector3(0, -25, 0), 5, 0.5f, 0.8f);
            ball = new Ball(new Vector3(0, -10, 20), MathHelper.ToRadians(45), MathHelper.ToRadians(0), MathHelper.ToRadians(45));

            field.Initialize(graphics, content);
        }

        public override void LoadContent()
        {
            field.LoadContent(content);
            player_one.LoadContent(content);
            ball.LoadContent(content);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            camera.Update();
            player_one.Update(field);
            ball.Update(player_one);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GameStateManager.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);

            field.Draw(camera, graphics);

            player_one.Draw(camera, graphics);

            ball.Draw(camera, graphics);

            base.Draw(spriteBatch);
        }
    }
}
