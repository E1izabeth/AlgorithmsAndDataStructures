using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week2
{
    class Lab2_4
    {
        private int k1, k2;

        private static void Swap(int[] array, int i, int j)
        {
            int tmp = array[i];
            array[i] = array[j];
            array[j] = tmp;
        }
        
        public void Quicksort(int[] elements, int left, int right)
        {
            while (true)
            {
                if (left > k2 || right < k1) return;

                int i = left, j = right;
                int pivot = elements[(left + right) / 2];

                while (i <= j)
                {
                    while (elements[i].CompareTo(pivot) < 0)
                    {
                        i++;
                    }

                    while (elements[j].CompareTo(pivot) > 0)
                    {
                        j--;
                    }

                    if (i > j) continue;

                    int tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    i++;
                    j--;
                }

                if (left < j)
                {
                    this.Quicksort(elements, left, j);
                }

                if (i < right)
                {
                    left = i;
                    this.Quicksort(elements, left, right);
                }

                break;
            }
        }

        private void DoWork(string[] args)
        {
            int n, A, B, C;
            int[] a;

            using (var file = new System.IO.StreamReader("input.txt"))
            {
                var input = file.ReadLine().Split(' ').Select(int.Parse).ToArray();
                n = input[0];
                k1 = input[1] - 1;
                k2 = input[2] - 1;
                input = file.ReadLine().Split().Select(int.Parse).ToArray();

                A = input[0];
                B = input[1];
                C = input[2];
                a = new int[n];
                a[0] = input[3];
                a[1] = input[4];
            }

            for (var i = 2; i < n; i++)
            {
                a[i] = A * a[i - 2] + B * a[i - 1] + C;
            }

            this.Quicksort(a, 0, a.Length - 1);
            using (var file = new System.IO.StreamWriter("output.txt"))
            {
                for (int i = k1; i <= k2; i++)
                {
                    file.Write($"{a[i]} ");
                }
            }
        }

        public static void Main(string[] args)
        {
            var app = new Lab2_4();
            app.DoWork(args);
        }
    }
}
