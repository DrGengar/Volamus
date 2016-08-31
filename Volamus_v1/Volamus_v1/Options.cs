using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Volamus_v1
{
    class Options : GameState
    {
        SelectableInt bgSound;
        SelectableBool music;
        SelectableImage image;

        public Options()
        {
            int[] sound = new int[21];
            for(int i = 0; i < 21; i++)
            {
                sound[i] = 5*i;
            }

            Texture2D image1, image2, image3;

            image1 = GameStateManager.Instance.Content.Load<Texture2D>("Images/rock");
            image2 = GameStateManager.Instance.Content.Load<Texture2D>("Images/paper");
            image3 = GameStateManager.Instance.Content.Load<Texture2D>("Images/scissor");

            Texture2D[] im = new[] { image1, image2, image3 };

            bgSound = new SelectableInt(sound, "BackgroundSound");
            music = new SelectableBool("Music");
            image = new SelectableImage(im, "Image");

            image.Active = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            bgSound.LoadContent();
            music.LoadContent();
            image.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            bgSound.UnloadContent();
            music.UnloadContent();
            image.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            bgSound.Update(gameTime);
            music.Update(gameTime);
            image.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            bgSound.Draw(100);
            music.Draw(200);
            image.Draw(300);
        }
    }
}
