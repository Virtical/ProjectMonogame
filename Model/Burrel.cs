using Microsoft.Xna.Framework;
using System;
using TankMonogame.Model.QuadTreeAlgorithm;
using TankMonogame.Shared.Enums;
using TankMonogame.Shared.Interface;

namespace TankMonogame.Model
{
    public class Burrel: IGetBox<Burrel>, IEquals<Burrel>
    {
        public Burrel(Point position)
        {
            LTPoint = position;
            RTPoint = position + new Point(64, 0);
            RBPoint = position + new Point(64, 64);
            LBPoint = position + new Point(0, 64);
        }

        public readonly Point LTPoint;
        public readonly Point RTPoint;
        public readonly Point RBPoint;
        public readonly Point LBPoint;

        public int ImageId = 5;

        public static BoxQT GetBox(Burrel item)
        {
            return new BoxQT(item.LTPoint.X, item.LTPoint.Y, item.RBPoint.X - item.LTPoint.X, item.RBPoint.Y - item.LTPoint.Y);
        }

        public static bool Equals(Burrel x, Burrel y)
        {
            return x.Equals(y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Burrel other = (Burrel)obj;
            return LTPoint == other.LTPoint;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LTPoint);
        }
    }
}
