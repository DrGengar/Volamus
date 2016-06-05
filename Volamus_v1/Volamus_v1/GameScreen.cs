﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;

namespace Volamus_v1
{
    public class GameScreen : GameState
    {
        Field field;
        Player player_one, player_two;
        Collision collision;

        //SplitScreen
        Viewport defaultView, leftView, rightView;

        private void Initialize()
        {
            content = GameStateManager.Instance.Content;

            field = new Field(50, 100, 15);

            player_one = new Player(new Vector3(0, -25, 0), 5, 0.5f, 0.8f, field);
            player_two = new Player(new Vector3(0, 25, 0), 5, 0.5f, 0.8f, field);

            field.Initialize();

            defaultView = GameStateManager.Instance.GraphicsDevice.Viewport;
            leftView = defaultView;
            rightView = defaultView;
            leftView.Width = leftView.Width / 2;
            rightView.Width = rightView.Width / 2;
            rightView.X = leftView.Width + 1;

            collision = new Collision();
            }

        public override void LoadContent()
        {
            Initialize();

            field.LoadContent();

            player_one.LoadContent();
            player_two.LoadContent();

            Ball.Instance.LoadContent();
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {

            collision.CollisionMethod(player_one);
            collision.CollisionMethod(player_two);

            collision.CollisionMethod(field, player_one, player_two);

            player_one.Update(field, Keys.W, Keys.S, Keys.A, Keys.D, Keys.Space,Keys.Q,Keys.E,Keys.Left, Keys.Right);
            player_two.Update(field, Keys.I, Keys.K, Keys.J, Keys.L, Keys.Enter,Keys.U,Keys.O, Keys.D9,Keys.D0 );

            Ball.Instance.Update(player_one,field);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GameStateManager.Instance.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            GameStateManager.Instance.GraphicsDevice.Clear(Color.CornflowerBlue);

            GameStateManager.Instance.GraphicsDevice.Viewport = leftView;
            field.Draw(player_one.Camera);
            Ball.Instance.Draw(player_one.Camera);
            player_one.Draw(player_one.Camera);
            player_two.Draw(player_one.Camera);
            GameStateManager.Instance.SpriteBatch.DrawString(player_one.Font, player_one.Points.ToString(), 
                new Vector2(leftView.Width/2, 0), Color.Black);

            GameStateManager.Instance.GraphicsDevice.Viewport = rightView;
            field.Draw(player_two.Camera);
            Ball.Instance.Draw(player_two.Camera);
            player_one.Draw(player_two.Camera);
            player_two.Draw(player_two.Camera);
            GameStateManager.Instance.SpriteBatch.DrawString(player_two.Font, player_two.Points.ToString(), 
                new Vector2(leftView.Width + rightView.Width/2, 0), Color.Black);


            GameStateManager.Instance.GraphicsDevice.Viewport = defaultView;

            base.Draw(spriteBatch);
        }
        
    }
}