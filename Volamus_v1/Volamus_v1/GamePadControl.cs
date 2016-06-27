using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace Volamus_v1
{
    public class GamePadControl
    {
        public Buttons Up;
        public Buttons Down;
        public Buttons Left;
        public Buttons Right;
        public Buttons Jump;
        public Buttons Dir_Left;
        public Buttons Dir_Right;
        public Buttons Weak;
        public Buttons Strong;

        [ContentSerializerIgnore]
        public GamePadState oldstate;

        [ContentSerializerIgnore]
        public GamePadState newstate;

        public String XmlPath;
        public PlayerIndex Index;

        public GamePadControl()
        {
            XmlPath = "Content/Load/GamePadStandard.xml";
        }

        public GamePadControl(PlayerIndex i)
        {
            Index = i;
            XmlPath = "Content/Load/GamePad" + (i.ToString()) + ".xml";
        }

        public void Initialize()
        {
            if (!File.Exists(XmlPath))
            {
                GamePadControl standard = XmlIntermediatemanager.Deserialize<GamePadControl>("Content/Load/GamePadStandard.xml");
                standard.Index = Index;
                XmlIntermediatemanager.Serialize<GamePadControl>(XmlPath, standard);
            }
        }

        public GamePadControl LoadContent()
        {
            return XmlIntermediatemanager.Deserialize<GamePadControl>(XmlPath);
        }

        public void UnloadContent() { }

        public void Update(GameTime gameTime) { }

        public bool IsButtonDown(Buttons key)
        {
            return GamePad.GetState(Index).IsButtonDown(key);
        }

        public bool IsButtonUp(Buttons key)
        {
            return GamePad.GetState(Index).IsButtonUp(key);
        }
    }
}
