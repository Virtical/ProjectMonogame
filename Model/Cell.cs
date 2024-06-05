using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TankMonogame.Model.Interface;
using TankMonogame.Model.QuadTree;

namespace TankMonogame.Model
{
    public class Cell : ICell, IGetBox<Cell>, IEquals<Cell>
    {
        public Cell(int size, Point position, ICell.TypeCell cellType) 
        {
            CellSize = size;
            Type = cellType;

            LTPoint = position;
            RTPoint = position + new Point(size, 0);
            RBPoint = position + new Point(size, size);
            LBPoint = position + new Point(0, size);
        }

        public readonly int CellSize;
        public ICell.TypeCell Type { get; set; }

        public readonly Point LTPoint;
        public readonly Point RTPoint;
        public readonly Point RBPoint;
        public readonly Point LBPoint;

        public bool IsInside(Point p)
        {
            throw new NotImplementedException();
        }

        public static BoxQT GetBox(Cell item)
        {
            return new BoxQT(item.LTPoint.X, item.LTPoint.Y, item.RBPoint.X- item.LTPoint.X, item.RBPoint.Y - item.LTPoint.Y);
        }

        public static bool Equals(Cell x, Cell y)
        {
            return x.LTPoint.Equals(y.LTPoint);
        }
    }
}
