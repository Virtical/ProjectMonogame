using System;

namespace TankMonogame.Model.QuadTreeAlgorithm
{
    public class BoxQT
    {
        private float left;
        private float top;
        private float width;
        private float height;

        public BoxQT(float left = 0, float top = 0, float width = 0, float height = 0)
        {
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
        }

        public BoxQT(VectorQT position, VectorQT size)
        {
            left = position.X;
            top = position.Y;
            width = size.X;
            height = size.Y;
        }

        public float GetLeft()
        {
            return left;
        }
        public float GetTop()
        {
            return top;
        }

        public float GetRight()
        {
            return left + width;
        }

        public float GetBottom()
        {
            return top + height;
        }

        public VectorQT GetTopLeft()
        {
            return new VectorQT(left, top);
        }

        public VectorQT GetCenter()
        {
            return new VectorQT(left + width / 2, top + height / 2);
        }

        public VectorQT GetSize()
        {
            return new VectorQT(width, height);
        }

        public bool Contains(BoxQT box)
        {

            var l = left <= box.left;
            var r = box.GetRight() <= GetRight();
            var t = top <= box.top;
            var b = box.GetBottom() <= GetBottom();

            return left <= box.left && box.GetRight() <= GetRight() &&
                   top <= box.top && box.GetBottom() <= GetBottom();
        }

        public bool Intersects(BoxQT box)
        {
            return !(left >= box.GetRight() || GetRight() <= box.left ||
                     top >= box.GetBottom() || GetBottom() <= box.top);
        }

        public CollisionResultQT DetectCollision(BoxQT other)
        {
            if (!Intersects(other))
            {
                return null; // Нет столкновения
            }

            // Вычисляем глубину проникновения по каждой оси
            float dx1 = other.GetRight() - GetLeft();
            float dx2 = GetRight() - other.GetLeft();
            float dy1 = other.GetBottom() - GetTop();
            float dy2 = GetBottom() - other.GetTop();

            // Находим минимальное проникновение
            float penetrationX = Math.Min(dx1, dx2);
            float penetrationY = Math.Min(dy1, dy2);

            // Определяем нормаль столкновения
            VectorQT normal;
            if (penetrationX < penetrationY)
            {
                normal = dx1 < dx2 ? new VectorQT(-1, 0) : new VectorQT(1, 0);
            }
            else
            {
                normal = dy1 < dy2 ? new VectorQT(0, -1) : new VectorQT(0, 1);
            }

            // Возвращаем результат столкновения
            return new CollisionResultQT(Math.Min(penetrationX, penetrationY), normal);
        }
    }
}
