using Microsoft.Xna.Framework;
using TankMonogame.Model.Interface;

namespace TankMonogame.Model
{
    public class Car : IObject
    {
        public int ImageId { get; set; }

        public Vector2 Pos { get; set; }
        public Vector2 Speed { get; set; }

        public void Update()
        {
            Pos += Speed;
        }
    }
}
