﻿using Microsoft.Xna.Framework;
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
        public Vector3[] LightsPosition
        {
            get { return lightsPosition; }
        }

        public Vector4[] LightsAmbient
        {
            get { return lightsAmbient; }
        }

        public Vector4[] LightsDiffuse
        {
            get { return lightsDiffuse; }
        }

        public Vector4[] LightsSpecular
        {
            get { return lightsSpecular; }
        }

        public float[] LightsRadius
        {
            get { return lightsRadius; }
        }

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

        public Player One
        {
            get
            {
                if(pinguinOne != null) return pinguinOne;

                if(bumblebeeOne != null) return bumblebeeOne;

                if(dolphinOne != null) return dolphinOne;

                return null;
            }
        }

        public Player Two
        {
            get
            {
                if (pinguinTwo != null) return pinguinTwo;

                if (bumblebeeTwo != null) return bumblebeeTwo;

                if (dolphinTwo != null) return dolphinTwo;

                return null;
            }
        }

        public Field Field
        {
            get
            {
                if (iceField != null) return iceField;

                if (meadowField != null) return meadowField;

                if (waterField != null) return waterField;

                return null;
            }
        }

        private Image text;
        private float counter; // counter for scrolling

        Pinguin pinguinOne, pinguinTwo;
        BumbleBee bumblebeeOne, bumblebeeTwo;
        Dolphin dolphinOne, dolphinTwo;

        IceField iceField;
        MeadowField meadowField;
        WaterField waterField;

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
        Texture2D matchball;

        PointLight[] lights;
        Vector3[] lightsPosition;
        Vector4[] lightsAmbient;
        Vector4[] lightsDiffuse;
        Vector4[] lightsSpecular;
        float[] lightsRadius;

        public Match(Pinguin one, Pinguin two, IceField f, int points, int w, bool c_s, bool c_v)
        {
            iceField = f;

            pinguinOne = one;
            pinguinTwo = two;

            lights = new PointLight[4];
            lights[0] = new PointLight(new Vector3(-f.Length/2 - 10, -f.Width/2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);
            lights[1] = new PointLight(new Vector3(f.Length / 2 - 10, -f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);
            lights[2] = new PointLight(new Vector3(-f.Length / 2 - 10, f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);
            lights[3] = new PointLight(new Vector3(f.Length / 2 - 10, f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);

            Initialize(new Vector2(f.Length, f.Width), points, w, c_s, c_v);
        }

        public Match(BumbleBee one, BumbleBee two, MeadowField f, int points, int w, bool c_s, bool c_v)
        {
            meadowField = f;

            bumblebeeOne = one;
            bumblebeeTwo = two;

            lights = new PointLight[4];
            lights[0] = new PointLight(new Vector3(-f.Length / 2 - 10, -f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);
            lights[1] = new PointLight(new Vector3(f.Length / 2 - 10, -f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);
            lights[2] = new PointLight(new Vector3(-f.Length / 2 - 10, f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);
            lights[3] = new PointLight(new Vector3(f.Length / 2 - 10, f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);

            Initialize(new Vector2(f.Length, f.Width), points, w, c_s, c_v);
        }

        public Match(Dolphin one, Dolphin two, WaterField f, int points, int w, bool c_s, bool c_v)
        {
            waterField = f;

            dolphinOne = one;
            dolphinTwo = two;

            lights = new PointLight[4];
            lights[0] = new PointLight(new Vector3(-f.Length / 2 - 10, -f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);
            lights[1] = new PointLight(new Vector3(f.Length / 2 - 10, -f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);
            lights[2] = new PointLight(new Vector3(-f.Length / 2 - 10, f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);
            lights[3] = new PointLight(new Vector3(f.Length / 2 - 10, f.Width / 2 - 10, 20), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f), 100.0f);

            Initialize(new Vector2(f.Length, f.Width), points, w, c_s, c_v);
        }

        private void Initialize(Vector2 dimensions , int points, int w, bool c_s, bool c_v)
        {
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

            GroupOne = new SpectatorGroup(new Vector3(-dimensions.X/2 - 15, -dimensions.Y/2, 0), 5, rnd);
            GroupTwo = new SpectatorGroup(new Vector3(dimensions.X/2 + 15, dimensions.Y/2, 0), 5, rnd);

            lightsPosition = new Vector3[lights.Length];
            lightsAmbient = new Vector4[lights.Length];
            lightsDiffuse = new Vector4[lights.Length];
            lightsSpecular = new Vector4[lights.Length];
            lightsRadius = new float[lights.Length];

            for (int i = 0; i < lights.Length; i++)
            {
                lightsPosition[i] = lights[i].Position;
                lightsAmbient[i] = lights[i].Ambient;
                lightsDiffuse[i] = lights[i].Diffuse;
                lightsSpecular[i] = lights[i].Specular;
                lightsRadius[i] = lights[i].Radius;
            }
        }

        public void LoadContent()
        {
            text = new Image();
            text.Color = Color.Black;
            text.Scale = new Vector2(1.2f, 1.2f);
            text.Text = "Endstory";
            text.LoadContent();
            text.Position = new Vector2(GameStateManager.Instance.dimensions.X / 2 - 300, GameStateManager.Instance.dimensions.Y);
            counter = 0.4f;

            if (iceField != null)
            {
                iceField.LoadContent();
            }

            if (waterField != null)
            {
                waterField.LoadContent();
            }

            if (meadowField != null)
            {
                meadowField.LoadContent();
            }

            Ball.Instance.LoadContent(wind);

            if(pinguinOne != null && pinguinTwo != null)
            {
                pinguinOne.LoadContent();
                pinguinTwo.LoadContent();
            }

            if (bumblebeeOne != null && bumblebeeTwo != null)
            {
                bumblebeeOne.LoadContent();
                bumblebeeTwo.LoadContent();
            }

            if (dolphinOne != null && dolphinTwo != null)
            {
                dolphinOne.LoadContent();
                dolphinTwo.LoadContent();
            }

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
            matchball = GameStateManager.Instance.Content.Load<Texture2D>("Images/matchballPNG");
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

            if (InputManager.Instance.KeyPressed(Keys.F1))
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
                            One.Update(Field);
                            Two.Update(Field);

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
                                One.Update(Field);
                                Two.Update(Field);

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

                One.Update(Field);
                Two.Update(Field);

                if (change_size)
                {
                    changeSizePlus.Update(rnd);
                    changeSizeMinus.Update(rnd);
                }

                if (change_velocity)
                {
                    changeVelocityPlus.Update(rnd);
                    changeVelocityMinus.Update(rnd);
                }

                if (isFinished)
                {
                    confetti.Update(rnd);
                    text.Update(gameTime);
                    text.Position = text.Position + new Vector2(0, -counter);

                    if (text.Position.Y <= -750)
                    {
                        text.Position.Y = 1200;
                    }
                }

                if (!isFinished)
                {
                    Ball.Instance.Update();
                }

                Collision.Instance.CollisionMethod(Field);

                GroupOne.Update();
                GroupTwo.Update();
            }
        }

        public void Draw(Camera camera, Viewport view, SpriteBatch spriteBatch)
        {
            if (preMatch)
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
                if (iceField != null)
                {
                    iceField.Draw(camera);
                }

                if (waterField != null)
                {
                    waterField.Draw(camera);
                }

                if (meadowField != null)
                {
                    meadowField.Draw(camera);
                }

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
                        GameStateManager.Instance.SpriteBatch.DrawString(One.Font2, One.Points.ToString(), temp + new Vector2((pointsImage.Width / 2 - text.X) / 2, (pointsImage.Height / 2 - text.Y) / 2), Color.White);

                        if (Two.Points == maxPoints - 1 || One.Points == maxPoints - 1 && ((Two.Points != maxPoints - 1) || (One.Points != maxPoints)))
                        {
                            Vector2 temp2 = new Vector2(temp.X + (pointsImage.Width / 2) + 12, 0);
                            GameStateManager.Instance.SpriteBatch.Draw(matchball, temp2, null, Color.White, 0.0f, Vector2.Zero, 0.45f, SpriteEffects.None, 0);
                        }

                        if (One.Points == maxPoints - 1)
                        {
                            //GameStateManager.Instance.SpriteBatch.Draw(matchball, temp, null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                            GameStateManager.Instance.SpriteBatch.DrawString(One.Font2, One.Points.ToString(), temp + new Vector2((pointsImage.Width / 2 - text.X) / 2, (pointsImage.Height / 2 - text.Y) / 2), Color.Yellow);
                        }

                    }
                    else
                    {
                        if (winner == One)
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
                else
                {
                    if (camera == Two.Camera)
                    {
                        if (!isFinished)
                        {
                            Two.DrawArrow(camera);

                            Vector2 temp = new Vector2(view.X + pointsImage.Width / 4 + ((view.Width - pointsImage.Width) / 2), 0);

                            GameStateManager.Instance.SpriteBatch.Draw(pointsImage, temp, null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

                            Vector2 text = Two.Font2.MeasureString(Two.Points.ToString());
                            GameStateManager.Instance.SpriteBatch.DrawString(Two.Font2, Two.Points.ToString(), temp + new Vector2((pointsImage.Width / 2 - text.X) / 2, (pointsImage.Height / 2 - text.Y) / 2), Color.White);


                            if (Two.Points == maxPoints - 1)
                            {
                                //
                                //GameStateManager.Instance.SpriteBatch.Draw(matchball, temp, null, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                                GameStateManager.Instance.SpriteBatch.DrawString(Two.Font2, Two.Points.ToString(), temp + new Vector2((pointsImage.Width / 2 - text.X) / 2, (pointsImage.Height / 2 - text.Y) / 2), Color.Yellow);
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

                if (isFinished)
                {
                    confetti.Draw(camera);
                    if (GameScreen.Instance.Timer > 200)
                    {
                        text.Draw(spriteBatch);
                    }
                }

                GroupOne.Draw(camera);
                GroupTwo.Draw(camera);
            }
        }
    }
}
