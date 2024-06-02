using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using TankMonogame.Model.Interface;

namespace TankMonogame.Model
{
    public class Map
    {
        public readonly int Width;
        public readonly int Height;
        Random r = new Random();
        public HashSet<Cell> Cells = new HashSet<Cell>();
        List<Point> borders = new List<Point>();


        public Map(int width, int height, int cellSize) 
        {
            Width = width;
            Height = height;

            List<Point> controlPoints = new List<Point>() { new Point(128, 320), new Point(2240, 320), new Point(2240, 1216), new Point(128, 1216) };

            HashSet<Point> trail = Snake.SnakeByDots(controlPoints);

            foreach (Point point in trail)
            {
                for (int x = -1; x < 2; x++)
                {
                    for(int y = -1; y < 2; y++)
                    {
                        if (Math.Abs(x) == Math.Abs(y)) continue;

                        if (!trail.Contains(new Point(point.X + x*64, point.Y + y*64)))
                        {
                            Cells.Add(new Cell(cellSize, new Point(point.X + x * 64, point.Y + y * 64), ICell.TypeCell.Level8));
                            borders.Add(new Point(point.X + x * 64, point.Y + y * 64));
                        }
                    }
                }

                if (point == controlPoints[0] || point == controlPoints[1] || point == controlPoints[2] || point == controlPoints[3])
                {
                    Cells.Add(new Cell(cellSize, point, ICell.TypeCell.Level6));
                }
                else
                {
                    Cells.Add(new Cell(cellSize, point, (ICell.TypeCell)r.Next(3, 5)));
                }
            }

            for (int y = 0; y < height; y += cellSize)
            {
                for (int x = 0; x < width; x += cellSize)
                {
                    if (!trail.Contains(new Point(x, y)) && !borders.Contains(new Point(x, y)))
                    {
                        Cells.Add(new Cell(cellSize, new Point(x, y), (ICell.TypeCell)r.Next(0, 3)));
                    }
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
