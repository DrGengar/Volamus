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

        private Wind wind;

        private Match match;

        //SplitScreen
        private Viewport defaultView, leftView, rightView;

        private FrameCounter frameCounter;

        private static GameScreen instance;

        public static GameScreen Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameScreen();
                }

                return instance;
            }
        }

        private GameScreen()
        {
            content = GameStateManager.Instance.Content;

            field = new Field(100, 90, 15);
            field.Initialize();

            player_one = new Player(new Vector3(0, -25, 0), 5, 0.5f, 0.8f, field);
            player_two = new Player(new Vector3(0, 25, 0), 5, 0.5f, 0.8f, field, PlayerIndex.One);
            player_one.Enemy = player_two;
            player_two.Enemy = player_one;

            match = new Match(player_one, player_two, field, 200, 0,  false, false);

            defaultView = GameStateManager.Instance.GraphicsDevice.Viewport;
            leftView = defaultView;
            rightView = defaultView;
            leftView.Width = leftView.Width / 2;
            rightView.Width = rightView.Width / 2;
            rightView.X = leftView.Width + 1;

            //player_one.IsServing = true;
            //Collision.Instance.LastTouched = player_one;

            frameCounter = new FrameCounter();

            //wind = new Wind(0);

            GameStateManager.Instance.BackgroundSound.Play2D("Content//Sound//soproSound1.ogg", true);
            GameStateManager.Instance.BackgroundSound.SoundVolume = 0.2f;
        }

        public override void LoadContent()
        {
            /*field.LoadContent();

            Ball.Instance.LoadContent(wind);

            player_one.LoadContent();
            player_two.LoadContent();*/

            match.LoadContent();

            GameStateManager.Instance.BackgroundSound.SetAllSoundsPaused(false);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            match.Unloadcontent();
            instance = null;
            GameStateManager.Instance.BackgroundSound.SetAllSoundsPaused(true);
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameCounter.Update(deltaTime);

            /*player_one.Update(field);
            player_two.Update(field);


            Ball.Instance.Update();

            Collision.Instance.CollisionMethod(field);*/

            match.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.Escape) || InputManager.Instance.ButtonPressed(Buttons.Back))
            {
                GameStateManager.Instance.ChangeScreens("InGameMenu");
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GameStateManager.Instance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GameStateManager.Instance.GraphicsDevice.Clear(Color.Black);

            //Linker SplitScreen: Linker Spieler (player_one)
            GameStateManager.Instance.GraphicsDevice.Viewport = leftView;
            /*field.Draw(player_one.Camera);
            Ball.Instance.Draw(player_one.Camera);
            player_one.Draw(player_one.Camera);
            player_one.DrawArrow(player_one.Camera);

            player_two.Draw(player_one.Camera);*/

            match.Draw(match.One.Camera, leftView);

            //Punkte vom linken Spieler
            /*GameStateManager.Instance.SpriteBatch.DrawString(player_one.Font, player_one.Points.ToString() + "/" + Collision.Instance.match.ToString(), 
                new Vector2(leftView.Width/2, 0), Color.White);*/

            //Rechter SplitScreen: Rechter Spieler (player_two)
            GameStateManager.Instance.GraphicsDevice.Viewport = rightView;
            /*field.Draw(player_two.Camera);
            Ball.Instance.Draw(player_two.Camera);
            player_one.Draw(player_two.Camera);
            player_two.Draw(player_two.Camera);
            player_two.DrawArrow(player_two.Camera);*/

            match.Draw(match.Two.Camera, rightView);

            //Punkte vom rechten Spieler
            /*GameStateManager.Instance.SpriteBatch.DrawString(player_two.Font, player_two.Points.ToString() + "/" + Collision.Instance.match.ToString(), 
                new Vector2(leftView.Width + rightView.Width/2, 0), Color.White);*/

            //Ganzes Bild
            GameStateManager.Instance.GraphicsDevice.Viewport = defaultView;

            //FPS Anzeige
            var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);
            spriteBatch.DrawString(GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Standard"), fps, new Vector2(1, 1), Color.White);

            base.Draw(spriteBatch);
        }
        
    }
}
