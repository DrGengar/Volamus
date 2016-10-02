using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Spectator
    {
        public bool Cheering
        {
            get { return cheering; }
            set { cheering = value; }
        }

        public Model model;
        public Texture texture;

        public Effect effect;

        public Vector3 position, scale;

        public bool is_falling, cheering;

        public Spectator(Vector3 pos, Vector3 s)
        {
            position = pos;
            scale = s;
            is_falling = cheering = false;

        }

        public void LoadContent()
        {
            effect = GameStateManager.Instance.Content.Load<Effect>("Effects/shader");
        }

        public void UnloadContent()
        {

        }

        public void Update()
        {

        }

        public void Draw(Camera camera, int rotation)
        {

        }
    }
}
