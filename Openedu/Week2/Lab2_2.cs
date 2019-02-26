using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week2
{
    class Lab2_2
    {
        private long _inversions = 0;

        public void MergeSort<T>(T[] array, long start, long end) where T : IComparable<T>
        {
            if (end - start == 0)
            {
                return;
            }
            else
            {
                long middle = (end + start) / 2;
                this.MergeSort(array, start, middle);
                this.MergeSort(array, middle + 1, end);
                this.Merge(array, start, middle, end);
            }
        }

        public void Merge<T>(T[] array, long start, long middle, long end) where T : IComparable<T>
        {
            T[] result = new T[end - start + 1];
            long li = 0, ri = 0;

            while (li < middle - start + 1 && ri < end - middle)
            {
                if (array[start + li].CompareTo(array[middle + ri + 1]) <= 0)
                {
                    result[ri + li] = array[start + li];
                    li++;
                }
                else
                {
                    //bacause of subarrays sorted yet
                    _inversions += middle - start - li + 1;
                    result[ri + li] = array[middle + ++ri];
                }
            }

            while (li < middle - start + 1)
            {
                result[ri + li] = array[start + li];
                li++;
            }

            while (ri < end - middle)
            {
                result[ri + li] = array[middle + ++ri];
                //ri++;
            }

            for (int i = 0; i < result.Length; i++)
                array[start + i] = result[i];
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

            this.MergeSort(array, 0, array.Length - 1);


            using (var file = new System.IO.StreamWriter("output.txt"))
            {
                file.Write(_inversions);
            }
        }

        public static void Main(string[] args)
        {
            var app = new Lab2_2();
            app.DoWork(args);
        }
    }
}
