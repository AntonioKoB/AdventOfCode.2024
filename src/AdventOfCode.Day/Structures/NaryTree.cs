using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day.Structures
{
    public class NaryTree<T>
    {
        public NaryTreeNode<T> Root { get; set; }

        public NaryTree(T rootValue)
        {
            Root = new NaryTreeNode<T>(rootValue);
        }

        public List<T> FindPathsToLeaf(Func<T, bool> isValidLeaf)
        {
            var leafs = new List<T>();

            void FindPaths(NaryTreeNode<T> currentNode)
            {
                if (currentNode == null)
                {
                    return;
                }

                if (isValidLeaf.Invoke(currentNode.Value))
                {
                    leafs.Add(currentNode.Value);
                }
                else
                {
                    foreach (var child in currentNode.Children)
                    {
                        FindPaths(child);
                    }
                }
            }

            FindPaths(Root);
            return leafs;
        }

        public void PrintTree(NaryTreeNode<T> node, Action<T> printValueAction, string indent = "", bool isLast = true)
        {
            // Print the current node value
            Console.Write(indent);
            if (isLast)
            {
                Console.Write("└─");
                indent += "  ";
            }
            else
            {
                Console.Write("├─");
                indent += "| ";
            }

            printValueAction(node.Value);

            // Recursively print each child
            for (int i = 0; i < node.Children.Count; i++)
            {
                PrintTree(node.Children[i], printValueAction, indent, i == node.Children.Count - 1);
            }
        }

    }

    public class NaryTreeNode<T>
    {
        public T Value { get; set; }
        public List<NaryTreeNode<T>> Children { get; set; }

        public NaryTreeNode(T value)
        {
            Value = value;
            Children = new List<NaryTreeNode<T>>();
        }
    }
}
