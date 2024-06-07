using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TankMonogame.Model.AStarAlgorithm;
using TankMonogame.Shared.Interface;

namespace TankMonogame.Model
{
    public class Rocket : IObject
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
        public List<AStar.Node> TargetRoute { get; set; }

        private int state = 0;

        public bool IsDestroyed = false;


        public void Update()
        {
            if (state < TargetRoute.Count - 1)
            {
                Pos = TargetRoute[state].Position;
                Angle = TargetRoute[state].Angle;
                state++;
            }
            else
            {
                IsDestroyed = true;
            }
        }
    }
}
