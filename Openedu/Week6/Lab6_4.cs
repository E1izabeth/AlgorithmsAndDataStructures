using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week6
{

    class Lab6_4
    {
        public class Tree
        {
            public class Node
            {
                public int Key { get; set; }
                public Node Parent { get; set; }
                public Node Left { get; set; }
                public Node Right { get; set; }
            }

            public long Size { get; private set; }
            public Node[] Vertexes { get; private set; }
            List<int> _rootIndexes;

            public Tree(long n)
            {
                this.Size = n;
            }

            public void MakeTree(string[] stdin)
            {
                _rootIndexes = new List<int>();
                this.Vertexes = new Node[this.Size];
                for (int i = 1; i <= this.Size; i++)
                {
                    int[] temp = stdin[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                    if (this.Vertexes[i - 1] == null)
                        this.Vertexes[i - 1] = new Node();
                    this.Vertexes[i - 1].Key = temp[0];

                    if (temp[1] != 0)
                    {
                        //Left
                        if (temp[1] != 0)
                        {
                            if (this.Vertexes[temp[1] - 1] == null)
                                this.Vertexes[temp[1] - 1] = new Node() { Parent = this.Vertexes[i - 1] };
                            this.Vertexes[i - 1].Left = this.Vertexes[temp[1] - 1];
                        }
                    }
                    //Righ
                    if (temp[2] != 0)
                    {
                        if (temp[2] != 0 && this.Vertexes[temp[2] - 1] == null)
                            this.Vertexes[temp[2] - 1] = new Node() { Parent = this.Vertexes[i - 1] };
                        this.Vertexes[i - 1].Right = this.Vertexes[temp[2] - 1];
                    }
                }
            }

            public long RemoveNode(long request, Node root)
            {
                if (root != null)
                {
                    Node node = this.Search(root, request);
                    if (node != null)
                    {
                        if (node == root)
                        {
                            root = null;
                            return 0;
                        }
                        else
                        {
                            if (node.Parent.Right == node)
                                node.Parent.Right = null;
                            else
                                node.Parent.Left = null;
                        }
                        this.Size -= this.Count(node);

                    }
                }
                return this.Size;
            }
            public long Count(Node node)
            {
                long result = 1;
                if (node.Left != null)
                    result += this.Count(node.Left);
                if (node.Right != null)
                    result += this.Count(node.Right);
                return result;
            }

            public Node Search(Node node, long value)
            {
                while (value != node.Key && 
                    (value < node.Key && node.Left != null) || (value > node.Key && node.Right != null))
                {
                    if (value < node.Key && node.Left != null)
                        node = node.Left;
                    if (value > node.Key && node.Right != null)
                        node = node.Right;
                }

                if (value == node.Key)
                    return node;
                else
                    return null;
            }

        }

        public static void Main(string[] args)
        {
            var app = new Lab6_4();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (var sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                long n = long.Parse(stdin[0]);
                var tree = new Tree(n);
                tree.MakeTree(stdin);
                long[] requests = stdin[n + 2].Split(' ').Select(x => long.Parse(x)).ToArray();
                var root = tree.Vertexes[0];
                while (root.Parent != null)
                    root = root.Parent;
                for (int i = 0; i < requests.Length; i++)
                {
                    sw.WriteLine(tree.RemoveNode(requests[i], root));
                }
            }

        }
    }
}

