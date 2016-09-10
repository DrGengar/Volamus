using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Volamus_v1
{
    public class TitleScreen : GameState
    {
        MenuManager menuManager;

        private Image Volamus;
        private Image background;

        public TitleScreen()
        {
            menuManager = new MenuManager();
        }

        public override void LoadContent()
        {
            Ball.Instance.UnloadContent();
            Collision.Instance.UnloadContent();
            GameScreen.Instance.UnloadContent2();

            base.LoadContent();
            menuManager.LoadContent("Content/Load/Menu/TitleScreen.xml");

            background = new Image();
            background.Path = "Images/TitleScreen";
            background.LoadContent();
            background.Scale = new Vector2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / ((float)background.Texture.Width / 1.5f),
                    (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / ((float)background.Texture.Height / 1.5f));
            background.Position = new Vector2(0,0);

            Volamus = new Image();
            Volamus.Path = "Images/gesamtesLogoPNG";
            Volamus.Scale = new Vector2(0.5f, 0.5f);
            Volamus.LoadContent();
            Volamus.Position = new Vector2((GameStateManager.Instance.dimensions.X - Volamus.SourceRect.Width)/2, -100);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            menuManager.UnloadContent();
            background.UnloadContent();
            Volamus.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            menuManager.Update(gameTime);
            background.Update(gameTime);
            Volamus.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            background.Draw(spriteBatch);
            Volamus.Draw(spriteBatch);
            menuManager.Draw(spriteBatch);
        }
    }
}
