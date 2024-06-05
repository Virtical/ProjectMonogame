using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankMonogame.Model.QuadTree;

namespace TankMonogame.Model.Interface
{
    public interface IGetBox<T>
    {
        public static BoxQT GetBox(T item)
        {
            return new BoxQT();
        }
    }
}
