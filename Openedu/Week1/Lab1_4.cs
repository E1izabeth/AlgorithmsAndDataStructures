using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Openedu.Week1
{
    public class Lab1_4
    {
        public double[] InsertionSort(double[] array, int[] ids)
        {
            double[] result = new double[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                int j = i;
                while (j > 0 && result[j - 1] > array[i])
                {
                    var t = ids[j];
                    ids[j] = ids[j - 1];
                    ids[j - 1] = t;
                    result[j] = result[j - 1];
                    j--;
                }
                result[j] = array[i];
            }
            return result;
        }

        private void DoWork(string[] args)
        {
            double[] array;
            int[] ids;

            using (var file = new System.IO.StreamReader("input.txt"))
            {
                var count = int.Parse(file.ReadLine());
                var input = file.ReadLine().Split(' ');
                array = new double[count];
                ids = new int[count];

                for (int i = 0; i < ids.Length; i++)
                {
                    ids[i] = i + 1;
                }

                for (int i = 0; i < count; i++)
                {
                    array[i] = double.Parse(input[i]);
                }
            }

            var result = this.InsertionSort(array, ids);

            using (var file = new System.IO.StreamWriter("output.txt"))
            {
                file.Write($"{ids[0]} {ids[ids.Length / 2]} {ids[ids.Length - 1]}");
            }
        }

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            var app = new Lab1_4();
            app.DoWork(args);
        }
    }
}
