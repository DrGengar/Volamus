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
    public class Credits : GameState
    {
        private Image background;
        private Image bg;
        private Image back;

        private Image text;

        private int counter; // counter for scrolling

        public Credits()
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
            text.Text = "VOLAMUS \n \n \n a Game by Lena Spitz, Mareen Allgaier and Lars Haider \n \n \n Programmed for 3D Game Projects at OVGU \n \n \n Models created with Blender 2.77a \n \n \n TEXTURES: \n ice texture: \n https://freestocktextures.com/texture/ice-frozen-ground,200.html \n weitere texturen: https://pixabay.com/de \n \n \n SOUNDS \n http://www.ambiera.com/irrklang/features.html \n http://www.freesfx.co.uk \n child_crowd_cheering \n single_blow_from_police_whistle \n Going_Coastal \n Toggle_Switch\n \n \n SPECIAL THANKS TO: \n Gerd Schmidt";
            text.LoadContent();
            text.Position = new Vector2(GameStateManager.Instance.dimensions.X / 2 - 300, GameStateManager.Instance.dimensions.Y);

            back = new Image();
            back.Path = "Images/buttonTexture";
            back.Text = "Back";
            back.LoadContent();
            back.Position = new Vector2(50, GameStateManager.Instance.dimensions.Y / 2 - 50);

            counter = 1;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            back.UnloadContent();
            bg.UnloadContent();
            background.UnloadContent();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            background.Update(gameTime);
            bg.Update(gameTime);
            back.Update(gameTime);
            text.Update(gameTime);
            text.Position = text.Position + new Vector2(0, -counter);

            if (text.Position.Y <= -1100)
            {
                text.Position.Y = 1200;
            }


            back.isActive = true;
            back.ActivateEffect("FadeEffect");

            if (back.isActive && (InputManager.Instance.ButtonPressed(Buttons.A) || InputManager.Instance.KeyPressed(Keys.Enter)))
            {
                GameStateManager.Instance.ChangeScreens("Options");
            }
            if (InputManager.Instance.ButtonPressed(Buttons.B) || InputManager.Instance.KeyPressed(Keys.Escape))
            {
                GameStateManager.Instance.ChangeScreens("Options");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            background.Draw(spriteBatch);
            bg.Draw(spriteBatch);
            back.Draw(spriteBatch);
            text.Draw(spriteBatch);
        }
    }
}
