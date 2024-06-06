using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankMonogame.Model.QuadTreeAlgorithm;

namespace TankMonogame.Model.GJKAlgorithm
{
    public static class GJK
    {
        public static bool DefinitionOfCollision(BoxQT borders1, BoxQT borders2)
        {
            var simplex = new Simplex();
            Vector2? dir = new Vector2(0, -1);
            var initSupportPoint = FindSupportPoint(dir.Value, borders1, borders2);

            simplex.points.Add(initSupportPoint);

            dir = new Vector2(-dir.Value.X, -dir.Value.Y);

            while (dir != null)
            {
                var supportPoint = FindSupportPoint(dir.Value, borders1, borders2);

                if (Vector2.Dot(new Vector2(supportPoint.X, supportPoint.Y), dir.Value!) <= 0)
                {
                    return false;
                }

                simplex.points.Add(supportPoint);
                dir = simplex.CalculateDirection();
            }

            return true;
        }

        private static Point FindSupportPoint(Vector2 d, BoxQT A, BoxQT B)
        {
            var Afar = A.FarthestVectorInDirection(Vector2.Normalize(d));
            var Bfar = B.FarthestVectorInDirection(Vector2.Normalize(-d));
            var s = Afar - Bfar;

            return s;
        }

        public static Point FarthestVectorInDirection(this BoxQT borders, Vector2 direction)
        {
            if (borders.IsEmpty())
            {
                throw new ArgumentException("Borders or its Vectors list is null or empty.");
            }

            Vector2 farthestVector = borders.GetTopLeft().ToVector2();
            float maxDotProduct = Vector2.Dot(farthestVector, direction);

            foreach (var vector in borders.Corners())
            {
                float dotProduct = Vector2.Dot(new Vector2(vector.X, vector.Y), direction);
                if (dotProduct > maxDotProduct)
                {
                    maxDotProduct = dotProduct;
                    farthestVector = new Vector2(vector.X, vector.Y);
                }
            }

            return new Point((int)farthestVector.X, (int)farthestVector.Y);
        }
    }

    public class Simplex
    {
        public List<Point> points = new List<Point>();

        public Vector2? CalculateDirection()
        {
            var a = points[^1];
            var ao = new Vector2(-a.X, -a.Y);

            Point b;
            Point ab;
            Vector2 abPerp;

            if (points.Count == 3)
            {
                b = points[1];
                ab = b - a;

                var c = points[0];
                var ac = c - a;

                abPerp = new Vector2(ab.Y, -ab.X);

                if (Vector2.Dot(abPerp, new Vector2(c.X, c.Y)) >= 0)
                {
                    abPerp = new Vector2(-abPerp.X, -abPerp.Y);
                }

                if (Vector2.Dot(abPerp, ao) > 0)
                {
                    points.RemoveAt(0);
                    return abPerp;
                }

                var acPerp = new Vector2(ac.Y, -ac.X);
                if (Vector2.Dot(acPerp, new Vector2(b.X, b.Y)) >= 0)
                {
                    acPerp = new Vector2(-acPerp.X, -acPerp.Y);
                }
                if (Vector2.Dot(acPerp, ao) > 0)
                {
                    points.RemoveAt(1);
                    return acPerp;
                }
                return null;
            }

            b = points[0];
            ab = b - a;
            abPerp = new Vector2(ab.Y, -ab.X);

            if (Vector2.Dot(abPerp, ao) <= 0)
            {
                abPerp = new Vector2(-abPerp.X, -abPerp.Y);
            }
            return abPerp;
        }
    }
}
