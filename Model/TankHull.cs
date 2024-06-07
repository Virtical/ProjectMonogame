using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TankMonogame.Shared.Enums;
using TankMonogame.Shared.Interface;

namespace TankMonogame.Model
{
    public class TankHull : IObject, IGetBox<TankHull>, IEquals<TankHull>
    {
        public static Texture2D Texture { get; set; }
        public int Id { get; set; }
        public Vector2 Pos { get; set; }
        public float Speed { get; set; }
        public float MAcceleration { get; set; }
        public float RAcceleration { get; set; }
        public int MaxSpeed { get; set; }
        public float Angle { get; set; }
        public float RotationSpeed { get; set; }
        public float MaxRotationSpeed { get; set; }
        public Vector2 Anchor { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }
        public Vector2 VelocityProjection { get; set; }
        public Vector2 Velocity { get; set; }

        public bool IsDestroyed = false;

        public Dictionary<DirectionOfMovement, float> MovementDirectionMultipliers = new Dictionary<DirectionOfMovement, float>()
        {
            { DirectionOfMovement.Forward, 1 },
            { DirectionOfMovement.Backward, -0.5f }
        };

        public Dictionary<DirectionOfRotation, float> RotationDirectionMultipliers = new Dictionary<DirectionOfRotation, float>()
        {
            { DirectionOfRotation.Right, 1 },
            { DirectionOfRotation.Left, -1 }
        };

        public void Update()
        {
            if (!IsDestroyed)
            {
                Angle += RotationSpeed;
                Velocity = Speed * new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
                Pos += Velocity - VelocityProjection;
                VelocityProjection = new Vector2(0, 0);
            }
        }
    }
}
