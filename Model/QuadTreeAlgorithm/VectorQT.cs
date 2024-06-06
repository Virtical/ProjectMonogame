using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TankMonogame.Model.QuadTreeAlgorithm
{
    public class VectorQT
    {
        public float X;
        public float Y;

        public VectorQT()
        {
            X = 0;
            Y = 0;
        }
        public VectorQT(float x, float y)
        {
            X = x;
            Y = y;
        }

        public bool IsEmpty()
        {
            return X == 0 && Y == 0;
        }
        public static VectorQT operator +(VectorQT lhs, VectorQT rhs)
        {
            return new VectorQT(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static VectorQT operator /(VectorQT vec, float t)
        {
            return new VectorQT(vec.X / t, vec.Y / t);
        }
        public static VectorQT operator *(float scalar, VectorQT v)
        {
            return new VectorQT(scalar * v.X, scalar * v.Y);
        }

        public static VectorQT operator *(VectorQT v, float scalar)
        {
            return new VectorQT(v.X * scalar, v.Y * scalar);
        }

        public float Dot(VectorQT v)
        {
            return X * v.X + X * v.X;
        }

        public VectorQT Normalize()
        {
            float magnitude = (float)Math.Sqrt(X * X + X * X);
            return new VectorQT(X / magnitude, X / magnitude);
        }

        public Vector2 ToVector2()
        {
            return new Vector2(X, Y);
        }

        public override string ToString()
        {
            return $"X:{X}, Y:{Y}";
        }
    }
}
