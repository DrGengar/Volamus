using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Match
    {
        public int MaxPoints
        {
            get { return maxPoints; }
        }

        public Player PlayerOne
        {
            get { return One; }
        }

        public Player PlayerTwo
        {
            get { return Two; }
        }

        public Field Field
        {
            get { return field; }
        }

        public Wind Wind
        {
            get { return wind; }
        }

        public bool IsFinished
        {
            get { return isFinished; }
        }

        public Player Winner
        {
            get { return winner; }
        }

        public Player Looser
        {
            get { return looser; }
        }

        public Player One, Two;
        Field field;
        Wind wind;
        bool change_size;
        bool change_velocity;
        int maxPoints;

        bool isFinished;
        Player winner, looser;

        RockPaperSciccors rpsOne, rpsTwo;
        private bool preMatch;
        private const float delay = 2;
        private float remainingDelay = delay;

        PickSizePlus changeSizePlus;
        PickSizeMinus changeSizeMinus;

        PickVeloPlus changeVelocityPlus;
        PickVeloMinus changeVelocityMinus;

        Confetti confetti;
        Random rnd = new Random();

        SpectatorGroup GroupOne, GroupTwo;

        Image winnerImage, looserImage;
        Texture2D pointsImage;

        public Match(Player one, Player two, Field f,int points, int w, bool c_s, bool c_v)
        {
            One = one;
            Two = two;
            field = f;
            maxPoints = points;
            wind = new Wind(w);
            change_size = c_s;
            change_velocity = c_v;

            preMatch = true;
            rpsOne = new RockPaperSciccors(One);
            rpsTwo = new RockPaperSciccors(Two);

            isFinished = false;

            changeSizePlus = new PickSizePlus();
            changeVelocityPlus = new PickVeloPlus();

            changeSizeMinus = new PickSizeMinus();
            changeVelocityMinus = new PickVeloMinus();

            GroupOne = new SpectatorGroup(new Vector3(-60, -50, 0), 5, rnd);
            GroupTwo = new SpectatorGroup(new Vector3(60, 50, 0), 5, rnd);
        }

        public void LoadContent()
        {
            field.LoadContent();

            Ball.Instance.LoadContent(wind);

            One.LoadContent();
            Two.LoadContent();

            rpsOne.LoadContent();
            rpsTwo.LoadContent();

            changeSizePlus.LoadContent();
            changeSizeMinus.LoadContent();

            changeVelocityPlus.LoadContent();
            changeVelocityMinus.LoadContent();

            GroupOne.LoadContent();
            GroupTwo.LoadContent();

            winnerImage = new Image();
            winnerImage.Path = "Images/winnerPNG";
            winnerImage.Position = new Vector2(GameStateManager.Instance.GraphicsDeviceManager.PreferredBackBufferWidth / 2, 100);
            winnerImage.LoadContent();

            looserImage = new Image();
            looserImage.Path = "Images/looserPNG";
            looserImage.Position = new Vector2(GameStateManager.Instance.GraphicsDeviceManager.PreferredBackBufferWidth / 2, 100);
            looserImage.LoadContent();

            pointsImage = GameStateManager.Instance.Content.Load<Texture2D>("Images/AnzeigetafelPNG");
        }

        public void Unloadcontent()
        {
            /*One.UnloadContent();
            Two.UnloadContent();
            field.UnloadContent();
            Ball.Instance.UnloadContent();
            changeSize.UnloadContent();
            changeVelocity.UnloadContent();*/
        }

        public void Update(GameTime gameTime)
        {
            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(InputManager.Instance.KeyPressed(Keys.F1))
            {
                preMatch = false;
                One.IsServing = true;
                Collision.Instance.LastTouched = One;
            }

            if (preMatch)
            {
                rpsOne.Update(gameTime);
                rpsTwo.Update(gameTime);

                if (rpsOne.Decision != 0 && rpsTwo.Decision != 0)
                {
                    rpsOne.ShowChoice = true;
                    rpsTwo.ShowChoice = true;

                    if (rpsOne.Ready && rpsTwo.Ready)
                    {
                        if (rpsOne.ThisBeats(rpsTwo))
                        {
                            rpsOne.Win = 1;
                            rpsTwo.Win = 2;

                            One.IsServing = true;
                            Collision.Instance.LastTouched = One;
                            One.Update(field);
                            Two.Update(field);

                            Ball.Instance.Update();

                            remainingDelay -= timer;

                            if (remainingDelay <= 0)
                            {
                                preMatch = false;
                                remainingDelay = delay;
                            }
                        }
                        else
                        {
                            if (rpsTwo.ThisBeats(rpsOne))
                            {
                                rpsTwo.Win = 1;
                                rpsOne.Win = 2;

                                Two.IsServing = true;
                                Collision.Instance.LastTouched = Two;
                                One.Update(field);
                                Two.Update(field);

                                Ball.Instance.Update();

                                remainingDelay -= timer;

                                if (remainingDelay <= 0)
                                {
                                    preMatch = false;
                                    remainingDelay = delay;
                                }
                            }
                            else
                            {
                                remainingDelay -= timer;

                                if (remainingDelay <= 0)
                                {
                                    rpsOne.Decision = 0;
                                    rpsTwo.Decision = 0;
                                    rpsOne.ShowChoice = false;
                                    rpsTwo.ShowChoice = false;
                                    rpsOne.Ready = false;
                                    rpsTwo.Ready = false;

                                    remainingDelay = delay;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (One.Points == maxPoints && !isFinished)
                {
                    //winner == one -> Ende
                    isFinished = true;
                    winner = One;
                    looser = Two;
                    confetti = new Confetti(One.Direction);
                    GroupOne.SetCheering();
                }

                if (Two.Points == maxPoints && !isFinished)
                {
                    //winner == two -> Ende
                    isFinished = true;
                    winner = Two;
                    looser = One;
                    confetti = new Confetti(Two.Direction);
                    GroupTwo.SetCheering();
                }

                One.Update(field);
                Two.Update(field);

                if(change_size)
                {
                    changeSizePlus.Update(rnd);
                    changeSizeMinus.Update(rnd);
                }

                if(change_velocity)
                {
                    changeVelocityPlus.Update(rnd);
                    changeVelocityMinus.Update(rnd);
                }

                if(isFinished)
                {
                    confetti.Update(rnd);
                }

                if (!isFinished)
                {
                    Ball.Instance.Update();
                }

                Collision.Instance.CollisionMethod(field);

                GroupOne.Update();
                GroupTwo.Update();
            }
        }

        public void Draw(Camera camera, Viewport view)
        {
            if(preMatch)
            {
                if (camera == One.Camera)
                {
                    rpsOne.Draw(view);
                }
                else
                {
                    rpsTwo.Draw(view);
                }
            }
            else
            {
                field.Draw(camera);

                if (!isFinished)
                {
                    Ball.Instance.Draw(camera);
                }

                One.Draw(camera);

                if (camera == One.Camera)
                {
                    if (!isFinished)
                    {
                        One.DrawArrow(camera);

                        //pointsImage.Position = new Vector2(view.X + (view.Width - pointsImage.SourceRect.Width)/ 2, -50);
                        Vector2 temp = new Vector2(view.X + pointsImage.Width / 4 + ((view.Width - pointsImage.Width) / 2), 0);

                        GameStateManager.Instance.SpriteBatch.Draw(pointsImage, temp, null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

                        Vector2 text = One.Font2.MeasureString(One.Points.ToString());

                        if (One.Points == maxPoints - 1)
                        {
                            GameStateManager.Instance.SpriteBatch.DrawString(One.Font2, One.Points.ToString(), temp + new Vector2((pointsImage.Width / 2 - text.X) / 2, (pointsImage.Height / 2 - text.Y) / 2), Color.Yellow);
                        }
                        else
                        {
                            GameStateManager.Instance.SpriteBatch.DrawString(One.Font2, One.Points.ToString(), temp + new Vector2((pointsImage.Width / 2 - text.X) / 2, (pointsImage.Height / 2 - text.Y) / 2), Color.White);
                        }
                    }
                    else
                    {
                        if(winner == One)
                        {
                            winnerImage.Position = new Vector2(view.X + (view.Width - winnerImage.SourceRect.Width)/2, 100);
                            winnerImage.Draw(GameStateManager.Instance.SpriteBatch);
                        }
                        else
                        {
                            looserImage.Position = new Vector2(view.X + (view.Width - looserImage.SourceRect.Width)/2, 100);
                            looserImage.Draw(GameStateManager.Instance.SpriteBatch);
                        }
                    }
                }
                else
                {
                    if(camera == Two.Camera)
                    {
                        if (!isFinished)
                        {
                            Two.DrawArrow(camera);

                            Vector2 temp = new Vector2(view.X + pointsImage.Width / 4 + ((view.Width - pointsImage.Width) / 2), 0);

                            GameStateManager.Instance.SpriteBatch.Draw(pointsImage, temp, null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

                            Vector2 text = Two.Font2.MeasureString(Two.Points.ToString());

                            if (Two.Points == maxPoints - 1)
                            {
                                GameStateManager.Instance.SpriteBatch.DrawString(Two.Font2, Two.Points.ToString(), temp + new Vector2((pointsImage.Width / 2 - text.X) / 2, (pointsImage.Height / 2 - text.Y) / 2), Color.Tomato);
                            }
                            else
                            {
                                GameStateManager.Instance.SpriteBatch.DrawString(Two.Font2, Two.Points.ToString(), temp + new Vector2((pointsImage.Width / 2 - text.X) / 2, (pointsImage.Height / 2 - text.Y) / 2), Color.White);
                            }
                        }
                        else
                        {
                            if (winner == Two)
                            {
                                winnerImage.Position = new Vector2(view.X + (view.Width - winnerImage.SourceRect.Width) / 2, 100);
                                winnerImage.Draw(GameStateManager.Instance.SpriteBatch);
                            }
                            else
                            {
                                looserImage.Position = new Vector2(view.X + (view.Width - looserImage.SourceRect.Width) / 2, 100);
                                looserImage.Draw(GameStateManager.Instance.SpriteBatch);
                            }
                        }
                    }
                }

                Two.Draw(camera);

                if (change_size)
                {
                    changeSizePlus.Draw(camera);
                    changeSizeMinus.Draw(camera);
                }

                if (change_velocity)
                {
                    changeVelocityPlus.Draw(camera);
                    changeVelocityMinus.Draw(camera);
                }

                if(isFinished)
                {
                    confetti.Draw(camera);
                }

                GroupOne.Draw(camera);
                GroupTwo.Draw(camera);
            }
        }
    }
}
