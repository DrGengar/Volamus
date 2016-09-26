using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using IrrKlang;

namespace Volamus_v1
{
    public class GameStateManager
    {
        private static GameStateManager instance;

        [XmlIgnore]
        public Vector2 dimensions { private set; get; }
        [XmlIgnore]
        public ContentManager Content { private set; get; }
        XmlManager<GameState> xmlGameStateManager;

        GameState currentState, newState;
        [XmlIgnore]
        public GraphicsDevice GraphicsDevice;
        [XmlIgnore]
        public GraphicsDeviceManager GraphicsDeviceManager;
        [XmlIgnore]
        public ISoundEngine Music;
        public int MusicVolume;

        [XmlIgnore]
        public ISoundEngine Ingame;
        public int IngameVolume;

        public bool Fullscreen;

        [XmlIgnore]
        public ISoundEngine Menu;

        [XmlIgnore]
        public bool Exit;

        [XmlIgnore]
        public SpriteBatch SpriteBatch;

        public Image Image;
        [XmlIgnore]
        public bool isTransitioning { get; private set; }

        public static GameStateManager Instance
        {
            get
            {
                if (instance == null)
                {
                    XmlManager<GameStateManager> xml = new XmlManager<GameStateManager>();
                    instance = xml.Load("Content/Load/GameStateManager.xml");
                }

                return instance;
            }
        }

        public void ChangeScreens(string screenName)
        {
            switch(screenName)
            {
                case "GameScreen":
                    newState = (GameState)GameScreen.Instance;
                    break;

                case "SplashScreen":
                    newState = (GameState)(new SplashScreen());
                    break;

                case "TitleScreen":
                    newState = (GameState)(new TitleScreen());
                    break;

                case "InGameMenu":
                    newState = (GameState)(new InGameMenu());
                    break;

                case "EndGameMenu":
                    newState = (GameState)(new EndGameMenu());
                    break;

                case "Settings":
                    newState = (GameState)(new Settings());
                    break;

                case "Options":
                    newState = (GameState)(new Options());
                    break;

                case "EndScreen":
                    newState = (GameState)(new EndScreen());
                    break;

                case "MatchOptions":
                    newState = (GameState)(new MatchOptions());
                    break;

                case "Controls":
                    newState = (GameState)(new Controls());
                    break;

                case "Credits":
                    newState = (GameState)(new Credits());
                    break;

                case "Story":
                    newState = (GameState)(new Story());
                    break;
            }

            Image.isActive = true;
            Image.FadeEffect.Increase = true;
            Image.Alpha = 0.0f;
            isTransitioning = true;
        }

        void Transition(GameTime gameTime)
        {
            if (isTransitioning)
            {
                Image.Update(gameTime);
                if (Image.Alpha == 1.0f)
                {
                    currentState.UnloadContent();
                    currentState = newState;
                    xmlGameStateManager.Type = currentState.Type;
                    if (File.Exists(currentState.xmlPath))
                    {
                        currentState = xmlGameStateManager.Load(currentState.xmlPath);
                    }
                    currentState.LoadContent();
                }
                else
                {
                    if (Image.Alpha == 0.0f)
                    {
                        Image.isActive = false;
                        isTransitioning = false;
                    }
                }
            }
        }

        private GameStateManager()
        {
            dimensions = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height); //Laden von XML Datei einfügen
            currentState = new SplashScreen(); //SplashScreen
            xmlGameStateManager = new XmlManager<GameState>();
            xmlGameStateManager.Type = currentState.Type;
            currentState = xmlGameStateManager.Load("Content/Load/SplashScreen.xml"); //SplashScreen.xml
            Exit = false;
        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
            Music.SoundVolume = ((float)MusicVolume) / 100;
            Ingame.SoundVolume = ((float)IngameVolume) / 100;
            Menu.SoundVolume = 1.0f;
            Music.Play2D("Content//Sound//going_coastal.ogg", true);
            currentState.LoadContent();
            Image.LoadContent();
        }

        public void UnloadContent()
        {
            currentState.UnloadContent();
            Image.UnloadContent();
        }

        public void Update(GameTime gameTime)
        {
            if (Fullscreen && !GraphicsDeviceManager.IsFullScreen)
            {
                GraphicsDeviceManager.ToggleFullScreen();
            }

            if (!Fullscreen && GraphicsDeviceManager.IsFullScreen)
            {
                GraphicsDeviceManager.ToggleFullScreen();
            }

            dimensions = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height); //Laden von XML Datei einfügen
            GraphicsDeviceManager.PreferredBackBufferWidth = (int)GameStateManager.Instance.dimensions.X;
            GraphicsDeviceManager.PreferredBackBufferHeight = (int)GameStateManager.Instance.dimensions.Y;

            Image.Scale = new Vector2((float)GraphicsDeviceManager.PreferredBackBufferWidth / (float)Image.Texture.Width,
                            (float)GraphicsDeviceManager.PreferredBackBufferHeight / (float)Image.Texture.Height);

            currentState.Update(gameTime);
            Transition(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentState.Draw(spriteBatch);
            if (isTransitioning)
            {
                Image.Draw(spriteBatch);
            }
        }
    }
}
