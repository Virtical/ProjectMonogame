using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankMonogame.Model.Interface
{
    public interface IEquals<T>
    {
       public static bool Equals(T x, T y)
       {
            return x.Equals(y);
       }
    }
}
