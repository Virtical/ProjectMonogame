using Microsoft.Xna.Framework;
using System.Linq;
using System;
using TankMonogame.Model.QuadTreeAlgorithm;

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
            var newXValues = new double[4];
            var newYValues = new double[4];

            newXValues[0] = item.Pos.X + (item.LeftTop.X * Math.Cos(item.Angle) - item.LeftTop.Y * Math.Sin(item.Angle));
            newYValues[0] = item.Pos.Y + (item.LeftTop.X * Math.Sin(item.Angle) + item.LeftTop.Y * Math.Cos(item.Angle));

            newXValues[1] = item.Pos.X + (item.RightBottom.X * Math.Cos(item.Angle) - item.RightBottom.Y * Math.Sin(item.Angle));
            newYValues[1] = item.Pos.Y + (item.RightBottom.X * Math.Sin(item.Angle) + item.RightBottom.Y * Math.Cos(item.Angle));

            newXValues[2] = item.Pos.X + (item.LeftTop.X * Math.Cos(item.Angle) - item.RightBottom.Y * Math.Sin(item.Angle));
            newYValues[2] = item.Pos.Y + (item.LeftTop.X * Math.Sin(item.Angle) + item.RightBottom.Y * Math.Cos(item.Angle));

            newXValues[3] = item.Pos.X + (item.RightBottom.X * Math.Cos(item.Angle) - item.LeftTop.Y * Math.Sin(item.Angle));
            newYValues[3] = item.Pos.Y + (item.RightBottom.X * Math.Sin(item.Angle) + item.LeftTop.Y * Math.Cos(item.Angle));

            return new BoxQT((float)newXValues.Min(), (float)newYValues.Min(), (float)(newXValues.Max() - newXValues.Min()), (float)(newYValues.Max() - newYValues.Min()));
        }

        new public static bool Equals(IObject x, IObject y)
        {
            return x.ImageId.Equals(y.ImageId);
        }
        void Update();
    }
}
