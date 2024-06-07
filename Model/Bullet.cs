using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TankMonogame.Shared.Interface;

namespace TankMonogame.Model
{
    public class Bullet : IObject
    {
        public static Texture2D Texture { get; set; }
        public int Id { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Anchor { get; set; }
        public float Angle { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }

        public void Update()
        {
            Pos += new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle)) * Speed;
        }
    }
}
