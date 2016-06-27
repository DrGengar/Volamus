using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;

namespace Volamus_v1
{
    public class GameScreen : GameState
    {
        private Field field;
        private Player player_one, player_two;

        //SplitScreen
        private Viewport defaultView, leftView, rightView;

        private FrameCounter frameCounter;

        private void Initialize()
        {
            content = GameStateManager.Instance.Content;

            field = new Field(100, 90, 15);

            player_one = new Player(new Vector3(0, -25, 0), 5, 0.5f, 0.8f, field);
            player_two = new Player(new Vector3(0, 25, 0), 5, 0.5f, 0.8f, field, PlayerIndex.One);

            player_one.Enemy = player_two;
            player_two.Enemy = player_one;

            field.Initialize();

            defaultView = GameStateManager.Instance.GraphicsDevice.Viewport;
            leftView = defaultView;
            rightView = defaultView;
            leftView.Width = leftView.Width / 2;
            rightView.Width = rightView.Width / 2;
            rightView.X = leftView.Width + 1;

            player_one.IsServing = true;
            Collision.Instance.LastTouched = player_one;

            frameCounter = new FrameCounter();
        }

        public override void LoadContent()
        {
            Initialize();

            field.LoadContent();

            Ball.Instance.LoadContent();

            player_one.LoadContent();
            player_two.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameCounter.Update(deltaTime);

            //player_one.Update(field, Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space, Keys.Q, Keys.E, Keys.Left, Keys.Right);
            //player_two.Update(field, Buttons.LeftThumbstickUp, Buttons.LeftThumbstickDown, Buttons.LeftThumbstickLeft, Buttons.LeftThumbstickRight, Buttons.A, Buttons.LeftTrigger, Buttons.RightTrigger, Buttons.RightThumbstickLeft, Buttons.RightThumbstickRight);
            player_one.Update(field);
            player_two.Update(field);


            Ball.Instance.Update();

            Collision.Instance.CollisionMethod(field);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GameStateManager.Instance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GameStateManager.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);

            //Linker SplitScreen: Linker Spieler (player_one)
            GameStateManager.Instance.GraphicsDevice.Viewport = leftView;
            field.Draw(player_one.Camera);
            Ball.Instance.Draw(player_one.Camera);
            player_one.Draw(player_one.Camera);
            player_two.Draw(player_one.Camera);

            //Halbkreis und Pfeil zeichnen
            GameStateManager.Instance.SpriteBatch.Draw(player_one.Circle, new Rectangle(leftView.X, leftView.Height - (player_one.Circle.Height), 
                player_one.Circle.Width, player_one.Circle.Height), Color.White);
            GameStateManager.Instance.SpriteBatch.Draw(player_one.Arrow, new Rectangle(leftView.X + player_one.Circle.Width/2, leftView.Height, player_one.Arrow.Width, player_one.Arrow.Height), null,
                Color.White, player_one.Direction*player_one.Gamma, new Vector2(player_one.Arrow.Width / 2f, player_one.Arrow.Height), SpriteEffects.None, 0f);

            //Punkte vom linken Spieler
            GameStateManager.Instance.SpriteBatch.DrawString(player_one.Font, player_one.Points.ToString(), 
                new Vector2(leftView.Width/2, 0), Color.Black);

            //Rechter SplitScreen: Rechter Spieler (player_two)
            GameStateManager.Instance.GraphicsDevice.Viewport = rightView;
            field.Draw(player_two.Camera);
            Ball.Instance.Draw(player_two.Camera);
            player_one.Draw(player_two.Camera);
            player_two.Draw(player_two.Camera);

            //Halbkreis und Pfeil zeichnen
            GameStateManager.Instance.SpriteBatch.Draw(player_two.Circle, new Rectangle(rightView.X, rightView.Height - (player_two.Circle.Height), 
                player_two.Circle.Width, player_two.Circle.Height), Color.White);
            GameStateManager.Instance.SpriteBatch.Draw(player_two.Arrow, new Rectangle(rightView.X + player_two.Circle.Width / 2, rightView.Height, player_two.Arrow.Width, player_two.Arrow.Height), null,
                Color.White, player_two.Direction*player_two.Gamma, new Vector2(player_two.Arrow.Width / 2f, player_two.Arrow.Height), SpriteEffects.None, 0f);

            //Punkte vom rechten Spieler
            GameStateManager.Instance.SpriteBatch.DrawString(player_two.Font, player_two.Points.ToString(), 
                new Vector2(leftView.Width + rightView.Width/2, 0), Color.Black);

            //Ganzes Bild
            GameStateManager.Instance.GraphicsDevice.Viewport = defaultView;

            //FPS Anzeige
            var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);
            spriteBatch.DrawString(GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Standard"), fps, new Vector2(1, 1), Color.Black);

            base.Draw(spriteBatch);
        }
        
    }
}
