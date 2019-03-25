using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week5
{
    class Lab5_2
    {
        public static void Main(string[] args)
        {
            var app = new Lab5_2();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                Console.SetOut(sw);
                QueueWithPriorities queue = new QueueWithPriorities();
                
                for (int i = 1; i < stdin.Length; i++)
                    switch (stdin[i][0]) { 
                        case 'A': queue.Insert(i, int.Parse(stdin[i].Split(' ')[1])); break;
                        case 'X': queue.Extract(); break;
                        case 'D': queue.Decrease(int.Parse(stdin[i].Split(' ')[1]), int.Parse(stdin[i].Split(' ')[2])); break;
                    }
            }
        }

        
        public class QueueWithPriorities
        {
            public class Element
            {
                public int CurrentIndex { get; set; }
                public long Value { get; set; }
            }

            public Dictionary<int, Element> References = new Dictionary<int, Element>();
            public Element[] array = new Element[6000000];
            public Element Top { get { return array[0]; } }
            public int HeapSize { get; private set; }

            public void Extract()
            {
                if (this.HeapSize == 0)
                    Console.WriteLine("*");
                else
                {
                    Console.WriteLine(this.Top.Value);
                    this.HeapSize--;
                    this.SwapElementsWithIndexes(0, this.HeapSize);
                    this.Heapify(0);
                }
            }

            public void Decrease(int lineIndex, int value)
            {
                int index = References[lineIndex].CurrentIndex;
                array[index].Value = value;
                while (index > 0 && array[this.Parent(index)].Value > array[index].Value)
                {
                    this.SwapElementsWithIndexes(index, this.Parent(index));
                    index = this.Parent(index);
                }
            }

            public void Insert(int lineIndex, int value)
            {
                array[this.HeapSize] = new Element { Value = int.MaxValue, CurrentIndex = HeapSize };
                References.Add(lineIndex, array[this.HeapSize]);
                this.HeapSize++;
                this.Decrease(lineIndex, value);
            }

            private int Parent(int index)
            {
                return (index + 1) / 2 - 1;
            }

            private void Heapify(int index)
            {
                int rightChildIndex = (index + 1) * 2;
                int leftChildIndex = rightChildIndex - 1;
                int lowestIndex = int.MinValue;

                if (leftChildIndex < this.HeapSize && array[leftChildIndex].Value < array[index].Value)
                    lowestIndex = leftChildIndex;
                else
                    lowestIndex = index;

                if (rightChildIndex < this.HeapSize && array[rightChildIndex].Value < array[lowestIndex].Value)
                    lowestIndex = rightChildIndex;

                if (lowestIndex != index)
                {
                    this.SwapElementsWithIndexes(lowestIndex, index);
                    this.Heapify(lowestIndex);
                }
            }

            private void SwapElementsWithIndexes(int a, int b)
            {
                array[a].CurrentIndex = b;
                array[b].CurrentIndex = a;
                Element temp = array[a];
                array[a] = array[b];
                array[b] = temp;
            }
        }
    }
}