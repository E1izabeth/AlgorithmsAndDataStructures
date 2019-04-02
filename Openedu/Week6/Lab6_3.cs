using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week6
{
    class Lab6_3
    {
        class Tree
        {
            class Node
            {
                public int Key { get; set; }
                public int Parent { get; set; }
                public int Left { get; set; }
                public int Right { get; set; }
            }

            public int Height { get { if (_vertexes != null) return this.CalcHeight(); else return -1; } }

            private int CalcHeight()
            {
                int heigth = 0;
                for (int i = 0; i < _rootIndexes.Count; i++)
                {
                    int tempHeigth = 1;
                    int currentNode = _rootIndexes[i];
                    while (_vertexes[currentNode].Parent != 0)
                    {
                        tempHeigth++;
                        currentNode = _vertexes[currentNode].Parent;
                    }
                    if (heigth < tempHeigth)
                        heigth = tempHeigth;
                }
                return heigth;
            }

            Node[] _vertexes;
            List<int> _rootIndexes;

            public void MakeTree(string[] stdin)
            {
                _rootIndexes = new List<int>();
                _vertexes = new Node[stdin.Length];
                for (int i = 1; i < stdin.Length; i++)
                {
                    int[] temp = stdin[i].Split(' ').Select(x => int.Parse(x)).ToArray();
                    if (_vertexes[i] == null)
                        _vertexes[i] = new Node();
                    _vertexes[i].Key = temp[0];
                    _vertexes[i].Left = temp[1];
                    _vertexes[i].Right = temp[2];

                    if (temp[1] != 0)
                    {
                        _vertexes[temp[1]] = new Node();
                        _vertexes[temp[1]].Parent = i;
                    }
                    if (temp[2] != 0)
                    {
                        _vertexes[temp[2]] = new Node();
                        _vertexes[temp[2]].Parent = i;
                    }
                    if (temp[1] == 0 && temp[2] == 0)
                        _rootIndexes.Add(i);
                }
            }
        }

        public static void Main(string[] args)
        {
            var app = new Lab6_3();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (var sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                if (int.Parse(stdin[0]) == 0)
                    sw.Write(0);
                else
                {
                    var tree = new Tree();
                    tree.MakeTree(stdin);
                    sw.WriteLine(tree.Height);
                }
            }
        }
    }

}

