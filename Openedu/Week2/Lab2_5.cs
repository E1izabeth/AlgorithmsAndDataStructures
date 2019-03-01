using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week2
{
    class Lab2_5
    {
        public static bool ScarecrowSort(long[] array, int minStep)
        {
            List<List<long>> arrays = new List<List<long>>();
            for (int i = 0; i < minStep; i++)
            {
                arrays.Add(new List<long>());
                for (int j = i; j < array.Length; j += minStep)
                    arrays[i].Add(array[j]);
                MergeSort(arrays[i], 0, arrays[i].Count - 1);
            }

            array[0] = arrays[0][0];
            for (int i = 1; i < array.Length; i++)
            {
                int arrayIndex = i % minStep;
                int index = i / minStep;
                if (array[i - 1] > arrays[arrayIndex][index])
                    return false;
                else
                    array[i] = arrays[arrayIndex][index];
            }

            return true;
        }
        
        public static void MergeSort(List<long> array, int startIndex, int endIndex)
        {
            if (endIndex - startIndex == 0) return;

            int middleIndex = (endIndex + startIndex) / 2;
            MergeSort(array, startIndex, middleIndex);
            MergeSort(array, middleIndex + 1, endIndex);
            Merge(array, startIndex, middleIndex, endIndex);
        }

        public static void Merge(List<long> array, int startIndex, int middleIndex, int endIndex)
        {
            long[] result = new long[endIndex - startIndex + 1];
            int li = 0, ri = 0;

            while (li < middleIndex - startIndex + 1 && ri < endIndex - middleIndex)
                if (array[startIndex + li].CompareTo(array[middleIndex + ri + 1]) <= 0)
                    result[ri + li] = array[startIndex + li++];
                else
                    result[ri + li] = array[middleIndex + ++ri];

            while (li < middleIndex - startIndex + 1)
                result[ri + li] = array[startIndex + li++];

            while (ri < endIndex - middleIndex)
                result[ri + li] = array[middleIndex + ++ri];

            for (int i = 0; i < result.Length; i++)
                array[startIndex + i] = result[i];
        }

        private void DoWork(string[] args)
        {
            bool canBeSort;

            using (var file = new System.IO.StreamReader("input.txt"))
            {
                var arg = file.ReadLine().Split(' ').Select(x => int.Parse(x)).ToArray();
                var inputArray = file.ReadLine().Split(' ').Select(x => long.Parse(x)).ToArray();

                canBeSort = true;
                if (arg[1] != 1)
                    canBeSort = ScarecrowSort(inputArray, arg[1]);
            }
            
            using (var file = new System.IO.StreamWriter("output.txt"))
            {
                if (canBeSort)
                    file.WriteLine("YES");
                else
                    file.WriteLine("NO");
            }
        }

        public static void Main(string[] args)
        {
            var app = new Lab2_5();
            app.DoWork(args);
        }
    }
}
