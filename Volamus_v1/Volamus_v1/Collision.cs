using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Volamus_v1
{
    public class Collision
    {
        private Player lastTouched;
        private Parabel newParabel;

        private static Collision instance;
        private int colliding;


        public static Collision Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Collision();
                }

                return instance;
            }
        }

        public Player LastTouched
        {
            get
            {
                return lastTouched;
            }

            set
            {
                lastTouched = value;
            }
        }

        //Constructor
        private Collision()
        {
        }

        public void CollisionMethod(Field field)
        {
            //Ball mit Ebene z=0
            if (BallWithPlane())
            {
                Ball.Instance.IsFlying = false;
                //Wenn außerhalb des Feldes: Gegner von LastTouched +1 Punkt und bekommt Aufschlag
                if(Ball.Instance.Position.X > (field.Width/2) || Ball.Instance.Position.X < -(field.Width / 2) || Ball.Instance.Position.Y > (field.Length/2) || Ball.Instance.Position.Y < -(field.Length / 2))
                {
                    lastTouched.Enemy.Points += 1;
                    lastTouched.Enemy.IsServing = true;
                }
                //Sonst muss der ball im Feld gelandet sein -> Unterscheidung eigenes (gegner bekommt Punkt)/gegnerisches feld(ich selbst bekomme Punkt)
                else
                {
                    float min = lastTouched.Box[1].Y;

                    if (lastTouched.Direction*Ball.Instance.Position.Y < min)
                    {
                        lastTouched.Enemy.Points += 1;
                        lastTouched.Enemy.IsServing = true;
                    }
                    else
                    {
                        lastTouched.Points += 1;
                        lastTouched.IsServing = true;
                    }
                }
            }

            if(!Ball.Instance.BoundingSphere.Intersects(lastTouched.InnerBoundingBox) && !Ball.Instance.BoundingSphere.Intersects(lastTouched.Enemy.InnerBoundingBox) && !Ball.Instance.BoundingSphere.Intersects(field.NetBoundingBox))
            {
                colliding = 0;
            }

            BallWithOuterBoundingBox(lastTouched);
            BallWithOuterBoundingBox(lastTouched.Enemy);

            BallWithInnerBoundingBox(lastTouched);
            BallWithInnerBoundingBox(lastTouched.Enemy);

            //Ball mit Netz
            if (Ball.Instance.BoundingSphere.Intersects(field.NetBoundingBox) && colliding == 0)
            {
                //Neue Flugbahn mit Ausfallwinkel = Einfallswinkel, x - Ebene
                colliding = 2;

                Vector3 hitdirection = Ball.Instance.Active.Hit_Direction;
                float angle_z = MathHelper.ToDegrees((float)Math.Atan((hitdirection.Z / hitdirection.Y)));
                float angle_x = MathHelper.ToDegrees((float)Math.Atan((hitdirection.X / hitdirection.Y)));
                newParabel = new Parabel(Ball.Instance.Position, lastTouched.Direction*(-angle_z), angle_x, Ball.Instance.Active.Angles.Y, 0.6f * Ball.Instance.Active.Velocity, 
                    Ball.Instance.Active.Direction * (-1)); //Skalierung
                Ball.Instance.Active = newParabel;
            }
        }

        //Ball mit Ebene z=0
        public bool BallWithPlane()
        {
            if(Ball.Instance.BoundingSphere.Intersects(new Plane(new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(0,1,0))) != (PlaneIntersectionType) 0)
            {
                return true;
            }

            return false;
        }

        //Spieler mit Netz
        public bool PlayerWithNet(Player player, Field field)
        {

            return player.OuterBoundingBox.Intersects(field.NetBoundingBox);
        }

        //Spieler mit Feldgrenzen
        public bool PlayerWithField(Player player, Field field)
        {
            return player.InnerBoundingBox.Intersects(field.BoundingBox);
        }

        private void BallWithInnerBoundingBox(Player player)
        {
            //Ball mit InnerBoundingBox vom Spieler -> Ball soll abprallen vom Spieler
            if (Ball.Instance.BoundingSphere.Intersects(player.InnerBoundingBox) && Ball.Instance.IsFlying == true && !player.IsServing && colliding == 0)
            {
                if(!player.Equals(lastTouched))
                {
                    lastTouched.Touch_count = 0;
                }

                Vector3 hitdirection = Ball.Instance.Active.Hit_Direction;
                float angle_z = MathHelper.ToDegrees((float)Math.Atan((hitdirection.Z / hitdirection.Y)));
                float angle_x = MathHelper.ToDegrees((float)Math.Atan((hitdirection.X / hitdirection.Y)));

                colliding = player.Direction;

                float left = 0.0f;
                float right = 0.0f;
                float front = 0.0f;
                float back = 0.0f;

                if (player.Direction == 1)
                {
                    left = player.InnerBoundingBox.Min.X;
                    right = player.InnerBoundingBox.Max.X;
                    front = player.InnerBoundingBox.Max.Y;
                    back = player.InnerBoundingBox.Min.Y;
                }
                else if(player.Direction == -1)
                {
                    left = player.InnerBoundingBox.Min.X;
                    right = player.InnerBoundingBox.Max.X;
                    front = player.InnerBoundingBox.Min.Y;
                    back = player.InnerBoundingBox.Max.Y;
                }

                //Kollision von vorne
                if (player.Direction * Ball.Instance.Position.Y >= player.Direction * front)
                {
                    newParabel = new Parabel(Ball.Instance.Position, player.Direction * (-angle_z), angle_x, Ball.Instance.Active.Angles.Y, 0.75f * Ball.Instance.Active.Velocity,
                        Ball.Instance.Active.Direction * (-1)); //Skalierung
                }

                //Kollision von der Seite
                //Links
                if(Ball.Instance.Position.X < left)
                {
                    //Links
                    newParabel = new Parabel(Ball.Instance.Position, player.Direction * (-angle_z), player.Direction * angle_x, Ball.Instance.Active.Angles.Y, 0.75f * Ball.Instance.Active.Velocity,
                        Ball.Instance.Active.Direction); //Skalierung
                }

                if (Ball.Instance.Position.X > right)
                {
                    //Rechts
                    newParabel = new Parabel(Ball.Instance.Position, player.Direction * (-angle_z), player.Direction * angle_x, Ball.Instance.Active.Angles.Y, 0.75f * Ball.Instance.Active.Velocity,
                        Ball.Instance.Active.Direction); //Skalierung
                }
                
                //Kollision wenn von oben
                if (Ball.Instance.Position.Z >= player.InnerBoundingBox.Max.Z)
                {
                    newParabel = new Parabel(Ball.Instance.Position, player.Direction * angle_z, angle_x, Ball.Instance.Active.Angles.Y, 0.75f * Ball.Instance.Active.Velocity,
                        Ball.Instance.Active.Direction); //Skalierung
                }

                Ball.Instance.Active = newParabel;
                player.CanHit = false;
                lastTouched = player;

                lastTouched.Touch_count++;

                Ball.Instance.Update();
            }
        }

        private void BallWithOuterBoundingBox(Player player)
        {
            //Ball mit OuterBoundingBox vom Spieler -> Ball kann jetzt geschlagen werden vom Spieler
            if (Ball.Instance.BoundingSphere.Intersects(player.OuterBoundingBox) && (player.IsServing || Ball.Instance.IsFlying))
            {
                //Kollision
                player.CanHit = true;
            }
            else
            {
                player.CanHit = false;
            }
        }
    }
}
