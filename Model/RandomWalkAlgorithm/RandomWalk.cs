using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TankMonogame.Model.RandomWalkAlgorithm
{
    public class RandomWalk
    {
        Point start;
        Point current;
        Point end;

        private Random random;

        Direction lastDirection = Direction.Straight;

        int stepAfterRotate = 0;

        Dictionary<Direction, int> likelyDirection = new Dictionary<Direction, int>();

        Dictionary<Direction, Point> pointDirection = new Dictionary<Direction, Point>();

        public enum Direction
        {
            Left, Right, Straight
        }

        public RandomWalk(Point start, Point end, Point straightDirection)
        {
            random = new Random();

            likelyDirection[Direction.Left] = 0;
            likelyDirection[Direction.Right] = 0;
            likelyDirection[Direction.Straight] = 3;

            pointDirection[Direction.Left] = new Point((int)(straightDirection.X * Math.Cos(-90 * (Math.PI / 180)) - straightDirection.Y * Math.Sin(-90 * (Math.PI / 180))), (int)(straightDirection.X * Math.Sin(-90 * (Math.PI / 180)) + straightDirection.Y * Math.Cos(-90 * (Math.PI / 180))));
            pointDirection[Direction.Right] = new Point((int)(straightDirection.X * Math.Cos(90 * (Math.PI / 180)) - straightDirection.Y * Math.Sin(90 * (Math.PI / 180))), (int)(straightDirection.X * Math.Sin(90 * (Math.PI / 180)) + straightDirection.Y * Math.Cos(90 * (Math.PI / 180))));
            pointDirection[Direction.Straight] = straightDirection;

            this.start = start;
            current = start;
            this.end = end;
        }

        public static HashSet<Point> SnakeByDots(List<Point> points)
        {
            var pointsOfSnake = new HashSet<Point>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        pointsOfSnake.Add(new Point(points[i].X + x * 64, points[i].Y + y * 64));
                    }
                }

                var snake = new RandomWalk(points[i], points[i + 1], NormalizePoint(points[i + 1] - points[i]));

                var curPoint = points[i];
                while (curPoint != points[i + 1])
                {
                    curPoint = snake.GetNextPoint();
                    pointsOfSnake.Add(curPoint);

                    if (points[i].Y - points[i + 1].Y == 0)
                    {
                        foreach (Point p in addNeighborsY(curPoint))
                        {
                            pointsOfSnake.Add(p);
                        }
                    }
                    else
                    {
                        foreach (Point p in addNeighborsX(curPoint))
                        {
                            pointsOfSnake.Add(p);
                        }
                    }
                }
            }

            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    pointsOfSnake.Add(new Point(points[points.Count - 1].X + x * 64, points[points.Count - 1].Y + y * 64));
                }
            }

            return pointsOfSnake;
        }

        static IEnumerable<Point> addNeighborsY(Point point)
        {
            for (int i = -2; i <= 2; i++)
            {
                if (i == 0) continue;
                yield return new Point(point.X, point.Y + i * 64);
            }
        }

        static IEnumerable<Point> addNeighborsX(Point point)
        {
            for (int i = -2; i <= 2; i++)
            {
                if (i == 0) continue;
                yield return new Point(point.X + i * 64, point.Y);
            }
        }

        public static Point NormalizePoint(Point point)
        {
            var newPoint = new Point(0, 0);
            if (point.X != 0)
            {
                newPoint.X = point.X / Math.Abs(point.X) * 64;
            }
            else if (point.Y != 0)
            {
                newPoint.Y = point.Y / Math.Abs(point.Y) * 64;
            }

            return newPoint;
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
                    if (pointDirection[Direction.Straight].Y == 0)
                    {
                        UpdateLikelyDirectionX();
                    }
                    else
                    {
                        UpdateLikelyDirectionY();
                    }
                    return current;
                }
            }
            return Point.Zero;
        }

        private void UpdateLikelyDirectionX()
        {
            if (current.X != end.X)
            {
                if (Math.Abs(current.X - end.X) <= 192 || Math.Abs(current.X - start.X) <= 192)
                {
                    if (lastDirection == Direction.Left || lastDirection == Direction.Right)
                    {
                        likelyDirection[Direction.Right] = 0;
                        likelyDirection[Direction.Left] = 0;
                        likelyDirection[Direction.Straight] = 3;
                        stepAfterRotate = 0;
                    }

                    if (lastDirection == Direction.Straight)
                    {
                        likelyDirection[Direction.Left] = Math.Abs((current + pointDirection[Direction.Left]).Y - end.Y) < Math.Abs(current.Y - end.Y) ? 1 : 0;
                        likelyDirection[Direction.Right] = Math.Abs((current + pointDirection[Direction.Right]).Y - end.Y) < Math.Abs(current.Y - end.Y) ? 1 : 0;
                        stepAfterRotate++;

                        if (Math.Abs(current.Y - end.Y) == 64 && stepAfterRotate > 2)
                        {
                            likelyDirection[Direction.Straight] = 0;
                        }
                        else
                        {
                            likelyDirection[Direction.Straight] = 3;
                        }
                    }
                }
                else if (Math.Abs(current.X - end.X) < 384 || Math.Abs(current.X - start.X) < 384)
                {
                    if (lastDirection == Direction.Left || lastDirection == Direction.Right)
                    {
                        likelyDirection[Direction.Right] = 0;
                        likelyDirection[Direction.Left] = 0;
                        likelyDirection[Direction.Straight] = 3;
                        stepAfterRotate = 0;
                    }

                    if (lastDirection == Direction.Straight)
                    {
                        likelyDirection[Direction.Left] = Math.Abs((current + pointDirection[Direction.Left]).Y - end.Y) < Math.Abs(current.Y - end.Y) ? 1 : 0;
                        likelyDirection[Direction.Right] = Math.Abs((current + pointDirection[Direction.Right]).Y - end.Y) < Math.Abs(current.Y - end.Y) ? 1 : 0;
                        stepAfterRotate++;

                        if (Math.Abs(current.Y - end.Y) == 128 && stepAfterRotate > 2)
                        {
                            likelyDirection[Direction.Straight] = 0;
                        }
                        else
                        {
                            likelyDirection[Direction.Straight] = 3;
                        }
                    }
                }
                else
                {
                    if (lastDirection == Direction.Left || lastDirection == Direction.Right)
                    {
                        likelyDirection[Direction.Right] = 0;
                        likelyDirection[Direction.Left] = 0;
                        stepAfterRotate = 0;
                    }

                    if (lastDirection == Direction.Straight)
                    {
                        likelyDirection[Direction.Left] = Math.Abs(end.Y - current.Y) < 128 && stepAfterRotate > 3 ? 1 : 0;
                        likelyDirection[Direction.Right] = Math.Abs(current.Y - end.Y) < 128 && stepAfterRotate > 3 ? 1 : 0;
                        stepAfterRotate++;
                    }
                }
            }
            else
            {
                likelyDirection[Direction.Straight] = 0;
                likelyDirection[Direction.Left] = Math.Abs((current + pointDirection[Direction.Left]).Y - end.Y) < Math.Abs(current.Y - end.Y) ? 1 : 0;
                likelyDirection[Direction.Right] = Math.Abs((current + pointDirection[Direction.Right]).Y - end.Y) < Math.Abs(current.Y - end.Y) ? 1 : 0;
                stepAfterRotate = 0;
            }
        }

        private void UpdateLikelyDirectionY()
        {
            if (current.Y != end.Y)
            {
                if (Math.Abs(current.Y - end.Y) <= 192 || Math.Abs(current.Y - start.Y) <= 192)
                {
                    if (lastDirection == Direction.Left || lastDirection == Direction.Right)
                    {
                        likelyDirection[Direction.Right] = 0;
                        likelyDirection[Direction.Left] = 0;
                        likelyDirection[Direction.Straight] = 3;
                        stepAfterRotate = 0;
                    }

                    if (lastDirection == Direction.Straight)
                    {
                        likelyDirection[Direction.Left] = Math.Abs((current + pointDirection[Direction.Left]).X - end.X) < Math.Abs(current.X - end.X) ? 1 : 0;
                        likelyDirection[Direction.Right] = Math.Abs((current + pointDirection[Direction.Right]).X - end.X) < Math.Abs(current.X - end.X) ? 1 : 0;
                        stepAfterRotate++;

                        if (Math.Abs(current.X - end.X) == 64 && stepAfterRotate > 2)
                        {
                            likelyDirection[Direction.Straight] = 0;
                        }
                        else
                        {
                            likelyDirection[Direction.Straight] = 3;
                        }
                    }
                }
                else if (Math.Abs(current.Y - end.Y) < 384 || Math.Abs(current.Y - start.Y) < 384)
                {
                    if (lastDirection == Direction.Left || lastDirection == Direction.Right)
                    {
                        likelyDirection[Direction.Right] = 0;
                        likelyDirection[Direction.Left] = 0;
                        likelyDirection[Direction.Straight] = 3;
                        stepAfterRotate = 0;
                    }

                    if (lastDirection == Direction.Straight)
                    {
                        likelyDirection[Direction.Left] = Math.Abs((current + pointDirection[Direction.Left]).X - end.X) < Math.Abs(current.X - end.X) ? 1 : 0;
                        likelyDirection[Direction.Right] = Math.Abs((current + pointDirection[Direction.Right]).X - end.X) < Math.Abs(current.X - end.X) ? 1 : 0;
                        stepAfterRotate++;

                        if (Math.Abs(current.X - end.X) == 128 && stepAfterRotate > 2)
                        {
                            likelyDirection[Direction.Straight] = 0;
                        }
                        else
                        {
                            likelyDirection[Direction.Straight] = 3;
                        }
                    }
                }
                else
                {
                    if (lastDirection == Direction.Left || lastDirection == Direction.Right)
                    {
                        likelyDirection[Direction.Right] = 0;
                        likelyDirection[Direction.Left] = 0;
                        stepAfterRotate = 0;
                    }

                    if (lastDirection == Direction.Straight)
                    {
                        likelyDirection[Direction.Left] = Math.Abs(current.X - end.X) < 128 && stepAfterRotate > 3 ? 1 : 0;
                        likelyDirection[Direction.Right] = Math.Abs(end.X - current.X) < 128 && stepAfterRotate > 3 ? 1 : 0;
                        stepAfterRotate++;
                    }
                }
            }
            else
            {
                likelyDirection[Direction.Straight] = 0;
                likelyDirection[Direction.Left] = end.X - current.X > 0 ? 1 : 0;
                likelyDirection[Direction.Right] = current.X - end.X > 0 ? 1 : 0;
                stepAfterRotate = 0;
            }
        }
    }
}
