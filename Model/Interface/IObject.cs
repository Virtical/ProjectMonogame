using Microsoft.Xna.Framework;

namespace TankMonogame.Model.Interface
{
    public interface IObject
    {
        int ImageId { get; set; }
        Vector2 Pos { get; }
        Vector2 Anchor { get; }
        float Angle { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        Vector2 LeftTop { get; set; }
        Vector2 RightBottom { get; set; }
        void Update();
    }
}
