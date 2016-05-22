using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volamus_v1
{
    class Kamera
    {
        Vector3 cameraPos;
        Vector3 cameraView;
        Vector3 cameraUp;

        Matrix viewMatrix;
        Matrix projectionMatrix;
        //float aspectRatio;

        public Kamera(Vector3 camP, Vector3 camV, Vector3 camU)
        {
            cameraPos = camP;
            cameraView = camV;
            cameraUp = camU;

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90.0f), 16 / 9, .5f, 1000);

            Update();
        }

        public void ResetCamera()
        {
            cameraPos = new Vector3(0, -10, 40);
            cameraView = new Vector3(0, 0, 0);
            cameraUp = Vector3.UnitZ;
            viewMatrix = Matrix.Identity;

            /*1. Parameter FieldOfView
             * 2. Parameter: aspect ratio
             * 3. Parameter: near plane
             * 4. Parameter: far plane
             * */
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 16 / 9, .5f, 1000);
        }

        public void Update()
        {
            UpdateViewMatrix();
        }
        private void UpdateViewMatrix()
        {
            viewMatrix = Matrix.CreateLookAt(cameraPos, cameraView, cameraUp);
        }

        public Matrix get_View()
        {
            return viewMatrix;
        }

        public Matrix get_Projection()
        {
            return projectionMatrix;
        }


    }
}
