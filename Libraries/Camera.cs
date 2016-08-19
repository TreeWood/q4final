// Camera.cs

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quarter4Project.Libraries
{
    class Camera
    {

        #region Fields

        public Matrix matrix; // Matrix of the camera position.
        Vector2 pos = Vector2.Zero; // Camera Position.
        public Rectangle? limits;
        float timer = 0;

        #endregion

        #region Methods

        /// <summary>
        /// Sets matrix using the position of the camera.
        /// </summary>
        /// <returns>Returns matrix version of the camera position.</returns>
        public Matrix getMatrix(Vector2 parallax)
        {
            matrix = Matrix.CreateTranslation(new Vector3(-pos.X, -pos.Y, 1));
            return matrix;
        }

        #endregion

        #region Accessors

        public Vector2 Position
        {
            get
            {
                return pos;
            }
            set
            {
                pos = value;
                if (Limits != null)
                {
                    pos.X = MathHelper.Clamp(pos.X, pos.X, pos.X);
                    pos.Y = MathHelper.Clamp(pos.Y, pos.Y, pos.Y);
                }
            }
        }

        public Rectangle? Limits
        {
            get
            {
                return limits;
            }
            set
            {
                if (value != null)
                {
                    limits = new Rectangle { X = value.Value.X, Y = value.Value.Y, Width = value.Value.Width, Height = value.Value.Height };
                    Position = Position;
                }
                else
                {
                    limits = null;
                }
            }
        }

        #endregion

    }
}
