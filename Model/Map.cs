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

        HashSet<Point> p = Snake.SnakeByDots(new List<Point>() { new Point(128, 320), new Point(2240, 320), new Point(2240, 1216), new Point(128, 1216) });

        public List<Cell> Cells = new List<Cell>();
        private HashSet<Point> points = new HashSet<Point>();


        public Map(int width, int height, int cellSize) 
        {
            Width = width;
            Height = height;

            points.Add(new Point(128, 320));

            foreach (Point i in p)
            {
                points.Add(i);
            }

            foreach (Point point in points)
            {
                if (point != new Point(128, 320) && point != new Point(2240, 320) && point != new Point(2240, 1216) && point != new Point(128, 1216))
                {
                    Cells.Add(new Cell(cellSize, point, (ICell.TypeCell)r.Next(3, 5)));
                }
                else
                {
                    Cells.Add(new Cell(cellSize, point, ICell.TypeCell.Level6));
                }
            }

            for (int y = 0; y < height; y += cellSize)
            {
                for (int x = 0; x < width; x += cellSize)
                {
                    if (!points.Contains(new Point(x, y)))
                    Cells.Add(new Cell(cellSize, new Point(x, y), (ICell.TypeCell)r.Next(0, 3)));
                }
            }
        }
    }
}
