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
    public class Story : GameState
    {
        private Image story;
        private Image background;

        private int i; //counter for scrolling

        public Story()
        {
            background = new Image();
            background.Path = "Images/TitleScreen";
            background.LoadContent();
            background.Scale = new Vector2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / ((float)background.Texture.Width / 1.5f),
                    (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / ((float)background.Texture.Height / 1.5f));
            background.Position = new Vector2(0, 0);

            story = new Image();
            story.Text = "story";  //story
            story.LoadContent();
            story.Position = new Vector2(10, GameStateManager.Instance.dimensions.Y / 2);

            i = 1;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            background.UnloadContent();
            story.UnloadContent();
            i = 0;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            background.Update(gameTime);
            story.Update(gameTime);

            story.Position = story.Position + new Vector2(10 + i, GameStateManager.Instance.dimensions.Y / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            background.Draw(spriteBatch);
            story.Draw(spriteBatch);
        }
    }
}
