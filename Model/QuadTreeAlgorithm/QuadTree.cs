using System;
using System.Collections.Generic;
using System.Linq;
using TankMonogame.Shared.Interface;
using TankMonogame.Model.GJKAlgorithm;

namespace TankMonogame.Model.QuadTreeAlgorithm
{
    public class QuadTree<T>
        where T : IGetBox<T>, IEquals<T>
    {
        private static readonly int Threshold = 3;
        private static readonly int MaxDepth = 6;

        private readonly BoxQT mBox;
        private readonly Node mRoot;
        private readonly Func<T, BoxQT> mGetBox;
        private readonly Func<T, T, bool> mEqual;

        public QuadTree(BoxQT box, Func<T, BoxQT> getBox = default, Func<T, T, bool> equal = default)
        {
            mBox = box;
            mRoot = new Node();
            mGetBox = getBox;
            mEqual = equal;
        }

        private class Node
        {
            public Node[] Children = new Node[4];
            public List<T> Values = new List<T>();
        }

        private bool IsLeaf(Node node)
        {
            return node.Children[0] == null;
        }

        public BoxQT ComputeBox(BoxQT box, int i)
        {
            var origin = box.GetTopLeft();
            var childSize = box.GetSize() / 2.0f;

            switch (i)
            {
                case 0: // North West
                    return new BoxQT(origin, childSize);
                case 1: // North East
                    return new BoxQT(new VectorQT(origin.X + childSize.X, origin.Y), childSize);
                case 2: // South West
                    return new BoxQT(new VectorQT(origin.X, origin.Y + childSize.Y), childSize);
                case 3: // South East
                    return new BoxQT(origin + childSize, childSize);
                default:
                    throw new ArgumentException("Invalid child index");
            }
        }

        public int GetQuadrant(BoxQT nodeBox, BoxQT valueBox)
        {
            var center = nodeBox.GetCenter();

            if (valueBox.GetRight() < center.X)
            {
                if (valueBox.GetBottom() < center.Y)
                    return 0;
                else if (valueBox.GetTop() >= center.Y)
                    return 2;
                else
                    return -1;
            }
            else if (valueBox.GetLeft() >= center.X)
            {
                if (valueBox.GetBottom() < center.Y)
                    return 1;
                else if (valueBox.GetTop() >= center.Y)
                    return 3;
                else
                    return -1;
            }
            else
                return -1;
        }

        public void Add(T value)
        {
            add(mRoot, 0, mBox, value);
        }

        private void add(Node node, int depth, BoxQT box, T value)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (!box.Contains(mGetBox.Invoke(value)))
            {
                throw new InvalidOperationException("Box does not contain the value.");
            }

            if (IsLeaf(node))
            {
                if (depth >= MaxDepth || node.Values.Count < Threshold)
                {
                    node.Values.Add(value);
                }
                else
                {
                    Split(node, box);
                    add(node, depth, box, value);
                }
            }
            else
            {
                int i = GetQuadrant(box, mGetBox.Invoke(value));

                if (i != -1)
                {
                    add(node.Children[i], depth + 1, ComputeBox(box, i), value);
                }
                else
                {
                    node.Values.Add(value);
                }
            }
        }

        private void Split(Node node, BoxQT box)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (!IsLeaf(node))
            {
                throw new InvalidOperationException("Only leaves can be split.");
            }

            for (int i = 0; i < node.Children.Length; i++)
            {
                node.Children[i] = new Node();
            }

            var newValues = new List<T>();
            foreach (var value in node.Values)
            {
                int quadrantIndex = GetQuadrant(box, mGetBox.Invoke(value));
                if (quadrantIndex != -1)
                {
                    node.Children[quadrantIndex].Values.Add(value);
                }
                else
                {
                    newValues.Add(value);
                }
            }

