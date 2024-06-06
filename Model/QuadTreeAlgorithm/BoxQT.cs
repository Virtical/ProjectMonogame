using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TankMonogame.Model.GJKAlgorithm;

namespace TankMonogame.Model.QuadTreeAlgorithm
{
    public class BoxQT
    {
        private float width;
        private float height;

        private VectorQT LeftTop;
        private VectorQT RightTop;
        private VectorQT RightBottom;
        private VectorQT LeftBottom;


        public BoxQT()
        {
            LeftTop = new VectorQT();
            RightTop = new VectorQT();
            RightBottom = new VectorQT();
            LeftBottom = new VectorQT();
            width = 0;
            height = 0;
        }

        public BoxQT(float left, float top, float width, float height)
        {
            LeftTop = new VectorQT(left, top);
            RightTop = new VectorQT(left+width, top);
            RightBottom = new VectorQT(left+width, top+height);
            LeftBottom = new VectorQT(left, top + height);
            this.width = width;
            this.height = height;
        }

        public BoxQT(VectorQT lt, VectorQT rt, VectorQT rb, VectorQT lb)
        {
            LeftTop = lt;
            RightTop = rt;
            RightBottom = rb;
            LeftBottom = lb;
            width = 0;
            height = 0;
        }

        public BoxQT(VectorQT position, VectorQT size)
        {
            LeftTop = new VectorQT(position.X, position.Y);
            RightTop = new VectorQT(position.X + size.X, position.Y);
            RightBottom = new VectorQT(position.X + size.X, position.Y + size.Y);
            LeftBottom = new VectorQT(position.X, position.Y + size.Y);
            width = size.X;
            height = size.Y;
        }

        public bool IsEmpty()
        {
            return LeftTop.IsEmpty() && LeftBottom.IsEmpty() && RightBottom.IsEmpty() && RightTop.IsEmpty();
        }

        public IEnumerable<Vector2> Corners()
        {
            yield return LeftTop.ToVector2();
            yield return RightTop.ToVector2();
            yield return RightBottom.ToVector2();
            yield return LeftBottom.ToVector2();
        }

        public float GetLeft()
        {
            return Corners().Select(point => point.X).Min();
        }
        public float GetTop()
        {
            return Corners().Select(point => point.Y).Min();
        }

        public float GetRight()
        {
            return Corners().Select(point => point.X).Max();
        }

        public float GetBottom()
        {
            return Corners().Select(point => point.Y).Max();
        }

        public VectorQT GetTopLeft()
        {
            return LeftTop;
        }

        public VectorQT GetTopRight()
        {
            return RightTop;
        }

        public VectorQT GetBottomRight()
        {
            return RightBottom;
        }

        public VectorQT GetBottomLeft()
        {
            return LeftBottom;
        }

        public VectorQT GetCenter()
        {
            return new VectorQT(LeftTop.X + width / 2, LeftTop.Y + height / 2);
        }

        public VectorQT GetSize()
        {
            return new VectorQT(width, height);
        }

        public bool Contains(BoxQT box)
        {
            return GetLeft() <= box.GetLeft() && box.GetRight() <= GetRight() &&
                   GetTop() <= box.GetTop() && box.GetBottom() <= GetBottom();
        }

        public bool Intersects(BoxQT box)
        {
            return !(GetLeft() >= box.GetRight() || GetRight() <= box.GetLeft() ||
                     GetTop() >= box.GetBottom() || GetBottom() <= box.GetTop());
        }

        public CollisionResultQT DetectCollision(BoxQT other)
        {
            if (!GJK.DefinitionOfCollision(this, other))
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
