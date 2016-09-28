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
    public class EndScreen : GameState
    {
        private Image background;
        private Image bg;
        private Image skip;

        private Image text;

        private float counter; // counter for scrolling

        public EndScreen()
        {
            base.LoadContent();

            background = new Image();
            background.Path = "Images/TitleScreen";
            background.LoadContent();
            background.Scale = new Vector2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / ((float)background.Texture.Width / 1.5f),
                    (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / ((float)background.Texture.Height / 1.5f));
            background.Position = new Vector2(0, 0);

            bg = new Image();
            bg.Path = "Textures/Black";
            bg.Alpha = 0.3f;
            bg.LoadContent();
            bg.Scale = new Vector2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / ((float)background.Texture.Width / 1.5f),
                    (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / ((float)background.Texture.Height / 1.5f));
            bg.Position = new Vector2(GameStateManager.Instance.dimensions.X / 2, 0);

            text = new Image();
            text.Scale = new Vector2(1.2f, 1.2f);
            text.Text = "The winners of the tournament are certain and they immediately depart \n on an adventure with unknown and mysterious dangers. \n Yet these companions have the best chances to succeed, \n to find the golden cloud and return it to the scholars for study.  \n\n Thenceforth every year the Acagamics-Manikins and animals \n gather to honour the companions and their bravery with a celebration, \n the main event, without doubt, being the big tournament called 'Volamus'.";
            text.LoadContent();
            text.Position = new Vector2(GameStateManager.Instance.dimensions.X / 2 - 300, GameStateManager.Instance.dimensions.Y);

            skip = new Image();
            skip.Path = "Images/buttonTexture";
            skip.Text = "Close";
            skip.LoadContent();
            skip.Position = new Vector2(50, GameStateManager.Instance.dimensions.Y / 2 - 50);

            counter = 0.4f;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            skip.UnloadContent();
            bg.UnloadContent();
            background.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //  GameStateManager.Instance.Exit = true;

            background.Update(gameTime);
            bg.Update(gameTime);
            skip.Update(gameTime);
            text.Update(gameTime);
            text.Position = text.Position + new Vector2(0, -counter);

            if (text.Position.Y <= -750)
            {
                text.Position.Y = 1200;
            }
            skip.isActive = true;
            skip.ActivateEffect("FadeEffect");

            if (skip.isActive && (InputManager.Instance.ButtonPressed(Buttons.A) || InputManager.Instance.KeyPressed(Keys.Enter)))
            {
                GameStateManager.Instance.Exit = true;
            }
            if (InputManager.Instance.ButtonPressed(Buttons.B) || InputManager.Instance.KeyPressed(Keys.Escape))
            {
                GameStateManager.Instance.Exit = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            background.Draw(spriteBatch);
            bg.Draw(spriteBatch);
            skip.Draw(spriteBatch);
            text.Draw(spriteBatch);
        }
    }
}
