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
    public class RockPaperSciccors
    {
        int decision = 0;
        Player player;
        Texture2D rock, paper, scissors;
        bool show_choice = false;
        bool ready = false;
        int win = 0;

        int height;
        Vector2 rockVec, paperVec, scissorsVec;
        SpriteFont headline, standard;

        public int Decision
        {
            get { return decision; }
            set { decision = value; }
        }

        public bool ShowChoice
        {
            get { return show_choice; }
            set { show_choice = value; }
        }

        public bool Ready
        {
            get { return ready; }
            set { ready = value; }
        }

        public int Win
        {
            get { return win; }
            set { win = value; }
        }

        public RockPaperSciccors(Player pl)
        {
            player = pl;
        }

        public void LoadContent()
        {
            rock = GameStateManager.Instance.Content.Load<Texture2D>("Images/rock");
            paper = GameStateManager.Instance.Content.Load<Texture2D>("Images/paper");
            scissors = GameStateManager.Instance.Content.Load<Texture2D>("Images/scissor");
            headline = GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Headline");
            standard = GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/standardbigger");
        }

        public void Update(GameTime gameTime)
        {
            if (decision == 0)
            {
                if (player.Control)
                {
                    if (GamePad.GetState(player.GamePadControl.Index).IsButtonDown(player.GamePadControl.Weak))
                    {
                        decision = 1;
                    }

                    if (GamePad.GetState(player.GamePadControl.Index).IsButtonDown(player.GamePadControl.Strong))
                    {
                        decision = 2;
                    }

                    if (GamePad.GetState(player.GamePadControl.Index).IsButtonDown(player.GamePadControl.Jump))
                    {
                        decision = 3;
                    }
                }
                else
                {
                    if (Keyboard.GetState().IsKeyDown(player.KeyboardControl.Weak))
                    {
                        decision = 1;
                    }

                    if (Keyboard.GetState().IsKeyDown(player.KeyboardControl.Strong))
                    {
                        decision = 2;
                    }

                    if (Keyboard.GetState().IsKeyDown(player.KeyboardControl.Jump))
                    {
                        decision = 3;
                    }
                }
            }
        }

        public void Draw(Viewport view)
        {
            if (show_choice)
            {
                if(win == 1) //You Won!
                {
                    GameStateManager.Instance.SpriteBatch.DrawString(headline, "Winner!", new Vector2((view.Width + view.X + view.X - headline.MeasureString("Winner").X)/2 , 0), Color.White); 
                }
                else
                {
                    if(win == 2) //You Lose!
                    {
                        GameStateManager.Instance.SpriteBatch.DrawString(headline, "Loser!", new Vector2((view.Width + view.X + view.X - headline.MeasureString("Looser").X) /2, 0), Color.White);
                    }
                    else
                    {
                        if(ready)
                        {
                            GameStateManager.Instance.SpriteBatch.DrawString(headline, "Tie!", new Vector2((view.Width + view.X + view.X - headline.MeasureString("Tie").X) /2, 0), Color.White);
                        }
                    }
                }

                switch(decision)
                {
                    case 1:
                        if (rockVec.X < ((view.Width + view.X) + view.X - rock.Width) / 2)
                        {
                            rockVec.X += 3;
                        }
                        else
                        {
                            ready = true;
                        }
                        GameStateManager.Instance.SpriteBatch.Draw(rock, new Rectangle((int)rockVec.X, (int)rockVec.Y,
                            rock.Width, rock.Height), Color.White);
                        break;

                    case 2:
                        if (paperVec.X < ((view.Width + view.X) + view.X - paper.Width) / 2)
                        {
                            paperVec.X += 3;
                        }
                        else
                        {
                            ready = true;
                        }
                        GameStateManager.Instance.SpriteBatch.Draw(paper, new Rectangle((int)paperVec.X, (int)paperVec.Y,
                            paper.Width, paper.Height), Color.White);
                        break;
                    case 3:
                        if (scissorsVec.X < ((view.Width + view.X) + view.X - scissors.Width) / 2)
                        {
                            scissorsVec.X += 3;
                        }
                        else
                        {
                            ready = true;
                        }
                        GameStateManager.Instance.SpriteBatch.Draw(scissors, new Rectangle((int)scissorsVec.X, (int)scissorsVec.Y,
                            scissors.Width, scissors.Height), Color.White);
                        break;

                    default:
                        break;
                }

            }
            else
            {
                GameStateManager.Instance.SpriteBatch.DrawString(standard, "Rock Paper Scissors - Make Your Choice!", new Vector2(20,50), Color.White);

                height = rock.Height + paper.Height + scissors.Height + 40;
                rockVec = new Vector2(view.X + 20, (view.Height - height) / 2);
                paperVec = rockVec + new Vector2(0, rock.Height + 20);
                scissorsVec = paperVec + new Vector2(0, paper.Height + 20);

                GameStateManager.Instance.SpriteBatch.Draw(rock, new Rectangle((int)rockVec.X, (int)rockVec.Y,
                    rock.Width, rock.Height), Color.White);
                GameStateManager.Instance.SpriteBatch.Draw(paper, new Rectangle((int)paperVec.X, (int)paperVec.Y,
                    paper.Width, paper.Height), Color.White);
                GameStateManager.Instance.SpriteBatch.Draw(scissors, new Rectangle((int)scissorsVec.X, (int)scissorsVec.Y,
                    scissors.Width, scissors.Height), Color.White);

                if (player.Control)
                {
                    GameStateManager.Instance.SpriteBatch.DrawString(player.Font, player.GamePadControl.Weak.ToString(),
                        rockVec + new Vector2(rock.Width + 20, (rock.Height - player.Font.MeasureString(player.GamePadControl.Weak.ToString()).Y) / 2), Color.White);

                    GameStateManager.Instance.SpriteBatch.DrawString(player.Font, player.GamePadControl.Strong.ToString(),
                        paperVec + new Vector2(paper.Width + 20, (paper.Height - player.Font.MeasureString(player.GamePadControl.Strong.ToString()).Y) / 2), Color.White);

                    GameStateManager.Instance.SpriteBatch.DrawString(player.Font, player.GamePadControl.Jump.ToString(),
                        scissorsVec + new Vector2(scissors.Width + 20, (scissors.Height - player.Font.MeasureString(player.GamePadControl.Jump.ToString()).Y) / 2), Color.White);
                }
                else
                {
                    GameStateManager.Instance.SpriteBatch.DrawString(player.Font, player.KeyboardControl.Weak.ToString(),
                                        rockVec + new Vector2(rock.Width + 20, (rock.Height - player.Font.MeasureString(player.KeyboardControl.Weak.ToString()).Y) / 2), Color.White);

                    GameStateManager.Instance.SpriteBatch.DrawString(player.Font, player.KeyboardControl.Strong.ToString(),
                        paperVec + new Vector2(paper.Width + 20, (paper.Height - player.Font.MeasureString(player.KeyboardControl.Strong.ToString()).Y) / 2), Color.White);

                    GameStateManager.Instance.SpriteBatch.DrawString(player.Font, player.KeyboardControl.Jump.ToString(),
                        scissorsVec + new Vector2(scissors.Width + 20, (scissors.Height - player.Font.MeasureString(player.KeyboardControl.Jump.ToString()).Y) / 2), Color.White);
                }
            }
        }

        public bool ThisBeats(RockPaperSciccors rps)
        {
            if(this.decision == 1 && rps.decision == 3)
            {
                return true;
            }

            if (this.decision == 2 && rps.decision == 1)
            {
                return true;
            }

            if (this.decision == 3 && rps.decision == 2)
            {
                return true;
            }

            return false;
        }
    }
}
