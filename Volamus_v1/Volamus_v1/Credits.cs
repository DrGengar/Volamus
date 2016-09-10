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
        private Image back;
        private Image[] credits;

        private Image title;
        private Image names;
        private Image context;
        private Image textures;
        private Image textures2;
        private Image textures3;
        private Image sound;
        private Image sound2;
        private Image sound3;
        private Image sound4;
        private Image specialThanks;
        private Image specialThanks2;

        private Image text;

        private int counter; // counter for scrolling
        bool visible;

        public Credits()
        {
            base.LoadContent();

            background = new Image();
            background.Path = "Images/TitleScreen";
            background.LoadContent();
            background.Scale = new Vector2((float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / ((float)background.Texture.Width / 1.5f),
                    (float)GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / ((float)background.Texture.Height / 1.5f));
            background.Position = new Vector2(0, 0);

            title = new Image();
            names = new Image();
            context = new Image();
            textures = new Image();
            textures2 = new Image();
            textures3 = new Image();
            sound = new Image();
            sound2 = new Image();
            sound3 = new Image();
            sound4 = new Image();
            specialThanks = new Image();
            specialThanks2 = new Image();

            credits = new Image[12];

            //credits[0] = title;
            //credits[1] = names;
            //credits[2] = context;
            //credits[3] = textures;
            //credits[4] = textures2;
            //credits[5] = textures3;
            //credits[6] = sound;
            //credits[7] = sound2;
            //credits[8] = sound3;
            //credits[9] = sound4;
            //credits[10] = specialThanks;
            //credits[11] = specialThanks2;


            //for (int i = 0; i < credits.Length; i++) {
            //    //credits[i] = new Image();
            //    credits[i].LoadContent();
            //    credits[i].Position = new Vector2(GameStateManager.Instance.dimensions.X / 2 - 300, GameStateManager.Instance.dimensions.Y - 200 + i*10);
            //}

            //title.Text = "VOLAMUS \n \n";
            //names.Text = "a Game by Lena Spitz, Mareen Allgaier and Lars Haider \n \n";
            //context.Text = "Programmed for 3D Game Projects at OVGU \n \n";
            //textures.Text = "TEXTURES:";
            //textures2.Text = "ice texture: https://freestocktextures.com/texture/ice-frozen-ground,200.html";
            //textures3.Text = "weitere texturen: https://pixabay.com/de \n \n";
            //sound.Text = "SOUNDS:";
            //sound2.Text = " http://www.freesfx.co.uk";
            //sound3.Text = "child_crowd_cheering";
            //sound4.Text = "single_blow_from_police_whistle \n \n";
            //specialThanks.Text = "SPECIAL THANKS TO";
            //specialThanks2.Text = "Gerd Schmidt";

            text = new Image();
            text.Scale = new Vector2(1.4f, 1.4f);
            text.Text = "VOLAMUS \n \n \n a Game by Lena Spitz, Mareen Allgaier and Lars Haider \n \n \n Programmed for 3D Game Projects at OVGU \n \n \n TEXTURES: \n ice texture: \n https://freestocktextures.com/texture/ice-frozen-ground,200.html \n weitere texturen: https://pixabay.com/de \n \n \n SOUNDS \n http://www.freesfx.co.uk \n child_crowd_cheering \n single_blow_from_police_whistle \n \n \n SPECIAL THANKS TO: \n Gerd Schmidt";
            text.LoadContent();
            text.Position = new Vector2(GameStateManager.Instance.dimensions.X / 2 - 300, GameStateManager.Instance.dimensions.Y);

            back = new Image();
            back.Path = "Images/buttonTexture";
            back.Text = "Back";
            back.LoadContent();
            back.Position = new Vector2(50, GameStateManager.Instance.dimensions.Y / 2 - 50);

            counter = 1;
            visible = false;
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            back.UnloadContent();
            background.UnloadContent();

            //for (int i = 0; i < credits.Length; i++)
            //{
            //    credits[i].UnloadContent();
            //}
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            background.Update(gameTime);
            back.Update(gameTime);
            text.Update(gameTime);
            text.Position = text.Position + new Vector2(0, -counter);

            if (text.Position.Y <= -100)
            {
                GameStateManager.Instance.ChangeScreens("Options");
            }

            //for (int i = 0; i < credits.Length; i++)
            //{
            //    credits[i].Update(gameTime);
            //    credits[i].Position = credits[i].Position + new Vector2(0, -counter);
            //    if (credits[i].Position.Y > 100 || credits[i].Position.Y <= GameStateManager.Instance.dimensions.Y - 200 + i*50)
            //    {
            //        visible = true;
            //    }
            //}


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
            back.Draw(spriteBatch);
            text.Draw(spriteBatch);

            //if (visible == true)
            //{
            //    for (int i = 0; i < credits.Length; i++)
            //    {
            //        credits[i].Draw(spriteBatch);
            //    }
            //}

        }
    }
}
