using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TankMonogame.Model.QuadTreeAlgorithm;
using TankMonogame.Shared.Enums;
using TankMonogame.Model.RandomWalkAlgorithm;
using System.Linq;

namespace TankMonogame.Model
{
    public class Map
    {
        public readonly int Width;
        public readonly int Height;
        public static Dictionary<TypeCell, Texture2D> TextureCell = new Dictionary<TypeCell, Texture2D>();

        Random r = new Random();

        public HashSet<Cell> Cells = new HashSet<Cell>();
        HashSet<Point> borders = new HashSet<Point>();
        HashSet<Point> trail = new HashSet<Point>();
        public HashSet<Burrel> burrels = new HashSet<Burrel>();

        public QuadTree<Cell> StaticTreeBorders = new QuadTree<Cell>(new BoxQT(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Cell.GetBox, Cell.Equals);
        public QuadTree<Burrel> StaticTreeBurrels = new QuadTree<Burrel>(new BoxQT(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Burrel.GetBox, Burrel.Equals);

        public Map(int width, int height) 
        {
            Width = width;
            Height = height;

            List<Point> controlPoints = new List<Point>() { new Point(128, 320), new Point(2240, 320), new Point(2240, 1216), new Point(128, 1216) };

            trail = RandomWalk.SnakeByDots(controlPoints);

            foreach (Point point in trail)
            {
                for (int x = -1; x < 2; x++)
                {
                    for(int y = -1; y < 2; y++)
                    {
                        if (Math.Abs(x) == Math.Abs(y)) continue;

                        if (!trail.Contains(new Point(point.X + x*64, point.Y + y*64)))
                        {
                            var borderPoint = new Point(point.X + x * 64, point.Y + y * 64);
                            var borderCell = new Cell(borderPoint, TypeCell.Level8);
                            Cells.Add(borderCell);
                            StaticTreeBorders.Add(borderCell);
                            borders.Add(new Point(point.X + x * 64, point.Y + y * 64));
                        }
                    }
                }

                Cells.Add(new Cell(point, (TypeCell)r.Next(3, 5)));

                if (r.Next(40) == 0) 
                {
                    for (int x = -1; x < 2; x++)
                    {
                        for (int y = -1; y < 2; y++)
                        {
                            if (Math.Abs(x) == Math.Abs(y) && x != 0) continue;

                            var newPoint = new Point(x*64, y*64) + point;

                            var IsNearStart = Math.Sqrt(Math.Pow(controlPoints[0].X - newPoint.X, 2) + Math.Pow(controlPoints[0].Y - newPoint.Y, 2)) < 256;
                            var IsNearEnd = Math.Sqrt(Math.Pow(controlPoints[3].X - newPoint.X, 2) + Math.Pow(controlPoints[3].Y - newPoint.Y, 2)) < 256;

                            if (!IsNearStart && !IsNearEnd && trail.Contains(newPoint))
                            {
                                if (r.Next(2) == 0)
                                {
                                    var newBurrel = new Burrel(newPoint);
                                    StaticTreeBurrels.Add(newBurrel);
                                    burrels.Add(newBurrel);
                                }
                            }
                        }
                    }
                }
            }

            for (int y = 0; y < height; y += 64)
            {
                for (int x = 0; x < width; x += 64)
                {
                    if (!trail.Contains(new Point(x, y)) && !borders.Contains(new Point(x, y)))
                    {
                        Cells.Add(new Cell(new Point(x, y), (TypeCell)r.Next(0, 3)));
                    }
                }
            }
        }

        public Point GetRandomCleanPlace()
        {
            while (true)
            {
                var randomPoint = trail.ToArray()[r.Next(trail.Count-1)];

                var IsAllCellTrails = true;
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        if (!trail.Contains(randomPoint + new Point(x * 64, y * 64)) || burrels.Contains(new Burrel(randomPoint + new Point(x * 64, y * 64))))
                        {
                            IsAllCellTrails = false;
                            break;
                        }
                    }
                }

                if (IsAllCellTrails)
                {
                    return randomPoint;
                }
            }
        }

        public bool IsInside(int x, int y)
        {
            if ((x > 0) && (y < Width) && (y > 0) && (y < Height))
            {
                return true;   
            }

            return false;
        }
    }
}
