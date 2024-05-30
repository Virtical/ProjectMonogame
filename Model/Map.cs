using Microsoft.Xna.Framework;
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

        Snake snake = new Snake(new Point(64, 768), new Point(2432, 768));

        public List<Cell> Cells = new List<Cell>();
        private List<Point> points = new List<Point>();

        public Map(int width, int height, int cellSize) 
        {
            Width = width;
            Height = height;

            Point newPoint = new Point(64, 768);
            while (newPoint != new Point(2432, 768))
            {
                newPoint = snake.GetNextPoint();
                points.Add(newPoint);
            }

            for (int y = 0; y < height; y += cellSize)
            {
                for (int x = 0; x < width; x += cellSize)
                {
                    if ((y == (height / cellSize)/2* cellSize) && (x == 64 || x == 2432))
                    {
                        Cells.Add(new Cell(cellSize, new Point(x, y), ICell.TypeCell.Level4));
                    }

                    else if (points.Contains(new Point(x, y)))
                    {
                        Cells.Add(new Cell(cellSize, new Point(x, y), ICell.TypeCell.Level4));
                    }

                    else
                    {
                        Cells.Add(new Cell(cellSize, new Point(x, y), (ICell.TypeCell)r.Next(0, 3)));
                    }
                }
            }
        }
    }
}
