using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week3
{
    class Lab3_1
    {
        private static void Sort(ref long[] A, long n, long maxElement)
        {
            long[] B = new long[n];
            long[] C = new long[256];

            for (int pow = 0; (long)1 << pow <= maxElement; pow += 8)
            {
                for (int i = 0; i < 256; i++)
                {
                    C[i] = 0;
                }

                for (int i = 0; i < n; i++)
                {
                    C[(A[i] >> pow) & 255]++;
                }

                for (int i = 1; i < 256; i++)
                {
                    C[i] += C[i - 1];
                }

                for (long i = n - 1; i >= 0; i--)
                {
                    B[C[(A[i] >> pow) & 255] - 1] = A[i];
                    C[(A[i] >> pow) & 255]--;
                }

                for (int i = 0; i < n; i++)
                {
                    A[i] = B[i];
                }
            }
        }

        private void DoWork(string[] args)
        {
            long[] arr1, arr2, arr3;
            using (var input = new StreamReader("input.txt"))
            {
                var arr = input.ReadLine().Split(' ').Select(t => int.Parse(t)).ToArray();

                arr1 = new long[arr[0]];
                var t1 = input.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < t1.Length; i++)
                    arr1[i] = long.Parse(t1[i]);
                arr2 = new long[arr[1]];
                var t2 = input.ReadLine().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < t2.Length; i++)
                    arr2[i] = long.Parse(t2[i]);
            }
            arr3 = new long[arr1.Length * arr2.Length];

            long maxElement = -1;
            for (int i = 0; i < arr1.Length; i++)
                for (int j = 0; j < arr2.Length; j++)
                {
                    var c = arr1[i] * arr2[j];
                    arr3[i * arr2.Length + j] = c;
                    maxElement = maxElement > c ? maxElement : c;
                }

            Sort(ref arr3, arr1.Length * arr2.Length, maxElement);

            long s = 0;
            for (int i = 0; i < arr3.Length; i += 10)
                s += arr3[i];
            using (var output = new StreamWriter("output.txt"))
            {
                output.WriteLine(s);
            }
        }

        public static void Main(string[] args)
        {
            var app = new Lab3_1();
            app.DoWork(args);
        }
    }
}
