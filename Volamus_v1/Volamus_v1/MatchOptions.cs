using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Volamus_v1
{
    public class MatchOptions : GameState
    {
        SelectableBool wind;
        SelectableBool enemyVelo;
        SelectableBool ballRadius;

        SelectableInt points;

        SelectableImage stages;

        SelectableOptions[] options;
        int active;

        Image back, play;

        Random rnd = new Random();

        Texture2D veloMinus, veloPlus, sizeMinus, sizePlus;

        public MatchOptions()
        {
            int[] p = new int[10];

            for (int i = 0; i < p.Length; i++)
            {
                p[i] = i * 5 + 5;
            }

            points = new SelectableInt(p, "Maximum Points");

            Texture2D mapOne, mapTwo, mapThree;
            mapOne = GameStateManager.Instance.Content.Load<Texture2D>("Images/m1.3");
            mapTwo = GameStateManager.Instance.Content.Load<Texture2D>("Images/map2.3");
            mapThree = GameStateManager.Instance.Content.Load<Texture2D>("Images/map3.2");

            stages = new SelectableImage(new[] { mapOne, mapTwo, mapThree }, "Maps");

            wind = new SelectableBool("Wind");
            enemyVelo = new SelectableBool("Velocity Drops");
            ballRadius = new SelectableBool("Ball Radius Drops");

            options = new SelectableOptions[5];
            options[0] = stages;
            options[1] = points;
            options[2] = wind;
            options[3] = enemyVelo;
            options[4] = ballRadius;

            active = 0;
            options[active].Active = true;

            back = new Image();
            back.Path = "Images/buttonTexture";
            back.Text = "Back";
            back.LoadContent();
            back.Position = new Vector2(60, GameStateManager.Instance.dimensions.Y - back.SourceRect.Height - 60);

            play = new Image();
            play.Path = "Images/buttonTexture";
            play.Text = "Play";
            play.LoadContent();
            play.Position = new Vector2(GameStateManager.Instance.dimensions.X - play.SourceRect.Width - 60, GameStateManager.Instance.dimensions.Y - play.SourceRect.Height - 60);
        }

        public override void LoadContent()
        {
            Ball.Instance.UnloadContent();
            Collision.Instance.UnloadContent();
            GameScreen.Instance.UnloadContent2();

            base.LoadContent();
            stages.LoadContent();
            points.LoadContent();
            wind.LoadContent();
            enemyVelo.LoadContent();
            ballRadius.LoadContent();

            veloMinus = GameStateManager.Instance.Content.Load<Texture2D>("Images/pfeil_green2");
            veloPlus = GameStateManager.Instance.Content.Load<Texture2D>("Images/pfeil_green3");
            sizeMinus = GameStateManager.Instance.Content.Load<Texture2D>("Images/pfeil_blau");
            sizePlus = GameStateManager.Instance.Content.Load<Texture2D>("Images/pfeil_blau2");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            stages.UnloadContent();
            points.UnloadContent();
            wind.UnloadContent();
            enemyVelo.UnloadContent();
            ballRadius.UnloadContent();
            play.UnloadContent();
            back.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            stages.Update(gameTime);
            points.Update(gameTime);
            wind.Update(gameTime);
            enemyVelo.Update(gameTime);
            ballRadius.Update(gameTime);
            back.Update(gameTime);
            play.Update(gameTime);

            if (active < 5)
            {
                options[active].Active = false;
            }
            else
            {
                for (int i = 0; i < 5; i++)
                {
                    options[i].Active = false;
                }
            }

            if (InputManager.Instance.ButtonPressed(Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown) || InputManager.Instance.KeyPressed(Keys.Down))
            {
                GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);

                if (active == 6)
                {
                    active = 0;
                }
                else
                {
                    active += 1;
                }
            }

            if (InputManager.Instance.ButtonPressed(Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp) || InputManager.Instance.KeyPressed(Keys.Up))
            {
                GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);

                if (active == 0)
                {
                    active = 6;
                }
                else
                {
                    active -= 1;
                }
            }

            if(active == 5)
            {
                if (InputManager.Instance.ButtonPressed(Buttons.DPadRight, Buttons.LeftThumbstickRight, Buttons.RightThumbstickRight) || InputManager.Instance.KeyPressed(Keys.Right))
                {
                    GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);
                    active = 6;
                }
            }

            if (active == 6)
            {
                if (InputManager.Instance.ButtonPressed(Buttons.DPadLeft, Buttons.LeftThumbstickLeft, Buttons.RightThumbstickLeft) || InputManager.Instance.KeyPressed(Keys.Left))
                {
                    GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);
                    active = 5;
                }
            }

            if (active < 5)
            {
                options[active].Active = true;
                back.isActive = false;
                play.isActive = false;
                back.DeactivateEffect("FadeEffect");
                play.DeactivateEffect("FadeEffect");
                back.Alpha = 1.0f;
                play.Alpha = 1.0f;
            }
            else
            {
                if (active == 5)
                {
                    back.isActive = true;
                    back.ActivateEffect("FadeEffect");
                    play.isActive = false;
                    play.DeactivateEffect("FadeEffect");
                    play.Alpha = 1.0f;
                }
                else
                {
                    play.isActive = true;
                    play.ActivateEffect("FadeEffect");
                    back.isActive = false;
                    back.DeactivateEffect("FadeEffect");
                    back.Alpha = 1.0f;
                }
            }

            if ((back.isActive && (InputManager.Instance.ButtonPressed(Buttons.A) || InputManager.Instance.KeyPressed(Keys.Enter))) || InputManager.Instance.ButtonPressed(Buttons.B) || InputManager.Instance.KeyPressed(Keys.Escape))
            {
                GameStateManager.Instance.ChangeScreens("TitleScreen");
            }

            if (play.isActive && (InputManager.Instance.ButtonPressed(Buttons.A) || InputManager.Instance.KeyPressed(Keys.Enter)))
            {

                if(stages.active == 0)
                {
                    IceField field = new IceField(100, 90, 15, rnd);
                    field.Initialize();

                    GameScreen.Instance.SoundFileName = "Content//Sound//soproSound1.ogg";

                    Pinguin one, two;

                    if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                    {
                        one = new Pinguin(new Vector3(0, -25, 0), 5, 0, 0.5f, 0.8f, field, PlayerIndex.One);
                        two = new Pinguin(new Vector3(0, 25, 0), 5, 0, 0.5f, 0.8f, field, PlayerIndex.Two);
                    }
                    else
                    {
                        one = new Pinguin(new Vector3(0, -25, 0), 5, 0, 0.5f, 0.8f, field);
                        two = new Pinguin(new Vector3(0, 25, 0), 5, 0, 0.5f, 0.8f, field, PlayerIndex.One);
                    }

                    one.Enemy = two;
                    two.Enemy = one;

                    int w = 0;

                    if (wind.Array[wind.active])
                    {
                        w = 1;
                    }

                    GameScreen.Instance.Match = new Match(one, two, field, points.Array[points.active], w, ballRadius.Array[ballRadius.active], enemyVelo.Array[enemyVelo.active]);

                    GameStateManager.Instance.ChangeScreens("GameScreen");
                }

                if (stages.active == 1)
                {
                    MeadowField field = new MeadowField(100, 90, 15, rnd);
                    field.Initialize();

                    GameScreen.Instance.SoundFileName = "Content//Sound//hummel.ogg";

                    BumbleBee one, two;

                    if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                    {
                        one = new BumbleBee(new Vector3(0, -25, 5), 10, 5 , 0.5f, 0.8f, field, PlayerIndex.One);
                        two = new BumbleBee(new Vector3(0, 25, 5), 10, 5, 0.5f, 0.8f, field, PlayerIndex.Two);
                    }
                    else
                    {
                        one = new BumbleBee(new Vector3(0, -25, 5), 10, 5, 0.5f, 0.8f, field);
                        two = new BumbleBee(new Vector3(0, 25, 5), 10, 5, 0.5f, 0.8f, field, PlayerIndex.One);
                    }

                    one.Enemy = two;
                    two.Enemy = one;

                    int w = 0;

                    if (wind.Array[wind.active])
                    {
                        w = 1;
                    }

                    GameScreen.Instance.Match = new Match(one, two, field, points.Array[points.active], w, ballRadius.Array[ballRadius.active], enemyVelo.Array[enemyVelo.active]);

                    GameStateManager.Instance.ChangeScreens("GameScreen");
                }

                if (stages.active == 2)
                {
                    WaterField field = new WaterField(100, 90, 15, rnd);
                    field.Initialize();

                    Dolphin one, two;

                    GameScreen.Instance.SoundFileName = "Content//Sound//Delphin.ogg";

                    if (GamePad.GetState(PlayerIndex.Two).IsConnected)
                    {
                        one = new Dolphin(new Vector3(0, -25, 0), 5, 0, 0.5f, 0.8f, field, PlayerIndex.One);
                        two = new Dolphin(new Vector3(0, 25, 0), 5, 0, 0.5f, 0.8f, field, PlayerIndex.Two);
                    }
                    else
                    {
                        one = new Dolphin(new Vector3(0, -25, 0), 5, 0, 0.5f, 0.8f, field);
                        two = new Dolphin(new Vector3(0, 25, 0), 5, 0, 0.5f, 0.8f, field, PlayerIndex.One);
                    }

                    one.Enemy = two;
                    two.Enemy = one;

                    int w = 0;

                    if (wind.Array[wind.active])
                    {
                        w = 1;
                    }

                    GameScreen.Instance.Match = new Match(one, two, field, points.Array[points.active], w, ballRadius.Array[ballRadius.active], enemyVelo.Array[enemyVelo.active]);

                    GameStateManager.Instance.ChangeScreens("GameScreen");
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont spriteFont = GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Standard");
            base.Draw(spriteBatch);
            stages.Draw(100);
            points.Draw(350);
            wind.Draw(400);
            enemyVelo.Draw(450);
            GameStateManager.Instance.SpriteBatch.Draw(veloMinus ,new Vector2(120 + spriteFont.MeasureString("Velocity Drops").X, 450));
            GameStateManager.Instance.SpriteBatch.Draw(veloPlus, new Vector2(140 + spriteFont.MeasureString("Velocity Drops").X, 450));
            ballRadius.Draw(500);
            GameStateManager.Instance.SpriteBatch.Draw(sizeMinus, new Vector2(120 + spriteFont.MeasureString("Ball Radius Drops").X, 500));
            GameStateManager.Instance.SpriteBatch.Draw(sizePlus, new Vector2(140 + spriteFont.MeasureString("Ball Radius Drops").X, 500));

            play.Draw(spriteBatch);
            back.Draw(spriteBatch);
        }
    }
}
