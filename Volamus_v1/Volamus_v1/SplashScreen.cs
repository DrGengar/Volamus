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
            Screen.Scale = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / Screen.Texture.Width,
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / Screen.Texture.Height);

            Volamus.LoadContent();
            Volamus.Position = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - Volamus.SourceRect.Width) / 2, 0);

            Volamus.Color = Color.Pink;

            Enter.LoadContent();
            Enter.Position = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - Enter.SourceRect.Width) / 2,
                ((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - Enter.SourceRect.Height) / 3) * 2);

            Enter.Color = Color.Pink;
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
                GameStateManager.Instance.ChangeScreens("TitleScreen");
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
