using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Volamus_v1
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Kamera camera;
        Spielfeld field;
        Spieler player_one;
        Ball ball;


        public Game1()
        {
            field = new Spielfeld(50, 100, 15);
            camera = new Kamera(new Vector3(0, -60, 20), new Vector3(0, 0, 0), new Vector3(0, 1, 1)); // 0,-60,20   0,0,0    0,1,1
            player_one = new Spieler(new Vector3(0,-25,0),5,0.5f,0.8f);
            ball = new Ball(new Vector3(0, -10, 20),MathHelper.ToRadians(45), MathHelper.ToRadians(0), MathHelper.ToRadians(45));    
            graphics = new GraphicsDeviceManager(this);
         //   graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            field.Initialize(graphics,Content);
            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            field.LoadContent(Content);
            player_one.LoadContent(Content);
            ball.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            camera.Update();
            player_one.Update(field);
            ball.Update(player_one);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            field.Draw(camera, graphics);

            player_one.Draw(camera, graphics);

            ball.Draw(camera, graphics);

            base.Draw(gameTime);
        }
    }
}
