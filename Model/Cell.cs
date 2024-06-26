﻿using Microsoft.Xna.Framework;
using System;
using TankMonogame.Shared.Enums;
using TankMonogame.Model.QuadTreeAlgorithm;
using TankMonogame.Shared.Interface;

namespace TankMonogame.Model
{
    public class Cell : ICell, IGetBox<Cell>, IEquals<Cell>
    {
        public Cell(Point position, TypeCell cellType) 
        {
            CellSize = 64;
            Type = cellType;

            LTPoint = position;
            RTPoint = position + new Point(64, 0);
            RBPoint = position + new Point(64, 64);
            LBPoint = position + new Point(0, 64);
        }

        public readonly int CellSize;
        public TypeCell Type { get; set; }

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
            return x.Equals(y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Cell other = (Cell)obj;
            return LTPoint == other.LTPoint;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(LTPoint);
        }
    }
}
