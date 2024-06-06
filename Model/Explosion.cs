﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankMonogame.Shared.Interface;

namespace TankMonogame.Model
{
    public class Explosion: IObject
    {
        public int ImageId { get; set; }
        public Vector2 Pos { get; set; }
        public Vector2 Anchor { get; set; }
        public float Angle { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }

        public int AnimationFrame = 0;

        public void Update()
        {
            
        }
    }
}
