using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    public class Camera
    {
        Vector3 cameraPos;
        Vector3 cameraView;
        Vector3 cameraUp;

        Matrix viewMatrix;
        Matrix projectionMatrix;
        //float aspectRatio;

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
        }

        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
        }

        public Vector3 Position
        {
            get { return cameraPos; }
        }

        public Vector3 View
        {
            get { return cameraView; }
        }

        public Vector3 Up
        {
            get { return cameraUp; }
        }

        public Camera(Vector3 camP, Vector3 camV, Vector3 camU)
        {
            cameraPos = camP;
            cameraView = camV;
            cameraUp = camU;

            /*
             1. Parameter FieldOfView
             2. Parameter: aspect ratio
             3. Parameter: near plane
             4. Parameter: far plane
             */
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0f), 16 / 9, .5f, 5000);

            Update();
        }

        public void ResetCamera()
        {
            cameraPos = new Vector3(0, 0, 0);
            cameraView = new Vector3(0, 0, 0);
            cameraUp = Vector3.UnitZ;
            viewMatrix = Matrix.Identity;

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 16 / 9, .5f, 1000);
        }

        public void Update()
        {
            viewMatrix = Matrix.CreateLookAt(cameraPos, cameraView, cameraUp);
        }

        public void AddPosition(Vector3 pos)
        {
            cameraPos += pos;
        }

        public void AddView(Vector3 view)
        {
            cameraView += view;
        }
    }
}
