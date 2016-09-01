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
    public class KeyboardControl
    {
        public Keys Up;
        public Keys Down;
        public Keys Left;
        public Keys Right;
        public Keys Jump;
        public Keys Dir_Left;
        public Keys Dir_Right;
        public Keys Weak;
        public Keys Strong;

        //[ContentSerializerIgnore]
        public KeyboardState oldstate;

        //[ContentSerializerIgnore]
        public KeyboardState newstate;

        public String XmlPath;

        public KeyboardControl()
        {
            XmlPath = "Content/Load/Keyboard.xml";

            if (!File.Exists(XmlPath))
            {
                KeyboardControl standard = XmlIntermediatemanager.Deserialize<KeyboardControl>("Content/Load/KeyboardStandard.xml");
                XmlIntermediatemanager.Serialize<KeyboardControl>(XmlPath, standard);
            }
        }

        public KeyboardControl LoadContent()
        {
            return XmlIntermediatemanager.Deserialize<KeyboardControl>(XmlPath);
        }

        public void UnloadContent() { }

        public void Update(GameTime gameTime) { }

        public bool IsKeyDown(Keys key)
        {
           return Keyboard.GetState().IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return Keyboard.GetState().IsKeyUp(key);
        }
    }
}
