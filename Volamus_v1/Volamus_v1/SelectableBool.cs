using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class SelectableBool : SelectableOptions
    {
        bool[] options;
        String[] help;

        public new bool[] Array
        {
            get { return options; }
        }

        public SelectableBool(string n) : base(new[] {"Off", "On"}, n)
        {
            options = new[] { false, true };
            name = n;
            help = new [] { "Off", "On"};
        }

        public new void Draw(int y)
        {
            int x = (int)GameStateManager.Instance.dimensions.X;
            int x_half = x / 2;

            if (is_active)
            {
                int difference = Math.Abs(markArrow.Height - (int)spriteFont.MeasureString(name).Y);
                GameStateManager.Instance.SpriteBatch.Draw(markArrow, new Vector2(100 - markArrow.Width - 10, y + difference / 2));
            }

            GameStateManager.Instance.SpriteBatch.DrawString(spriteFont, name, new Vector2(100, y), Color.White);

            GameStateManager.Instance.SpriteBatch.DrawString(spriteFont, help[active], new Vector2(20 + x_half, y), Color.White);
        }
    }
}
