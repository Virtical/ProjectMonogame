using TankMonogame.Model.QuadTreeAlgorithm;

namespace TankMonogame.Shared.Interface
{
    public interface IGetBox<T>
    {
        public static BoxQT GetBox(T item)
        {
            return new BoxQT();
        }
    }
}
