using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using IrrKlang;

namespace Volamus_v1
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            //graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = (int)GameStateManager.Instance.dimensions.X;
            graphics.PreferredBackBufferHeight = (int)GameStateManager.Instance.dimensions.Y;
            graphics.ApplyChanges();

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            GameStateManager.Instance.BackgroundSound = new ISoundEngine();
            GameStateManager.Instance.SoundEffects = new ISoundEngine();
            GameStateManager.Instance.GraphicsDevice = GraphicsDevice;
            GameStateManager.Instance.GraphicsDeviceManager = graphics;
            GameStateManager.Instance.SpriteBatch = spriteBatch;

            GameStateManager.Instance.LoadContent(Content);
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            GameStateManager.Instance.UnloadContent();
        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            GameStateManager.Instance.Update(gameTime);

            base.Update(gameTime);
        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            GameStateManager.Instance.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
