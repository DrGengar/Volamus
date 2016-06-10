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
        private int touch_count; //Maximale Anzahl an ballberührungen hintereinander von einer Person ; TODO: Noch zu implementieren
        private Parabel newParabel;

        private static Collision instance;


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
            touch_count = 0;
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

                    if(lastTouched.Direction*Ball.Instance.Position.Y < min)
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

            BallWithOuterBoundingBox(lastTouched);
            BallWithOuterBoundingBox(lastTouched.Enemy);

            BallWithInnerBoundingBox(lastTouched);
            BallWithInnerBoundingBox(lastTouched.Enemy);

            //Ball mit Netz
            if (Ball.Instance.BoundingSphere.Intersects(field.NetBoundingBox) && (newParabel == null || !(newParabel.Equals(Ball.Instance.Active))))
            {
                //Neue Flugbahn mit Ausfallwinkel = Einfallswinkel, x - Ebene
                newParabel = new Parabel(Ball.Instance.Position, Ball.Instance.Active.Angles.X, Ball.Instance.Active.Angles.Z, Ball.Instance.Active.Angles.Y, Ball.Instance.Active.Velocity - 5, Ball.Instance.Active.Direction*(-1));
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
            return player.InnerBoundingBox.Intersects(field.NetBoundingBox);
        }

        //Spieler mit Feldgrenzen
        public bool PlayerWithField(Player player, Field field)
        {
            return player.InnerBoundingBox.Intersects(field.BoundingBox);
        }

        private void BallWithInnerBoundingBox(Player player)
        {
            //Ball mit InnerBoundingBox vom Spieler -> Ball soll abprallen vom Spieler
            if (Ball.Instance.BoundingSphere.Intersects(player.InnerBoundingBox) && Ball.Instance.IsFlying == true && !player.IsServing)
            {
                //Kollision
                newParabel = new Parabel(Ball.Instance.Position, Ball.Instance.Active.Angles.X, Ball.Instance.Active.Angles.Z, Ball.Instance.Active.Angles.Y, Ball.Instance.Active.Velocity, Ball.Instance.Active.Direction * (-1));
                Ball.Instance.Active = newParabel;
                player.CanHit = false;

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
