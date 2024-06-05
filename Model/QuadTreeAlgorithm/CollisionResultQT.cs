namespace TankMonogame.Model.QuadTreeAlgorithm
{
    public class CollisionResultQT
    {
        public float T { get; set; }
        public VectorQT N { get; set; }
        public CollisionResultQT(float t, VectorQT n)
        {
            T = t;
            N = n;
        }
    }
}
