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
    public class Controls : GameState
    {
        private Viewport defaultView, leftView, rightView;
        KeyboardControl keyboard;
        GamePadControl gamePad;

        SpriteFont font;

        Image back;

        public Controls()
        {
            defaultView = GameStateManager.Instance.GraphicsDevice.Viewport;
            leftView = defaultView;
            rightView = defaultView;
            leftView.Width = leftView.Width / 2;
            rightView.Width = rightView.Width / 2;
            rightView.X = leftView.Width;

            keyboard = new KeyboardControl();
            gamePad = new GamePadControl();

            font = GameStateManager.Instance.Content.Load<SpriteFont>("SpriteFonts/Standard");

            back = new Image();
            back.Path = "Images/buttonTexture";
            back.Text = "Back";
            back.isActive = true;
            back.LoadContent();
            back.ActivateEffect("FadeEffect");
            back.Position = new Vector2((defaultView.Width - back.SourceRect.Width)/2, defaultView.Height - back.SourceRect.Height - 40);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            keyboard = keyboard.LoadContent();
            gamePad = gamePad.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            keyboard.UnloadContent();
            gamePad.UnloadContent();
            back.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            back.Update(gameTime);

            back.Position.X = (GameStateManager.Instance.GraphicsDeviceManager.PreferredBackBufferWidth - back.Texture.Width) / 2;

            if(InputManager.Instance.ButtonPressed(Buttons.A, Buttons.Back) || InputManager.Instance.KeyPressed(Keys.Enter, Keys.Escape))
            {
                GameStateManager.Instance.ChangeScreens("Options");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            GameStateManager.Instance.SpriteBatch.DrawString(font, "Up", new Vector2(20, 100), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, "Left", new Vector2(20, 150), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, "Right", new Vector2(20, 200), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, "Down", new Vector2(20, 250), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, "Jump", new Vector2(20, 300), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, "Weak Hit/Serve", new Vector2(20, 350), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, "Strong Hit/Serve", new Vector2(20, 400), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, "Spin Left", new Vector2(20, 450), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, "Spin Right", new Vector2(20, 500), Color.White);

            //Keyboard
            GameStateManager.Instance.GraphicsDevice.Viewport = leftView;

            GameStateManager.Instance.SpriteBatch.DrawString(font, "Keyboard", new Vector2((leftView.X + (leftView.Width + leftView.X))/ 2 - 75, 0), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, keyboard.Up.ToString(), new Vector2((leftView.X + (leftView.Width + leftView.X)) / 2 - 50, 100), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, keyboard.Left.ToString(), new Vector2((leftView.X + (leftView.Width + leftView.X)) / 2 - 50, 150), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, keyboard.Right.ToString(), new Vector2((leftView.X + (leftView.Width + leftView.X)) / 2 - 50, 200), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, keyboard.Down.ToString(), new Vector2((leftView.X + (leftView.Width + leftView.X)) / 2 - 50, 250), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, keyboard.Jump.ToString(), new Vector2((leftView.X + (leftView.Width + leftView.X)) / 2 - 50, 300), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, keyboard.Weak.ToString(), new Vector2((leftView.X + (leftView.Width + leftView.X)) / 2 - 50, 350), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, keyboard.Strong.ToString(), new Vector2((leftView.X + (leftView.Width + leftView.X)) / 2 - 50, 400), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, keyboard.Dir_Left.ToString(), new Vector2((leftView.X + (leftView.Width + leftView.X)) / 2 - 50, 450), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, keyboard.Dir_Right.ToString(), new Vector2((leftView.X + (leftView.Width + leftView.X)) / 2 - 50, 500), Color.White);

            //GamePad
            GameStateManager.Instance.GraphicsDevice.Viewport = rightView;

            GameStateManager.Instance.SpriteBatch.DrawString(font, "Controller", new Vector2((rightView.X + (rightView.Width + rightView.X))/2, 0), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, gamePad.Up.ToString(), new Vector2((rightView.X + (rightView.Width + rightView.X)) / 2 - 100, 100), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, gamePad.Left.ToString(), new Vector2((rightView.X + (rightView.Width + rightView.X)) / 2 - 100, 150), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, gamePad.Right.ToString(), new Vector2((rightView.X + (rightView.Width + rightView.X)) / 2 - 100, 200), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, gamePad.Down.ToString(), new Vector2((rightView.X + (rightView.Width + rightView.X)) / 2 - 100, 250), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, gamePad.Jump.ToString(), new Vector2((rightView.X + (rightView.Width + rightView.X)) / 2 - 100, 300), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, gamePad.Weak.ToString(), new Vector2((rightView.X + (rightView.Width + rightView.X)) / 2 - 100, 350), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, gamePad.Strong.ToString(), new Vector2((rightView.X + (rightView.Width + rightView.X)) / 2 - 100, 400), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, gamePad.Dir_Left.ToString(), new Vector2((rightView.X + (rightView.Width + rightView.X)) / 2 - 100, 450), Color.White);
            GameStateManager.Instance.SpriteBatch.DrawString(font, gamePad.Dir_Right.ToString(), new Vector2((rightView.X + (rightView.Width + rightView.X)) / 2 - 100, 500), Color.White);

            GameStateManager.Instance.GraphicsDevice.Viewport = defaultView;

            back.Draw(spriteBatch);
        }
    }
}
