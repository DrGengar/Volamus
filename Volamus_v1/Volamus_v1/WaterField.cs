using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class WaterField : Field 
    {
        Ocean ocean;
        Texture2D skyTexture;

        public WaterField(int w, int l, int n_h) : base(w, l, n_h){}

        public new void LoadContent()
        {
            skyTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/himmelrot");
            skydome = new Skydome(25f, false, skyTexture);
            skydome.Load();

            netTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/iceTexture");

            ocean = new Ocean();
            ocean.LoadContent();

            base.LoadContent();
        }

        public new void Draw(Camera camera)
        {
            ocean.Draw(GameStateManager.Instance.GameTime, camera, skyTexture, new Vector3(0, 0, 0));
            base.Draw(camera);
        }
    }
}
