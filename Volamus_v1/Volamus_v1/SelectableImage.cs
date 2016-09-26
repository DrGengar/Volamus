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
    public class SelectableImage : SelectableOptions
    {
        Texture2D[] options;

        public new Texture2D[] Array
        {
            get { return options; }
        }

        public SelectableImage(Texture2D[] o, String n) : base(new[] {"Image"}, n)
        {
            options = o;
            name = n;
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
            int width = -20;
            int x = (int)GameStateManager.Instance.dimensions.X;

            for (int i = 0; i < options.Length; i++)
            {
                width += options[i].Width + 20;
            }

            int mid = (x - width) / 2;
            width = 0;

            for (int i = 0; i < options.Length; i++)
            {
                if(i == active)
                {
                    if(is_active)
                    {
                        GameStateManager.Instance.SpriteBatch.Draw(GameStateManager.Instance.Content.Load<Texture2D>("Images/mark2"), new Vector2(mid + width - 7.5f, y - 7.5f));
                    }
                    else
                    {
                        GameStateManager.Instance.SpriteBatch.Draw(GameStateManager.Instance.Content.Load<Texture2D>("Images/mark"), new Vector2(mid + width - 7.5f, y - 7.5f));
                    }
                }

                GameStateManager.Instance.SpriteBatch.Draw(options[i], new Vector2(mid + width, y));
                width += options[i].Width + 20;
            }
        }
    }
}
