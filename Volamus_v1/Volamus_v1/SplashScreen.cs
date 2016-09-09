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
            Volamus.Position = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - Volamus.SourceRect.Width) / 2, 0);

            Volamus.Color = Color.Black;

            Enter.LoadContent();
            Enter.Position = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - Enter.SourceRect.Width) / 2,
                ((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - Enter.SourceRect.Height) / 3) * 2);

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

            Screen.Scale = new Vector2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / ((float)Screen.Texture.Width / ((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / (float)Screen.Texture.Width)),
                (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / ((float)Screen.Texture.Height / ((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / (float)Screen.Texture.Height)));

            Volamus.Position = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - Volamus.SourceRect.Width) / 2, 0);

            Enter.Position = new Vector2((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width - Enter.SourceRect.Width) / 2,
                ((GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height - Enter.SourceRect.Height) / 3) * 2);

            if (InputManager.Instance.KeyPressed(Keys.Enter) || InputManager.Instance.ButtonPressed(Buttons.A))
            {
                GameStateManager.Instance.ChangeScreens("TitleScreen");
            }

            if (InputManager.Instance.KeyPressed(Keys.F1))
            {
                Field field = new Field(100, 90, 15);
                field.Initialize();

                Player one = new Player(new Vector3(0, -25, 0), 5, 0.5f, 0.8f, field);
                Player two = new Player(new Vector3(0, 25, 0), 5, 0.5f, 0.8f, field, PlayerIndex.One);

                one.Enemy = two;
                two.Enemy = one;

                GameScreen.Instance.Match = new Match(one, two, field, 50, 0, false, false);
                GameStateManager.Instance.ChangeScreens("GameScreen");
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
