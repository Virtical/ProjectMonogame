using Microsoft.Xna.Framework;
using System;
using System.Linq;
using TankMonogame.Model.Interface;
using TankMonogame.Model.QuadTree;

namespace TankMonogame.Model
{
    public class Tank : IObject, IGetBox<Tank>, IEquals<Tank>
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        public float Angle { get; set; }
        public float RotationSpeed { get; set; }
        public float MaxRotationSpeed { get; set; }
        public Vector2 Anchor { get; set; }
        public BarrelAndTower Turret { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }
        public Vector2 VelocityProjection { get; set; }
        public Vector2 Velocity { get; set; }

        public void Update()
        {
            Angle += RotationSpeed;
            Velocity = Speed * new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
            Pos += Velocity - VelocityProjection;
            VelocityProjection = new Vector2(0, 0);
        }

        public static BoxQT GetBox(IObject item)
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

        public static bool Equals(IObject x, IObject y)
        {
            return x.ImageId.Equals(y.ImageId);
        }
    }
}
