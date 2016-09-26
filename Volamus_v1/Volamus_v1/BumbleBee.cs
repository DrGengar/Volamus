using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class BumbleBee : Player
    {
        public BumbleBee(Vector3 pos, int m_j_height, float j_velo, float mvp, Field field) : base(pos, m_j_height, j_velo, mvp, field){ }

        public BumbleBee(Vector3 pos, int m_j_height, float j_velo, float mvp, Field field, PlayerIndex i) : base(pos, m_j_height, j_velo, mvp, field, i) { }

        public new void LoadContent()
        {
            Texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/pinguinUV");
            model = GameStateManager.Instance.Content.Load<Model>("Models/pinguin");
            base.LoadContent();
        }
    }
}
