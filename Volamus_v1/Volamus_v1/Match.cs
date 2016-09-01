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

        public Player One, Two;
        Field field;
        Wind wind;
        bool change_size;
        bool change_velocity;
        int maxPoints;

        bool isFinished;
        Player Winner, Looser;

        RockPaperSciccors rpsOne, rpsTwo;
        private bool preMatch;
        private const float delay = 2;
        private float remainingDelay = delay;

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
        }

        public void LoadContent()
        {
            field.LoadContent();

            Ball.Instance.LoadContent(wind);

            One.LoadContent();
            Two.LoadContent();

            rpsOne.LoadContent();
            rpsTwo.LoadContent();
        }

        public void Unloadcontent()
        {
            /*One.UnloadContent();
            Two.UnloadContent();
            field.UnloadContent();
            Ball.Instance.UnloadContent();*/
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
                if (One.Points == maxPoints)
                {
                    //winner == one -> Ende
                    isFinished = true;
                    Winner = One;
                    Looser = Two;

                    //...
                }

                if (Two.Points == maxPoints)
                {
                    //winner == two -> Ende
                    isFinished = true;
                    Winner = Two;
                    Looser = One;

                    //...
                }

                One.Update(field);
                Two.Update(field);

                Ball.Instance.Update();

                Collision.Instance.CollisionMethod(field);
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
                Ball.Instance.Draw(camera);
                One.Draw(camera);

                if (camera == One.Camera)
                {
                    One.DrawArrow(camera);
                    GameStateManager.Instance.SpriteBatch.DrawString(One.Font, One.Points.ToString() + " / " + maxPoints,
                        new Vector2(view.Width / 2, 0), Color.White);
                }
                else
                {
                    Two.DrawArrow(camera);
                    GameStateManager.Instance.SpriteBatch.DrawString(Two.Font, Two.Points.ToString() + " / " + maxPoints,
                        new Vector2(view.X + view.Width / 2, 0), Color.White);
                }

                Two.Draw(camera);
            }
        }
    }
}
