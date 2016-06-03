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
        Camera camera, camera_2;
        Field field;
        Player player_one, player_two;
        Ball ball;

        //SplitScreen
        Viewport defaultView, leftView, rightView;

        public GameScreen()
        {
            graphics = GameStateManager.Instance.GraphicsDeviceManager;
            content = GameStateManager.Instance.Content;

            field = new Field(50, 100, 15);
            camera = new Camera(new Vector3(0, -60, 20), new Vector3(0, 0, 0), new Vector3(0, 1, 1)); // 0,-60, 20   0,0,0    0,1,1

            camera_2 = new Camera(new Vector3(0, 60, 20), new Vector3(0, 0, 0), new Vector3(0, -1, 1));

            player_one = new Player(new Vector3(0, -25, 0), 5, 0.5f, 0.8f);
            player_two = new Player(new Vector3(0, 25, 0), 5, 0.5f, 0.8f);

            ball = new Ball(new Vector3(0, -10, 20), MathHelper.ToRadians(45), MathHelper.ToRadians(0), MathHelper.ToRadians(45));

            field.Initialize(graphics, content);
        }

        public override void LoadContent()
        {
            defaultView = GameStateManager.Instance.GraphicsDevice.Viewport;
            leftView = defaultView;
            rightView = defaultView;
            leftView.Width = leftView.Width / 2;
            rightView.Width = rightView.Width / 2;
            rightView.X = leftView.Width + 1;

            field.LoadContent(content);

            player_one.LoadContent(content);
            player_two.LoadContent(content);

            ball.LoadContent(content);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            camera.Update();
            camera_2.Update();

            player_one.Update(field);
            ball.Update(player_one);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GameStateManager.Instance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GameStateManager.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);

            GameStateManager.Instance.GraphicsDevice.Viewport = leftView;
            field.Draw(camera, graphics);
            ball.Draw(camera, graphics);
            player_one.Draw(camera, graphics);
            player_two.Draw(camera, graphics);

            GameStateManager.Instance.GraphicsDevice.Viewport = rightView;
            field.Draw(camera_2, graphics);
            ball.Draw(camera_2, graphics);
            player_one.Draw(camera_2, graphics);
            player_two.Draw(camera_2, graphics);

            GameStateManager.Instance.GraphicsDevice.Viewport = defaultView;

            base.Draw(spriteBatch);
        }
    }
}
