using Microsoft.Xna.Framework;
using System.Linq;
using System;
using TankMonogame.Model.QuadTreeAlgorithm;
using System.Threading.Tasks;

namespace TankMonogame.Shared.Interface
{
    public interface IObject : IGetBox<IObject>, IEquals<IObject>
    {
        int ImageId { get; set; }
        Vector2 Pos { get; set; }
        Vector2 Anchor { get; }
        float Angle { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        Vector2 LeftTop { get; set; }
        Vector2 RightBottom { get; set; }
        new public static BoxQT GetBox(IObject item)
        {
            var newValues = new VectorQT[4];

            var newXValues1 = item.Pos.X + (item.LeftTop.X * Math.Cos(item.Angle) - item.LeftTop.Y * Math.Sin(item.Angle));
            var newYValues1 = item.Pos.Y + (item.LeftTop.X * Math.Sin(item.Angle) + item.LeftTop.Y * Math.Cos(item.Angle));
            newValues[0] = new VectorQT((float)newXValues1, (float)newYValues1);

            var newXValues2 = item.Pos.X + (item.RightBottom.X * Math.Cos(item.Angle) - item.LeftTop.Y * Math.Sin(item.Angle));
            var newYValues2 = item.Pos.Y + (item.RightBottom.X * Math.Sin(item.Angle) + item.LeftTop.Y * Math.Cos(item.Angle));
            newValues[1] = new VectorQT((float)newXValues2, (float)newYValues2);

            var newXValues3 = item.Pos.X + (item.RightBottom.X * Math.Cos(item.Angle) - item.RightBottom.Y * Math.Sin(item.Angle));
            var newYValues3 = item.Pos.Y + (item.RightBottom.X * Math.Sin(item.Angle) + item.RightBottom.Y * Math.Cos(item.Angle));
            newValues[2] = new VectorQT((float)newXValues3, (float)newYValues3);

            var newXValues4 = item.Pos.X + (item.LeftTop.X * Math.Cos(item.Angle) - item.RightBottom.Y * Math.Sin(item.Angle));
            var newYValues4 = item.Pos.Y + (item.LeftTop.X * Math.Sin(item.Angle) + item.RightBottom.Y * Math.Cos(item.Angle));
            newValues[3] = new VectorQT((float)newXValues4, (float)newYValues4);

            return new BoxQT(newValues[0], newValues[1], newValues[2], newValues[3]);
        }

        new public static bool Equals(IObject x, IObject y)
        {
            return x.ImageId.Equals(y.ImageId);
        }
        void Update();
    }
}
