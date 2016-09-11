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
        private Image background;
        private Image bg;
        private Image skip;

        private Image text;

        private float counter; // counter for scrolling

        public Story()
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
            text.Text = " For a long, long time there has been a legend about a golden cloud. \n Nobody knows exactly how or where this cloud was created \n and which secrets it harbours. \n But one thing is for certain: \n The cloud is etxremely mighty and promises unlimited power. \n \n Amongst the Acagamics-manikins there is a group of \n old and wise scholars who believe in the core of truth in this legend. \n Therefore they made it their business to find the cloud and study it. \n For this daring task they are now looking to gather \n a fellowship of the most able. \n To be prepared for all possible dangers and to ascertain a save return, \n the companions have to possess both dexterity and strength, \n adaptability and courage. \n \n A tournament ensures that only the best of the best are \n selected for this adventure. \n Here the candidates have to compete in the most prestigious discipline, \n since only this way they can demonstrate their skills ideally...";
            text.LoadContent();
            text.Position = new Vector2(GameStateManager.Instance.dimensions.X / 2 - 300, GameStateManager.Instance.dimensions.Y);

            skip = new Image();
            skip.Path = "Images/buttonTexture";
            skip.Text = "Skip";
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
                GameStateManager.Instance.ChangeScreens("TitleScreen");
            }
            if (InputManager.Instance.ButtonPressed(Buttons.B) || InputManager.Instance.KeyPressed(Keys.Escape))
            {
                GameStateManager.Instance.ChangeScreens("TitleScreen");
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
