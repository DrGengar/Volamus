using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Volamus_v1
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Kamera camera;
        Spielfeld field;


        public Game1()
        {
            field = new Spielfeld(50, 100, 20);
            camera = new Kamera(new Vector3(0,-62,30),new Vector3(0, 0, 0), Vector3.UnitZ);
            graphics = new GraphicsDeviceManager(this);
            graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
        }

    
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            field.Initialize(graphics);
            base.Initialize();
        }

 
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            field.LoadContent(Content);

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
            BasicEffect field_effect = field.get_effect();
            field_effect.View = camera.get_View();
            field_effect.Projection = camera.get_Projection();

            foreach(var pass in field_effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, field.get_field(), 0, 2);
            }



            base.Draw(gameTime);
        }
    }
}
