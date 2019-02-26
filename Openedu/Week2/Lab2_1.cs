using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week2
{
    class Lab2_1
    {
        public void MergeSort<T>(T[] array, long startIndex, long endIndex, StreamWriter sw) where T : IComparable<T>
        {
            if (endIndex - startIndex == 0)
            {
                return;
            }
            else
            {
                long middleIndex = (endIndex + startIndex) / 2;
                this.MergeSort(array, startIndex, middleIndex, sw);
                this.MergeSort(array, middleIndex + 1, endIndex, sw);
                this.Merge(array, startIndex, middleIndex, endIndex, sw);
            }
        }

        public void Merge<T>(T[] array, long startIndex, long middleIndex, long endIndex, StreamWriter sw) where T : IComparable<T>
        {
            T[] result = new T[endIndex - startIndex + 1];
            long li = 0, ri = 0;

            while (li < middleIndex - startIndex + 1 && ri < endIndex - middleIndex)
            {
                if (array[startIndex + li].CompareTo(array[middleIndex + ri + 1]) <= 0)
                {

                    result[ri + li] = array[startIndex + li++];
                }
                else
                {
                    result[ri + li] = array[middleIndex + ++ri];
                }
            }

            while (li < middleIndex - startIndex + 1)
            {
                result[ri + li] = array[startIndex + li++];
            }

            while (ri < endIndex - middleIndex)
            {
                result[ri + li] = array[middleIndex + ++ri];
            }

            for (int i = 0; i < result.Length; i++)
                array[startIndex + i] = result[i];

            sw.WriteLine("{0} {1} {2} {3}", startIndex + 1, endIndex + 1, array[startIndex], array[endIndex]);
        }

        private void DoWork(string[] args)
        {
            int[] array;

            using (var file = new System.IO.StreamReader("input.txt"))
            {
                var count = int.Parse(file.ReadLine());
                var input = file.ReadLine().Split(' ');
                array = new int[count];

                for (int i = 0; i < count; i++)
                {
                    array[i] = int.Parse(input[i]);
                }
            }
            
            using (var file = new System.IO.StreamWriter("output.txt"))
            {
                this.MergeSort(array, 0, array.Length - 1, file);

                for (int i = 0; i < array.Length; i++)
                {
                    file.Write($"{array[i]} ");
                }
            }
        }

        public static void Main(string[] args)
        {
            var app = new Lab2_1();
            app.DoWork(args);
        }
    }
}
