using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankMonogame.Model.Interface;

namespace TankMonogame.Model
{
    internal class Tank : IObject
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        public float Rotation { get; set; }
        public float RotationSpeed { get; set; }
        public float MaxRotationSpeed { get; set; }

        public void Update()
        {
            Rotation += RotationSpeed;
            Pos += Speed * new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        }
    }
}
