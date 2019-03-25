using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week5
{
    class Lab5_1
    {
        public static void Main(string[] args)
        {
            var app = new Lab5_1();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                int[] stdin = File.ReadAllLines("input.txt")[1].Split(' ').Select(x => int.Parse(x)).ToArray();
                if (IsHeap(stdin))
                    sw.WriteLine("YES");
                else
                    sw.WriteLine("NO");
            }
        }

        static bool IsHeap(int[] array)
        {
            bool isHeap = true;
            for (int i = array.Length / 2 - 1; i >= 0 && isHeap; i--)
            {
                var index = i + 1;
                int rigthChildIndex = index * 2;
                int leftChildIndex = index * 2 - 1;

                if (leftChildIndex < array.Length && array[leftChildIndex] < array[index - 1])
                    isHeap = false;
                if (rigthChildIndex < array.Length && array[rigthChildIndex] < array[index - 1])
                    isHeap = false;
            }
            return isHeap;
        }
    }
}