            node.Values = newValues;
        }

        public void Remove(T value)
        {
            remove(mRoot, null, mBox, value);
        }

        void remove(Node node, Node parent, BoxQT box, T value)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (!box.Contains(mGetBox.Invoke(value)))
            {
                throw new InvalidOperationException("There is an element in the area");
            }

            if (IsLeaf(node))
            {
                RemoveValue(node, value);

                if (parent != null)
                    TryMerge(parent);
            }
            else
            {
                int i = GetQuadrant(box, mGetBox.Invoke(value));
                if (i != -1)
                    remove(node.Children[i], node, ComputeBox(box, i), value);
                else
                    RemoveValue(node, value);
            }
        }

        void RemoveValue(Node node, T value)
        {
            var it = node.Values.FindIndex(rhs => mEqual.Invoke(value, rhs));
            if (it == -1)
            {
                throw new InvalidOperationException("There is no an element in the area");
            }

            node.Values[it] = node.Values[node.Values.Count - 1];
            node.Values.RemoveAt(node.Values.Count - 1);
        }

        void TryMerge(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (IsLeaf(node))
            {
                throw new InvalidOperationException("Only no leaves can be merge.");
            }

            int nbValues = node.Values.Count;
            foreach (var child in node.Children)
            {
                if (!IsLeaf(child))
                    return;
                nbValues += child.Values.Count;
            }
            if (nbValues <= Threshold)
            {
                node.Values.Capacity = nbValues;
                foreach (var child in node.Children)
                {
                    foreach (var value in child.Values)
                        node.Values.Add(value);
                }
                for (int i = 0; i < node.Children.Count() - 1; i++)
                {
                    node.Children[i] = null;
                }
            }
        }

        public List<T> Query(BoxQT box)
        {
            var values = new List<T>();
            query(mRoot, mBox, box, values);
            return values;
        }

        private void query(Node node, BoxQT box, BoxQT queryBox, List<T> values)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (!GJK.DefinitionOfCollision(box, queryBox))
            {
                throw new InvalidOperationException("Only no leaves can be merge.");
            }

            foreach (var value in node.Values)
            {
                if (GJK.DefinitionOfCollision(mGetBox(value), queryBox))
                {
                    values.Add(value);
                }
            }

            if (!IsLeaf(node))
            {
                for (int i = 0; i < node.Children.Count(); ++i)
                {
                    var childBox = ComputeBox(box, i);
                    if (GJK.DefinitionOfCollision(childBox, queryBox))
                    {
                        query(node.Children[i], childBox, queryBox, values);
                    }
                }
            }
        }
    //    public List<Tuple<T, T>> FindAllIntersections()
    //    {
    //        var intersections = new List<Tuple<T, T>>();
    //        findAllIntersections(mRoot, intersections);
    //        return intersections;
    //    }

    //    void findAllIntersections(Node node, List<Tuple<T, T>> intersections)
    //    {
    //        for (int i = 0; i < node.Values.Count; ++i)
    //        {
    //            for (int j = 0; j < i; ++j)
    //            {
    //                if (mGetBox(node.Values[i]).Intersects(mGetBox(node.Values[j])))
    //                    intersections.Add(new Tuple<T, T>(node.Values[i], node.Values[j]));
    //            }
    //        }

    //        if (!IsLeaf(node))
    //        {
    //            foreach (var child in node.Children)
    //            {
    //                foreach (var value in node.Values)
    //                    findIntersectionsInDescendants(child, value, intersections);
    //            }

    //            foreach (var child in node.Children)
    //                findAllIntersections(child, intersections);
    //        }
    //    }

    //    void findIntersectionsInDescendants(Node node, T value, List<Tuple<T, T>> intersections)
    //    {
    //        foreach (var other in node.Values)
    //        {
    //            if (mGetBox(value).Intersects(mGetBox(other)))
    //                intersections.Add(new Tuple<T, T>(value, other));
    //        }

    //        if (!IsLeaf(node))
    //        {
    //            foreach (var child in node.Children)
    //            {
    //                findIntersectionsInDescendants(child, value, intersections);
    //            }
    //        }
    //    }

    }
}
