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
        private Match match;
        private float rotate = 90f;
        float x = 0;
        float y = 0;
        float test;

        //SplitScreen
        private Viewport defaultView, leftView, rightView;

        private FrameCounter frameCounter;

        private static GameScreen instance;

        public Match Match
        {
            get { return match; }
            set { match = value; }
        }

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

            defaultView = GameStateManager.Instance.GraphicsDevice.Viewport;
            leftView = defaultView;
            rightView = defaultView;
            leftView.Width = leftView.Width / 2;
            rightView.Width = rightView.Width / 2;
            rightView.X = leftView.Width + 1;


            frameCounter = new FrameCounter();
        }

        public override void LoadContent()
        {
            match.LoadContent();

            GameStateManager.Instance.Music.RemoveSoundSource("Content//Sound//going_coastal.ogg");

            GameStateManager.Instance.Music.Play2D("Content//Sound//soproSound1.ogg", true);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            GameStateManager.Instance.Music.RemoveSoundSource("Content//Sound//soproSound1.ogg");

            GameStateManager.Instance.Music.Play2D("Content//Sound//going_coastal.ogg", true);

        }

        public void UnloadContent2()
        {
            instance = null;
        }

        public override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            frameCounter.Update(deltaTime);

            match.Update(gameTime);

            if ((InputManager.Instance.KeyPressed(Keys.Escape) || InputManager.Instance.ButtonPressed(Buttons.Back)) && !Match.IsFinished)
            {
                GameStateManager.Instance.ChangeScreens("InGameMenu");
            }

            if ((InputManager.Instance.KeyPressed(Keys.Escape) || InputManager.Instance.ButtonPressed(Buttons.Back)) && Match.IsFinished)
            {
                GameStateManager.Instance.ChangeScreens("EndGameMenu");
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GameStateManager.Instance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GameStateManager.Instance.GraphicsDevice.Clear(Color.Black);

            //Linker SplitScreen: Linker Spieler (player_one)
            if (!GameScreen.Instance.Match.IsFinished)
            {
                GameStateManager.Instance.GraphicsDevice.Viewport = leftView;

                match.Draw(match.One.Camera, leftView);
            }

            //Rechter SplitScreen: Rechter Spieler (player_two)
            if (!GameScreen.Instance.Match.IsFinished)
            {
                GameStateManager.Instance.GraphicsDevice.Viewport = rightView;

                match.Draw(match.Two.Camera, rightView);
            }

 
            //Ganzes Bild
            GameStateManager.Instance.GraphicsDevice.Viewport = defaultView;

            // Ganze Bild nach Matchende
            if (GameScreen.Instance.Match.IsFinished == true)
            {

                //Startposition der Rotation bestimmen
                if (rotate == 90)
                {
                    if (match.Looser.Direction == 1)
                    {
                        rotate = 3 * MathHelper.Pi / 2; //180
                        test = rotate + 8 * MathHelper.Pi;
                    }
                    else
                    {
                        rotate = MathHelper.Pi / 2;  //90
                        test = rotate + 8 * MathHelper.Pi;
                    }
                    y = 100 * match.Winner.Direction;

                }

                GameScreen.Instance.Match.Draw(new Camera(new Vector3(x, y, 40), new Vector3(0, 0, 0), new Vector3(0, 0, 1)), defaultView);




                //Rotation
                x = 100 * (float)Math.Cos(rotate);
                y = 100 * (float)Math.Sin(rotate);
                rotate += 0.002f;//0.002f;

                //nach 4 Runden wird rotate zurückgesetzt, dass die Zahl nicht zu groß wird
                if (rotate >= test)
                {
                    rotate = 90;
                }
            }

            //zurücksetzen wenn ein Match zu Ende und noch eins begonnen wird
            else
            {
                rotate = 90f;
                x = 0;
                y = 70;
            }
            //  GameStateManager.Instance.GraphicsDevice.Viewport = defaultView;



            //FPS Anzeige
            var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);
            spriteBatch.DrawString(GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Standard"), fps, new Vector2(1, 1), Color.White);

            base.Draw(spriteBatch);
        }
        
    }
}
