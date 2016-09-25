using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Volamus_v1
{
    public class Collision
    {
        private Player lastTouched;
        private Parabel newParabel;

        private static Collision instance;
        private int colliding;
        private int groundContact;   //Bodenberührungen des Balls
        private bool collision_with_net;

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

        private Collision()
        {
        }

        public void UnloadContent()
        {
            instance = null;
        }

        public void CollisionMethod(Field field)
        {
            if (GameScreen.Instance.Match.IsFinished != true)
            {
                BallWithOuterBoundingBox(lastTouched);
                BallWithOuterBoundingBox(lastTouched.Enemy);

                BallWithInnerBoundingBox(lastTouched);
                BallWithInnerBoundingBox(lastTouched.Enemy);

                //Ball mit Ebene z=0
                if (BallWithPlane())
                {
                    //beim ersten Bodenkontakt werden die Punkte,... angepasst, Sound,..
                    if (groundContact == 0)
                    {
                        //GameStateManager.Instance.Ingame.SoundVolume = 0.3f;
                        GameStateManager.Instance.Ingame.Play2D("Content//Sound//single_blow_from_police_whistle.ogg");


                        //GameStateManager.Instance.Ingame.SoundVolume = 0.3f;
                        GameStateManager.Instance.Ingame.Play2D("Content//Sound//child_crowd_cheering.ogg");


                        //Wenn außerhalb des Feldes: Gegner von LastTouched +1 Punkt und bekommt Aufschlag
                        if (Ball.Instance.Position.X > (field.Width / 2) || Ball.Instance.Position.X < -(field.Width / 2) || Ball.Instance.Position.Y > (field.Length / 2) || Ball.Instance.Position.Y < -(field.Length / 2))
                        {

                            lastTouched.Enemy.Points += 1;
                            lastTouched.Enemy.IsServing = true;
                            lastTouched.CanHit = false;
                            

                            lastTouched.Touch_Count = 0;
                            lastTouched.Enemy.Touch_Count = 0;
                        }
                        //Sonst muss der ball im Feld gelandet sein -> Unterscheidung eigenes (gegner bekommt Punkt)/gegnerisches feld(ich selbst bekomme Punkt)
                        else
                        {
                            float min = lastTouched.Box[1].Y;

                            if (lastTouched.Direction * Ball.Instance.Position.Y < min)
                            {

                                lastTouched.Enemy.Points += 1;
                                lastTouched.Enemy.IsServing = true;
                                lastTouched.CanHit = false;

                                lastTouched.Touch_Count = 0;
                                lastTouched.Enemy.Touch_Count = 0;
                            }
                            else
                            {
                                lastTouched.Points += 1;
                                lastTouched.IsServing = true;
                                lastTouched.Enemy.CanHit = false;

                                lastTouched.Touch_Count = 0;
                                lastTouched.Enemy.Touch_Count = 0;
                            }
                        }

                    }

                    //Hüpfen des Balls -> Überarbeiten
                    if (groundContact <= 1 && !GameScreen.Instance.Match.IsFinished)
                    {
                        Vector3 hitdirection = Ball.Instance.Active.Hit_Direction;
                        float angle_z = MathHelper.ToDegrees((float)Math.Atan((hitdirection.Z / hitdirection.Y)));
                        float angle_x = MathHelper.ToDegrees((float)Math.Atan((hitdirection.X / hitdirection.Y)));
                        Ball.Instance.Position = new Vector3(Ball.Instance.Position.X, Ball.Instance.Position.Y, Ball.Instance.Position.Z + 0.1f);

                        newParabel = new Parabel(Ball.Instance.Position, Ball.Instance.Active.Direction * (-angle_z), Ball.Instance.Active.Direction * angle_x, Ball.Instance.Active.Angles.Y, 0.75f * Ball.Instance.Active.Velocity,
                            Ball.Instance.Active.Direction);

                        if(collision_with_net)
                        {
                            newParabel = new Parabel(Ball.Instance.Position, lastTouched.Direction * (angle_z), lastTouched.Direction * (-angle_x), Ball.Instance.Active.Angles.Y, 0.6f * Ball.Instance.Active.Velocity,
                            Ball.Instance.Active.Direction);
                            collision_with_net = false;
                        }

                        Ball.Instance.Active = newParabel;
                        groundContact++;
                    }

                    //zurücksetzen
                    else
                    {
                        //Die Veränderungen durch Drops zurücksetzen
                        Ball.Instance.EffectDrop = 1.0f;
                        Ball.Instance.BoundingSphereRadius = Ball.Instance.OriginalRadius;
                        lastTouched.Movespeed = 0.8f;
                        lastTouched.Enemy.Movespeed = 0.8f;

                        groundContact = 0;
                        Ball.Instance.IsFlying = false;
                    }
                }

                if (!Ball.Instance.BoundingSphere.Intersects(lastTouched.InnerBoundingBox) && !Ball.Instance.BoundingSphere.Intersects(lastTouched.Enemy.InnerBoundingBox) && !Ball.Instance.BoundingSphere.Intersects(field.NetBoundingBox))
                {
                    colliding = 0;
                }

                //Ball mit Netz
                if (Ball.Instance.BoundingSphere.Intersects(field.NetBoundingBox) && colliding == 0)
                {
                    //Neue Flugbahn mit Ausfallwinkel = Einfallswinkel, x - Ebene
                    colliding = 2;

                    Vector3 hitdirection = Ball.Instance.Active.Hit_Direction;
                    float angle_z = MathHelper.ToDegrees((float)Math.Atan((hitdirection.Z / hitdirection.Y)));
                    float angle_x = MathHelper.ToDegrees((float)Math.Atan((hitdirection.X / hitdirection.Y)));
                    newParabel = new Parabel(Ball.Instance.Position, lastTouched.Direction * (angle_z), lastTouched.Direction * angle_x, Ball.Instance.Active.Angles.Y, 0.6f * Ball.Instance.Active.Velocity,
                        Ball.Instance.Active.Direction * (-1)); //Skalierung
                    Ball.Instance.Active = newParabel;
                    collision_with_net = true;
                }
            }
        }

        //Ball mit Ebene z=0
        public bool BallWithPlane()
        {
            if (GameScreen.Instance.Match.IsFinished == false)
            {
                if (Ball.Instance.BoundingSphere.Intersects(new Plane(new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0))) != (PlaneIntersectionType)0)
                {
                    return true;
                }

                return false;
            }

            return false;
        }

        //Spieler mit Netz
        public bool PlayerWithNet(Player player, Field field)
        {
            return player.OuterBoundingBox.Intersects(field.NetBoundingBox);
        }

        //Spieler mit Drop
        public bool PlayerWithDrop(Player p, Drop d)
        {
            if (d.BoundingSphere.Intersects(p.InnerBoundingBox))
            {
                GameStateManager.Instance.Ingame.Play2D("Content//Sound//einsammelSound.ogg");
                return true;
            }

            return false;
        }

        private void BallWithInnerBoundingBox(Player player)
        {
            //Ball mit InnerBoundingBox vom Spieler -> Ball soll abprallen vom Spieler
            if (Ball.Instance.BoundingSphere.Intersects(player.InnerBoundingBox) && colliding == 0)
            {
                Vector3 hitdirection = Ball.Instance.Active.Hit_Direction;

                float angle_z = MathHelper.ToDegrees((float)Math.Atan((hitdirection.Z / hitdirection.Y)));
                float angle_x = MathHelper.ToDegrees((float)Math.Atan((hitdirection.X / hitdirection.Y)));

                if (hitdirection.Y == 0.0f)
                {
                    angle_z = 90.0f;
                    angle_x = 0.0f;
                }

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
                else if (player.Direction == -1)
                {
                    left = player.InnerBoundingBox.Min.X;
                    right = player.InnerBoundingBox.Max.X;
                    front = player.InnerBoundingBox.Min.Y;
                    back = player.InnerBoundingBox.Max.Y;
                }

                if (lastTouched.Enemy == player)
                {
                    lastTouched.Enemy.Touch_Count = 0;
                }

                //Kollision von vorne
                if (player.Direction * Ball.Instance.Position.Y >= player.Direction * front)
                {
                    newParabel = new Parabel(Ball.Instance.Position, player.Direction * (-angle_z), angle_x, Ball.Instance.Active.Angles.Y, 0.75f * Ball.Instance.Active.Velocity,
                        Ball.Instance.Active.Direction * (-1)); //Skalierung
                }

                //Kollision von hinten
                if (player.Direction * Ball.Instance.Position.Y <= player.Direction * back)
                {
                    newParabel = new Parabel(Ball.Instance.Position, player.Direction * (-angle_z), angle_x, Ball.Instance.Active.Angles.Y, 0.75f * Ball.Instance.Active.Velocity,
                        Ball.Instance.Active.Direction); //Skalierung
                }

                //Kollision von der Seite
                //Links
                if (Ball.Instance.Position.X < left)
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
                    newParabel = new Parabel(Ball.Instance.Position, player.Direction * angle_z, player.Direction * (-angle_x), Ball.Instance.Active.Angles.Y, 0.75f * Ball.Instance.Active.Velocity,
                        Ball.Instance.Active.Direction); //Skalierung
                }

                Ball.Instance.Active = newParabel;
                player.CanHit = false;
                lastTouched = player;

                if (player.Control)
                {
                    if (!GamePad.GetState(player.GamePadControl.Index).IsButtonDown(player.GamePadControl.Weak) && !GamePad.GetState(player.GamePadControl.Index).IsButtonDown(player.GamePadControl.Strong))
                    {
                        lastTouched.Touch_Count++;
                    }
                }
                else
                {
                    if (!Keyboard.GetState().IsKeyDown(player.KeyboardControl.Weak) && !Keyboard.GetState().IsKeyDown(player.KeyboardControl.Strong))
                    {
                        lastTouched.Touch_Count++;
                    }
                }
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
