﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Volamus_v1
{
    public class Image
    {
        public float Alpha;
        public string Text, FontName, Path;
        public Vector2 Position, Scale;
        public Rectangle SourceRect;
        public bool isActive;
        public Color Color;

        [XmlIgnore]
        public Texture2D Texture;
        Vector2 origin;
        ContentManager content;
        RenderTarget2D renderTarget;
        SpriteFont font;
        Dictionary<string, ImageEffect> effectList;
        public string Effects;

        public FadeEffect FadeEffect;

        void SetEffect<T>(ref T effect)
        {
            if (effect == null)
            {
                effect = (T)Activator.CreateInstance(typeof(T));
            }
            else
            {
                (effect as ImageEffect).isActive = true;
                var obj = this;
                (effect as ImageEffect).LoadContent(ref obj);
            }

            effectList.Add(effect.GetType().ToString().Replace("Volamus_v1.", ""), (effect as ImageEffect));
        }

        public void ActivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].isActive = true;
                var obj = this;
                effectList[effect].LoadContent(ref obj);
            }
        }

        public void DeactivateEffect(string effect)
        {
            if (effectList.ContainsKey(effect))
            {
                effectList[effect].isActive = false;
                effectList[effect].UnloadContent();
            }
        }

        public void StoreEffects()
        {
            Effects = String.Empty;
            foreach (var effect in effectList)
            {
                if (effect.Value.isActive)
                {
                    Effects += effect.Key + ":";
                }

                if (Effects != String.Empty)
                {
                    Effects.Remove(Effects.Length - 1);
                }
            }
        }

        public void RestoreEffects()
        {
            foreach (var effect in effectList)
            {
                DeactivateEffect(effect.Key);
            }

            string[] split = Effects.Split(':');
            foreach (string s in split)
            {
                ActivateEffect(s);
            }
        }

        public Image()
        {
            Path = Text = Effects = String.Empty;
            FontName = "SpriteFonts/Standard";
            Position = Vector2.Zero;
            Scale = Vector2.One;
            Alpha = 1.0f;
            Color = Color.White;
            SourceRect = Rectangle.Empty;
            effectList = new Dictionary<string, ImageEffect>();
        }

        public void LoadContent()
        {
            content = new ContentManager(GameStateManager.Instance.Content.ServiceProvider, "Content");

            if (Path != String.Empty)
            {
                Texture = GameStateManager.Instance.Content.Load<Texture2D>(Path);
            }

            font = content.Load<SpriteFont>(FontName);

            Vector2 dimensions = Vector2.Zero;

            if (Texture != null)
            {
                dimensions.X += Texture.Width;
            }
            else
            {
                dimensions.X += font.MeasureString(Text).X;
            }

            if (Texture != null)
            {
                dimensions.Y = Math.Max(Texture.Height, font.MeasureString(Text).Y);
            }
            else
            {
                dimensions.Y = font.MeasureString(Text).Y;
            }

            if (SourceRect == Rectangle.Empty)
            {
                SourceRect = new Rectangle(0, 0, (int)dimensions.X, (int)dimensions.Y);
            }

            renderTarget = new RenderTarget2D(GameStateManager.Instance.GraphicsDevice, (int)dimensions.X, (int)dimensions.Y);

            GameStateManager.Instance.GraphicsDevice.SetRenderTarget(renderTarget);
            GameStateManager.Instance.GraphicsDevice.Clear(Color.Transparent);
            GameStateManager.Instance.SpriteBatch.Begin();
            if (Texture != null)
            {
                GameStateManager.Instance.SpriteBatch.Draw(Texture, Vector2.Zero, Color.White);
                GameStateManager.Instance.SpriteBatch.DrawString(font, Text, new Vector2((dimensions.X - font.MeasureString(Text).X) / 2, (dimensions.Y - font.MeasureString(Text).Y) / 2), Color.White);
            }
            else
            {
                GameStateManager.Instance.SpriteBatch.DrawString(font, Text, Vector2.Zero, Color.White);
            }
            GameStateManager.Instance.SpriteBatch.End();

            Texture = renderTarget;

            GameStateManager.Instance.GraphicsDevice.SetRenderTarget(null);

            SetEffect<FadeEffect>(ref FadeEffect);

            if (Effects != String.Empty)
            {
                string[] split = Effects.Split(':');
                foreach (string item in split)
                {
                    ActivateEffect(item);
                }
            }
        }

        public void UnloadContent()
        {
            content.Unload();
            foreach (var effect in effectList)
            {
                DeactivateEffect(effect.Key);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var effect in effectList)
            {
                if (effect.Value.isActive)
                {
                    effect.Value.Update(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            origin = new Vector2(SourceRect.Width / 2, SourceRect.Height / 2);

            spriteBatch.Draw(Texture, Position + origin, SourceRect, Color * Alpha, 0.0f, origin, Scale, SpriteEffects.None, 0.0f);
        }
    }
}
