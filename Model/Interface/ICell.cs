

using Microsoft.Xna.Framework;

namespace TankMonogame.Model.Interface
{
    public interface ICell
    {
        public enum TypeCell : byte
        {
            Level1,
            Level2,
            Level3,
            Level4,
            Level5,
            Level6,
            Level7,
            Level8
        }

        TypeCell Type { get; set; }

        bool IsInside(Point p);


    }
}
