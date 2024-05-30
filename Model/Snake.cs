using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TankMonogame.Model
{
    public class Snake
    {
        Point start;
        Point current;
        Point end;

        private Random random;

        Direction lastDirection = Direction.Straight;

        Dictionary<Direction, int> likelyDirection = new Dictionary<Direction, int>();

        Dictionary<Direction, Point> pointDirection = new Dictionary<Direction, Point>();

        public enum Direction
        {
            Left, Right, Straight
        }

        public Snake(Point start, Point end)
        {
            random = new Random();

            likelyDirection[Direction.Left] = 3;
            likelyDirection[Direction.Right] = 3;
            likelyDirection[Direction.Straight] = 7;

            pointDirection[Direction.Left] = new Point(0, -64);
            pointDirection[Direction.Right] = new Point(0, 64);
            pointDirection[Direction.Straight] = new Point(64, 0);

            this.start = start;
            this.current = start;
            this.end = end;
        }

        public Point GetNextPoint()
        {
            if (current == end)
            {
                return Point.Zero;
            }

            int totalWeight = likelyDirection.Values.Sum();
            int randomNumber = random.Next(totalWeight);
            int cumulativeWeight = 0;

            foreach (KeyValuePair<Direction, int> pair in likelyDirection)
            {
                if (pair.Value == 0) continue;

                cumulativeWeight += pair.Value;
                if (randomNumber < cumulativeWeight)
                {
                    lastDirection = pair.Key;
                    current += pointDirection[pair.Key];
                    UpdateLikelyDirection();
                    return current;
                }
            }
            return Point.Zero;
        }

        private void UpdateLikelyDirection()
        {
            if (current.X < end.X)
            {
                if (lastDirection == Direction.Left)
                {
                    likelyDirection[Direction.Right] = 0;
                    likelyDirection[Direction.Left] = (192 - Math.Abs(end.Y - current.Y)) / 64;
                }

                if (lastDirection == Direction.Right)
                {
                    likelyDirection[Direction.Left] = 0;
                    likelyDirection[Direction.Right] = (192 - Math.Abs(end.Y - current.Y)) / 64;
                }

                if (lastDirection == Direction.Straight)
                {
                    if ((end.Y - current.Y) > 0)
                    {
                        likelyDirection[Direction.Right] = (end.Y - current.Y + 192) / 64;
                    }
                    else if ((end.Y - current.Y) < 0)
                    {
                        likelyDirection[Direction.Left] = (current.Y - end.Y + 192) / 64;
                    }
                    else
                    {
                        likelyDirection[Direction.Left] = 3;
                        likelyDirection[Direction.Right] = 3;
                    }
                }
            }
            else
            {
                likelyDirection[Direction.Straight] = 0;
                if ((end.Y - current.Y) > 0)
                {
                    likelyDirection[Direction.Right] = 1;
                    likelyDirection[Direction.Left] = 0;
                }
                else if ((end.Y - current.Y) < 0)
                {
                    likelyDirection[Direction.Left] = 1;
                    likelyDirection[Direction.Right] = 0;
                }
            }
        }
    }
}
