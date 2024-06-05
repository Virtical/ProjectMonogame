using Microsoft.Xna.Framework;
using TankMonogame.Shared.Enums;

namespace TankMonogame.Shared.Interface
{
    public interface ICell
    {
        TypeCell Type { get; set; }
        bool IsInside(Point p);
    }
}