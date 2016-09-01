using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class SelectableInt : SelectableOptions
    {
        int[] options;

        public new int[] Array
        {
            get { return options; }
        }

        public SelectableInt(int[] o, String n) : base( new[] {"0"}, n)
        {
            options = o;
            name = n;
            active = options.Length - 1;
        }

        public new void Update(GameTime gameTime)
        {
            if (is_active)
            {
                if (InputManager.Instance.ButtonPressed(Buttons.DPadLeft, Buttons.LeftThumbstickLeft, Buttons.RightThumbstickLeft) || InputManager.Instance.KeyPressed(Keys.Left))
                {
                    GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);

                    if (active > 0)
                    {
                        active -= 1;
                    }
                    else
                    {
                        if (active == 0)
                        {
                            active = options.Length - 1;
                        }
                    }
                }

                if (InputManager.Instance.ButtonPressed(Buttons.DPadRight, Buttons.LeftThumbstickRight, Buttons.RightThumbstickRight) || InputManager.Instance.KeyPressed(Keys.Right))
                {
                    GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);

                    if (active < options.Length - 1)
                    {
                        active += 1;
                    }
                    else
                    {
                        if (active == options.Length - 1)
                        {
                            active = 0;
                        }
                    }
                }
            }
        }

        public new void Draw(int y)
        {
            int x = (int)GameStateManager.Instance.dimensions.X;
            int x_half = x / 2;

            if (is_active)
            {
                int difference = Math.Abs(markArrow.Height - (int) spriteFont.MeasureString(name).Y);
                GameStateManager.Instance.SpriteBatch.Draw(markArrow, new Vector2(100 - markArrow.Width - 10, y + difference/2));
            }

            GameStateManager.Instance.SpriteBatch.DrawString(spriteFont, name, new Vector2(100, y), Color.White);

            GameStateManager.Instance.SpriteBatch.DrawString(spriteFont, options[active].ToString(), new Vector2(20 + x_half, y), Color.White);
        }
    }
}
