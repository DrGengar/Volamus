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
        Player lasttouched;

        public bool CollisionMethod()
        {
            if(Ball.Instance.BoundingSphere.Intersects(new Plane(new Vector3(0,0,0), new Vector3(1,0,0), new Vector3(0,1,0))) != (PlaneIntersectionType) 0)
            {
                return true;
            }

            return false;
        }

        public bool CollisionMethod(Field field, Player One, Player Two)
        {
            

            return false;
        }

        public bool CollisionMethod(Player player)
        {
            Model c1 = player.Model;
            Model c2 = Ball.Instance.Model;

            for (int i = 0; i < c1.Meshes.Count; i++)
            {
                // Check whether the bounding boxes of the two cubes intersect.
                BoundingSphere c1BoundingSphere = c1.Meshes[i].BoundingSphere;
                c1BoundingSphere.Radius *= 0.075f;
                c1BoundingSphere.Center += player.Position;

                for (int j = 0; j < c2.Meshes.Count; j++)
                {
                    BoundingSphere c2BoundingSphere = c2.Meshes[j].BoundingSphere;
                    c1BoundingSphere.Radius *= 1.0f;
                    c2BoundingSphere.Center += Ball.Instance.Position;

                    if (c1BoundingSphere.Intersects(c2BoundingSphere))
                    {
                        Ball.Instance.IsFlying = false;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
