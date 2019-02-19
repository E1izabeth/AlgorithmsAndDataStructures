using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week1
{
    public class Lab1_3
    {
        public int[] InsertionSort(int[] array, int[] indexesDiff)
        {
            int[] result = new int[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                int j = i;
                while (j > 0 && result[j - 1] > array[i])
                {
                    result[j] = result[j - 1];
                    j--;
                }
                indexesDiff[i] = j;
                result[j] = array[i];
            }
            return result;
        }
        
        private void DoWork(string[] args)
        {
            int[] array;
            int[] indexesDiff;

            using (var file = new System.IO.StreamReader("input.txt"))
            {
                var count = int.Parse(file.ReadLine());
                var input = file.ReadLine().Split(' ');
                array = new int[count];
                indexesDiff = new int[count];

                for (int i = 0; i < count; i++)
                {
                    array[i] = int.Parse(input[i]);
                }
            }

            var result = this.InsertionSort(array, indexesDiff);

            using (var file = new System.IO.StreamWriter("output.txt"))
            {
                for (int i = 0; i < array.Length; i++)
                {
                    file.Write($"{indexesDiff[i] + 1} ");
                }

                file.WriteLine();

                for (int i = 0; i < array.Length; i++)
                {
                    file.Write($"{result[i]} ");
                }
            }
        }

        public static void Main(string[] args)
        {
            var app = new Lab1_3();
            app.DoWork(args);
        }
    }
}
