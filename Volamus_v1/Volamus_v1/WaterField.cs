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

        SpectatorGroupDolphin groupOne, groupTwo;

        public SpectatorGroupDolphin GroupOne
        {
            get { return groupOne; }
        }

        public SpectatorGroupDolphin GroupTwo
        {
            get { return groupTwo; }
        }

        public WaterField(int w, int l, int n_h, Random rnd) : base(w, l, n_h)
        {
            groupOne = new SpectatorGroupDolphin(new Vector3(-w / 2 - 10, -l / 2 - 5, 0), 5, rnd);
            groupTwo = new SpectatorGroupDolphin(new Vector3(w / 2 + 10, l / 2 + 5, 0), 5, rnd);
        }

        public new void LoadContent()
        {
            groupOne.LoadContent();
            groupTwo.LoadContent();

            skyTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/himmelrot");
            skydome = new Skydome(25f, false, skyTexture);
            skydome.Load();

            netTexture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/netTextureSeaWeed");

            ocean = new Ocean();
            ocean.LoadContent();

            base.LoadContent();
        }

        public void UnloadContent()
        {
            groupOne.UnloadContent();
            groupTwo.UnloadContent();
        }

        public void Update(Random rnd)
        {
            groupOne.Update();
            groupTwo.Update();
        }

        public new void Draw(Camera camera)
        {
            ocean.Draw(GameStateManager.Instance.GameTime, camera, skyTexture, new Vector3(0, 0, -0.05f));
            base.Draw(camera);

            groupOne.Draw(camera);
            groupTwo.Draw(camera);

        }
    }
}
