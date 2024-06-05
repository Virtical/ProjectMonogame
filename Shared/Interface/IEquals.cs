namespace TankMonogame.Shared.Interface
{
    public interface IEquals<T>
    {
       public static bool Equals(T x, T y)
       {
            return x.Equals(y);
       }
    }
}
