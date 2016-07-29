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
        public ISoundEngine SoundEngine;

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
            newState = (GameState)Activator.CreateInstance(Type.GetType("Volamus_v1." + screenName));
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
            currentState = new GameScreen();
            xmlGameStateManager = new XmlManager<GameState>();
            xmlGameStateManager.Type = currentState.Type;
            currentState = xmlGameStateManager.Load("Content/Load/GameScreen.xml"); //SplashScreen.xml
        }

        public void LoadContent(ContentManager Content)
        {
            this.Content = new ContentManager(Content.ServiceProvider, "Content");
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
