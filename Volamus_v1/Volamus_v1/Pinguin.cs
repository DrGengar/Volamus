using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Pinguin : Player
    {
        bool is_falling;
        public Pinguin(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field) : base(pos, ma_j_height, mi_j_height, j_velo, mvp, field){}

        public Pinguin(Vector3 pos, int ma_j_height, int mi_j_height, float j_velo, float mvp, Field field, PlayerIndex i) : base(pos, ma_j_height, mi_j_height, j_velo, mvp, field, i) { }

        public new void LoadContent()
        {
            is_falling = false;
            Texture = GameStateManager.Instance.Content.Load<Texture2D>("Textures/pinguinUV");
            model = GameStateManager.Instance.Content.Load<Model>("Models/pinguin");
            CreateBoundingBoxes();
            base.LoadContent();
        }

        public void CheeringP()
        {
            BettaP = 0;
            Gamma = 0;

            HitAngleLeft = 90;
            HitAngleRight = 90;

            if (GameScreen.Instance.Match.Winner == this)
            {
                //Flügel gehen nach oben
                HitAngleHigh = 20;

                //hüpft
                if (Position.Z < 6.5 && !is_falling)
                {
                    Position = Position + new Vector3(0, 0, 0.5f);
                }
                else
                {
                    if (Position.Z > 0)
                    {
                        Position = Position - new Vector3(0, 0, 0.5f);
                        is_falling = true;
                    }
                    else
                    {
                        is_falling = false;
                    }
                }

                leftWingPos = new Vector3(Position.X - 2, Position.Y, Position.Z - 1);
                rightWingPos = new Vector3(Position.X + 2, Position.Y, Position.Z - 1);
            }
            else
            {
                PositionLeftWing = new Vector3(Position.X + 5, Position.Y, Position.Z + 2);
                PositionRightWing = new Vector3(Position.X - 5, Position.Y, Position.Z + 2);
                HitAngleHigh = -40;
            }
        }


        private void CreateBoundingBoxes()
        {

            Vector3 min = new Vector3(-2.9f, -2.9f, 0);
            Vector3 max = new Vector3(2.9f, 2.9f, 12);

            Vector3 mid = new Vector3((max.X + min.X) / 2, (Direction) * (max.Y + min.Y) / 2, min.Z);
            Vector3 translate = mid - Position;

            min.X -= translate.X;
            max.X -= translate.X;

            min.Y += Position.Y;
            max.Y += Position.Y;

            min.Z -= translate.Z;
            max.Z -= translate.Z;

            innerBoundingBox = new BoundingBox(min, max);

            //äußere BoundingBox
            Vector3 offset = new Vector3(1.5f * Ball.Instance.BoundingSphere.Radius, 1.5f * Ball.Instance.BoundingSphere.Radius, 0);
            outerBoundingBox = new BoundingBox((innerBoundingBox.Min - offset),
                innerBoundingBox.Max + offset);
            outerBoundingBox.Max.Z += 1.5f * Ball.Instance.BoundingSphere.Radius;
        }
    }
}
