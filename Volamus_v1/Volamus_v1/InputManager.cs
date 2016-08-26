using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Volamus_v1
{
    public class InputManager
    {
        GamePadState currentButtonState, prevButtonState;
        KeyboardState currentKeyState, prevKeyState;

        private static InputManager instance;

        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }

                return instance;
            }
        }

        public void Update()
        {
            prevKeyState = currentKeyState;
            prevButtonState = currentButtonState;
            if (!GameStateManager.Instance.isTransitioning)
            {
                currentKeyState = Keyboard.GetState();
                currentButtonState = GamePad.GetState(PlayerIndex.One);
            }
        }

        public bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key) && prevKeyState.IsKeyUp(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyUp(key) && prevKeyState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool KeyDown(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (currentKeyState.IsKeyDown(key))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonPressed(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (currentButtonState.IsButtonDown(button) && prevButtonState.IsButtonUp(button))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonReleased(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (currentButtonState.IsButtonUp(button) && currentButtonState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        public bool ButtonDown(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (currentButtonState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
