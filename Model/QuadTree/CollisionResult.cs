using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankMonogame.Model.QuadTree
{
    public class CollisionResult
    {
        public float T { get; set; }
        public VectorQT N { get; set; }
        public CollisionResult(float t, VectorQT n)
        {
            T = t;
            N = n;
        }
    }
}
