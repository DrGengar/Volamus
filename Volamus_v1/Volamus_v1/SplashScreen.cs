using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Volamus_v1
{
    public class SplashScreen : GameState
    {
        public Image Screen;
        public Image Enter;
        public Image Volamus;

        public override void LoadContent()
        {
            base.LoadContent();
            Screen.LoadContent();

            Screen.Scale = new Vector2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / ((float)Screen.Texture.Width / 1.5f),
                    (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / ((float)Screen.Texture.Height / 1.5f));

            Volamus.LoadContent();
            Volamus.Scale = new Vector2((float)Volamus.SourceRect.Width / ((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 1.6f), (float)Volamus.SourceRect.Height / ((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 1.6f));
            Volamus.Position = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - Volamus.SourceRect.Width) / 2, 0);


            Enter.LoadContent();
            Enter.Scale = new Vector2(1.5f, 1.5f);
            Enter.Position = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - Enter.SourceRect.Width) / 2,
                (GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - 150));

            Enter.Color = Color.Black;
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            Screen.UnloadContent();
            Volamus.UnloadContent();
            Enter.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Screen.Update(gameTime);
            Volamus.Update(gameTime);
            Enter.Update(gameTime);

            if (InputManager.Instance.KeyPressed(Keys.Enter) || InputManager.Instance.ButtonPressed(Buttons.A))
            {
                GameStateManager.Instance.ChangeScreens("Story");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Screen.Draw(spriteBatch);
            Volamus.Draw(spriteBatch);
            Enter.Draw(spriteBatch);
        }
    }
}
