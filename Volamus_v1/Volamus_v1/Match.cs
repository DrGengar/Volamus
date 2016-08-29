using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Match
    {
        public Player PlayerOne
        {
            get { return One; }
        }

        public Player PlayerTwo
        {
            get { return Two; }
        }

        public Field Field
        {
            get { return field; }
        }

        public Wind Wind
        {
            get { return wind; }
        }

        Player One, Two;
        Field field;
        Wind wind;
        bool change_size;
        bool change_velocity;

        //Ball.Instance
        //Collision.Instance;

        public Match(Player one, Player two, Field f, int w, bool c_s, bool c_v)
        {
            One = one;
            Two = two;
            field = f;
            wind = new Wind(w);
            change_size = c_s;
            change_velocity = c_v;
        }

        public void LoadContent()
        {
            One.LoadContent();
            Two.LoadContent();

            field.LoadContent();

            //skydome.Load();

            Ball.Instance.LoadContent(wind);
        }

        public void Unloadcontent()
        {
            /*One.UnloadContent();
            Two.UnloadContent();
            field.UnloadContent();
            Ball.Instance.UnloadContent();*/
        }

        public void Update(GameTime gameTime)
        {
            One.Update(field);
            Two.Update(field);

            Ball.Instance.Update();
        }
    }
}
