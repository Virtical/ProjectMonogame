using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using TankMonogame.Model.Interface;

namespace TankMonogame.Model
{
    public class BarrelAndTower : IObject
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        public float Rotation { get; set; }
        public float RotationSpeed { get; set; }
        public Vector2 Anchor { get; set; }
        public Tank tank { get; set; }

        public void Update()
        {
            MouseState mouseState = Mouse.GetState();
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 direction = mousePosition - Pos;
            float turretRotation = (float)Math.Atan2(direction.Y, direction.X);

            float difference = turretRotation - Rotation;
            difference = (float)((difference + Math.PI) % (2 * Math.PI) - Math.PI);

            if (Math.Abs(difference) < 0.03)
            {
                RotationSpeed = 0f;
            }
            else
            {
                RotationSpeed = Math.Sign(difference) * 0.03f;
            }

            Rotation += RotationSpeed;
            Pos = tank.Pos - new Vector2(15, 15) * new Vector2((float)Math.Cos(tank.Rotation), (float)Math.Sin(tank.Rotation));
        }
    }
}
