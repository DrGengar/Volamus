﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Volamus_v1
{
    public class Menu
    {
        public event EventHandler OnMenuChange;

        public string Axis;
        public string Effects;
        [XmlElement("Item")]
        public List<MenuItem> Items;

        int itemNumber;
        string id;

        public int ItemNumber
        {
            get { return itemNumber; }
        }

        public string ID
        {
            get { return id; }

            set
            {
                id = value;
                OnMenuChange(this, null);
            }
        }

        public void Transition(float alpha)
        {
            foreach (MenuItem item in Items)
            {
                item.Image.isActive = true;
                item.Image.Alpha = alpha;
                if (alpha == 0.0f)
                {
                    item.Image.FadeEffect.Increase = true;
                }
                else
                {
                    item.Image.FadeEffect.Increase = false;
                }
            }
        }

        void AlignMenuItems()
        {
            Vector2 dimensions = Vector2.Zero;
            foreach (MenuItem item in Items)
            {
                dimensions += new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height);
            }

            dimensions = new Vector2((GameStateManager.Instance.dimensions.X - dimensions.X) / 2,
                (GameStateManager.Instance.dimensions.Y - dimensions.Y) / 2);

            foreach (MenuItem item in Items)
            {
                if (Axis == "X")
                {
                    item.Image.Position = new Vector2(dimensions.X, (GameStateManager.Instance.dimensions.Y - item.Image.SourceRect.Height) / 2);
                }
                else
                {
                    if (Axis == "Y")
                    {
                        item.Image.Position = new Vector2((GameStateManager.Instance.dimensions.X - item.Image.SourceRect.Width) / 2,
                        dimensions.Y);
                    }
                }

                dimensions += new Vector2(item.Image.SourceRect.Width, item.Image.SourceRect.Height);
            }
        }

        public Menu()
        {
            id = String.Empty;
            itemNumber = 0;
            Effects = String.Empty;
            Axis = "Y";
            Items = new List<MenuItem>();
        }

        public void LoadContent()
        {
            string[] split = Effects.Split(':');
            foreach (MenuItem item in Items)
            {
                item.Image.LoadContent();
                foreach (string s in split)
                {
                    item.Image.ActivateEffect(s);
                }
            }
            AlignMenuItems();
        }

        public void UnloadContent()
        {
            foreach (MenuItem item in Items)
            {
                item.Image.UnloadContent();
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Axis == "X")
            {
                if (InputManager.Instance.KeyPressed(Keys.Right) || InputManager.Instance.ButtonPressed(Buttons.DPadRight, Buttons.LeftThumbstickRight))
                {
                    GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);
                    itemNumber++;
                }
                else
                {
                    if (InputManager.Instance.KeyPressed(Keys.Left) || InputManager.Instance.ButtonPressed(Buttons.DPadLeft, Buttons.LeftThumbstickLeft))
                    {
                        GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);
                        itemNumber--;
                    }
                }
            }
            else
            {
                if (Axis == "Y")
                {
                    if (InputManager.Instance.KeyPressed(Keys.Down) || InputManager.Instance.ButtonPressed(Buttons.DPadDown, Buttons.LeftThumbstickDown))
                    {
                        GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);
                        itemNumber++;
                    }
                    else
                    {
                        if (InputManager.Instance.KeyPressed(Keys.Up) || InputManager.Instance.ButtonPressed(Buttons.DPadUp, Buttons.LeftThumbstickUp))
                        {
                            GameStateManager.Instance.Menu.Play2D("Content//Sound//button.ogg", false);
                            itemNumber--;
                        }
                    }
                }
            }

            if (itemNumber < 0)
            {
                itemNumber = 0;
            }
            else
            {
                if (itemNumber > Items.Count - 1)
                {
                    itemNumber = Items.Count - 1;
                }
            }

            for (int i = 0; i < Items.Count; i++)
            {
                if (i == itemNumber)
                {
                    Items[i].Image.isActive = true;
                }
                else
                {
                    Items[i].Image.isActive = false;
                }

                Items[i].Image.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MenuItem item in Items)
            {
                item.Image.Draw(spriteBatch);
            }
        }
    }
}
