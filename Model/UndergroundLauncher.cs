using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankMonogame.Shared.Interface;

namespace TankMonogame.Model
{
    public class UndergroundLauncher: IObject
    {

        public int AnimationFrame = 0;
        public float Angle { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        public Vector2 LeftTop { get; set; }
        public Vector2 RightBottom { get; set; }
        public Vector2 Anchor { get; set; }
        public int ImageId { get; set; }
        public Vector2 Pos { get; set; }

        public State CurState = State.ThreeRockets;
        public enum State
        {
            ThreeRockets,
            TwoRockets,
            OneRocket
        }

        public void NextState()
        {
            CurState = (State)(((int)CurState + 1) % 3);
            AnimationFrame = (int)CurState * 2;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
