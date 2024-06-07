using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TankMonogame.Shared.Interface;

namespace TankMonogame.Model
{
    public class Turret : IObject
    {
        public static Texture2D Texture { get; set; }
        public int Id { get; set; }
        public Vector2 Pos { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        public float Angle { get; set; }
        public float RotationSpeed { get; set; }
        public Vector2 Anchor { get; set; }
        public TankHull Tank { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }

        public List<Bullet> Bullets = new List<Bullet>();

        public int? AnimationFrame = null;

        private int[] animation = new int[9] { -2, -4, -6, -8, -10, -8, -6, -4, -2};

        public bool IsDestroyed = false;

        public void Update()
        {
            if (!IsDestroyed)
            {
                Angle += RotationSpeed;

                if (AnimationFrame != null)
                {
                    Anchor = new Vector2(27, 23) - new Vector2(animation[AnimationFrame.Value], 0);
                    AnimationFrame++;
                    if (AnimationFrame == 8)
                    {
                        AnimationFrame = null;
                    }
                }
                else
                {
                    Anchor = new Vector2(27, 23);
                }
                Pos = Tank.Pos - new Vector2(15, 15) * new Vector2((float)Math.Cos(Tank.Angle), (float)Math.Sin(Tank.Angle));
            }
        }
    }
}
