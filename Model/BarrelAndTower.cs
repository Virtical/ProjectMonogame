using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using TankMonogame.Model.Interface;
using TankMonogame.Model.QuadTree;

namespace TankMonogame.Model
{
    public class BarrelAndTower : IObject
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        public float Angle { get; set; }
        public float RotationSpeed { get; set; }
        public Vector2 Anchor { get; set; }
        public Tank tank { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 direction = mousePosition - Pos;
            float turretRotation = (float)Math.Atan2(direction.Y, direction.X);

            float difference = turretRotation - Angle;
            difference = (float)((difference + Math.PI) % (2 * Math.PI) - Math.PI);

            if (Math.Abs(difference) < 0.03)
            {
                RotationSpeed = 0f;
            }
            else
            {
                RotationSpeed = Math.Sign(difference) * 0.03f;
            }

            Angle += RotationSpeed;
            Pos = tank.Pos - new Vector2(15, 15) * new Vector2((float)Math.Cos(tank.Angle), (float)Math.Sin(tank.Angle));
        }

        public BoxQT GetBox(IObject item)
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

        public bool Equals(IObject x, IObject y)
        {
            return x.ImageId.Equals(y.ImageId);
        }
    }
}
