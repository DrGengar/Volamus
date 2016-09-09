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
    class Settings : GameState
    {
        SelectableInt ingame;
        SelectableInt music;

        SelectableOptions[] options;
        int active;

        Image save;

        public Settings()
        {
            int[] sound = new int[21];
            for (int i = 0; i < 21; i++)
            {
                sound[i] = 5 * i;
            }

            ingame = new SelectableInt(sound, "Ingame Volume");
            music = new SelectableInt(sound, "Music Volume");

            for (int i = 0; i < 21; i++)
            {
                if (sound[i] == GameStateManager.Instance.MusicVolume)
                {
                    music.active = i;
                }

                if (sound[i] == GameStateManager.Instance.IngameVolume)
                {
                    ingame.active = i;
                }
            }

            options = new SelectableOptions[2];
            options[0] = ingame;
            options[1] = music;

            active = 0;
            options[active].Active = true;

            save = new Image();
            save.Path = "Images/buttonTexture";
            save.Text = "Save & Back";
            save.LoadContent();
            save.Position = new Vector2(100, GameStateManager.Instance.dimensions.Y / 2 + 100);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            ingame.LoadContent();
            music.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            ingame.UnloadContent();
            music.UnloadContent();
            save.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            ingame.Update(gameTime);
            music.Update(gameTime);
            save.Update(gameTime);

            if (active < 2)
            {
                options[active].Active = false;
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    options[i].Active = false;
                }
            }

            if (InputManager.Instance.ButtonPressed(Buttons.DPadDown, Buttons.LeftThumbstickDown, Buttons.RightThumbstickDown) || InputManager.Instance.KeyPressed(Keys.Down))
            {
                GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);

                if (active == 2)
                {
                    active = 0;
                }
                else
                {
                    active += 1;
                }
            }

            if (InputManager.Instance.ButtonPressed(Buttons.DPadUp, Buttons.LeftThumbstickUp, Buttons.RightThumbstickUp) || InputManager.Instance.KeyPressed(Keys.Up))
            {
                GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);

                if (active == 0)
                {
                    active = 2;
                }
                else
                {
                    active -= 1;
                }
            }

            if (active < 2)
            {
                options[active].Active = true;
                save.isActive = false;
                save.DeactivateEffect("FadeEffect");
                save.Alpha = 1.0f;
            }
            else
            {
                save.isActive = true;
                save.ActivateEffect("FadeEffect");
            }

            if (save.isActive && (InputManager.Instance.ButtonPressed(Buttons.A) || InputManager.Instance.KeyPressed(Keys.Enter)))
            {
                //Speichern

                GameStateManager.Instance.IngameVolume = ingame.Array[ingame.active];
                GameStateManager.Instance.MusicVolume = music.Array[music.active];

                XmlManager<GameStateManager> xml = new XmlManager<GameStateManager>();
                xml.Save("Content/Load/GameStateManager.xml", GameStateManager.Instance);

                GameStateManager.Instance.Music.SoundVolume = ((float)GameStateManager.Instance.MusicVolume) / 100;
                GameStateManager.Instance.Ingame.SoundVolume = ((float)GameStateManager.Instance.IngameVolume) / 100;

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
            ingame.Draw((int)GameStateManager.Instance.dimensions.Y / 2 - 100);
            music.Draw((int)GameStateManager.Instance.dimensions.Y / 2);

            save.Draw(spriteBatch);
        }
    }
}
