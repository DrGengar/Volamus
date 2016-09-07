﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Drop
    {
        Vector3 viewVector;

        Vector3 position;
        Vector3 velocity;

        Model drop;
        int timeToLive;

        BoundingSphere boundingSphere;

        public int ttl
        {
            get { return timeToLive; }
            set { timeToLive = value; }
        }
        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }

        //Drop während dem Spiel
        public Drop(Vector3 pos, int ttl, Model dr)
        {
            position = pos;
            timeToLive = ttl;
            drop = dr;

            boundingSphere = new BoundingSphere();

            foreach (ModelMesh mesh in drop.Meshes)
            {
                if (boundingSphere.Radius == 0)
                    boundingSphere = mesh.BoundingSphere;
                else
                    boundingSphere = BoundingSphere.CreateMerged(boundingSphere, mesh.BoundingSphere);
            }

            boundingSphere.Radius *= 1.0f;
        }

        //Drop ende mit Bewegung
        public Drop(Vector3 pos, int ttl, Model dr, Vector3 velo)
        {
            position = pos;
            timeToLive = ttl;
            drop = dr;
            velocity = velo;
        }

        //Update für die Drops, die die Ballgröße verändern
        public void Update()
        {
            boundingSphere.Center = position;
            timeToLive--;
        }

        //Update für die Drops, die die Bewegungsgeschwindigkeit des Gegners verringern
        public void UpdateVelo()
        {
            boundingSphere.Center = position;
            timeToLive--;
        }

        //Update für die Konfetties
        public void UpdateEnd()
        {
            position += velocity;
            timeToLive--;
        }


        public void Draw(Camera camera, Effect effect)
        {
            Matrix[] transforms = new Matrix[drop.Bones.Count];
            drop.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in drop.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    effect.Parameters["World"].SetValue(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                            Matrix.CreateScale(1.50f, 1.50f, 2.0f)
                            * Matrix.CreateTranslation(position));
                    effect.Parameters["View"].SetValue(camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue(camera.ProjectionMatrix);
                    Matrix WorldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(transforms[mesh.ParentBone.Index] * Matrix.CreateRotationX(MathHelper.ToRadians(90)) *
                            Matrix.CreateScale(1.0f, 1.0f, 1.0f)
                            * Matrix.CreateTranslation(position)));
                    effect.Parameters["WorldInverseTranspose"].SetValue(WorldInverseTransposeMatrix);

                    viewVector = Vector3.Transform(camera.View - camera.Position, Matrix.CreateRotationY(0));
                    viewVector.Normalize();
                    effect.Parameters["ViewVector"].SetValue(viewVector);
                }
                mesh.Draw();
            }

        }
    }
}
