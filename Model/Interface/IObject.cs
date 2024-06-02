using Microsoft.Xna.Framework;

namespace TankMonogame.Model.Interface
{
    public interface IObject
    {
        int ImageId { get; set; }
        Vector2 Pos { get; }
        Vector2 Anchor { get; }
        float Rotation { get; set; }
        public float Speed { get; set; }
        public int MaxSpeed { get; set; }
        void Update();
    }
}
