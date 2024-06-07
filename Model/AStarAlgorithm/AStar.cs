using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TankMonogame.Model.AStarAlgorithm
{
    public class AStar
    {
        public class Node
        {
            private static double Radius = 7.0;

            public readonly Vector2 Position;
            public readonly float Angle;
            public Node(Vector2 point, float angle)
            {
                Position = point;
                Angle = angle;
            }

            public IEnumerable<Node> GetNeighbors()
            {
                for (float theta = Angle-0.5f; theta <= Angle+0.5; theta += 0.2f)
                {
                    int x = (int)Math.Round(Position.X + Radius * Math.Cos(theta));
                    int y = (int)Math.Round(Position.Y + Radius * Math.Sin(theta));
                    yield return new Node(new Vector2(x, y), theta);
                }
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                    return false;

                Node other = (Node)obj;
                return (Position.X == other.Position.X) && (Position.Y == other.Position.Y);
            }

            public static int GetHeuristic(Node node1, Node node2)
            {
                double deltaX = node2.Position.X - node1.Position.X;
                double deltaY = node2.Position.Y - node1.Position.Y;
                return (int)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Position);
            }
        }

        public static List<Node> FindShortestPath(Node start, Node goal, Func<Node, Node, int> heuristic)
        {
            var openSet = new PriorityQueue<Node, int>();
            var cameFrom = new Dictionary<Node, Node>();
            var gScore = new Dictionary<Node, int>();
            var fScore = new Dictionary<Node, int>();

            openSet.Enqueue(start, 0);
            gScore[start] = 0;
            fScore[start] = heuristic(start, goal);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current.Equals(goal))
                    return ReconstructPath(cameFrom, current);

                foreach (var neighbor in current.GetNeighbors())
                {
                    int tentativeGScore = gScore[current] + 1;

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + heuristic(neighbor, goal);

                        if (!openSet.Contains(neighbor))
                            openSet.Enqueue(neighbor, fScore[neighbor]);
                    }
                }
            }

            return new List<Node>();
        }

        private static List<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
        {
            var totalPath = new List<Node> { current };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                totalPath.Insert(0, current);
            }

            return totalPath;
        }
    }

    public class PriorityQueue<TElement, TPriority>
    {
        private SortedDictionary<TPriority, Queue<TElement>> dictionary = new SortedDictionary<TPriority, Queue<TElement>>();

        public void Enqueue(TElement element, TPriority priority)
        {
            if (!dictionary.ContainsKey(priority))
            {
                dictionary[priority] = new Queue<TElement>();
            }
            dictionary[priority].Enqueue(element);
        }

        public TElement Dequeue()
        {
            if (dictionary.Count == 0) throw new InvalidOperationException("The queue is empty.");

            var firstPair = dictionary.First();
            var element = firstPair.Value.Dequeue();

            if (firstPair.Value.Count == 0)
            {
                dictionary.Remove(firstPair.Key);
            }

            return element;
        }

        public bool Contains(TElement element)
        {
            return dictionary.Any(p => p.Value.Contains(element));
        }

        public int Count => dictionary.Sum(p => p.Value.Count);
    }
}
