using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Openedu.Week1
{
    public class Lab1_5
    {
        public void Quicksort(double[] elements, int left, int right, StreamWriter file)
        {
            int i = left, j = right;
            var pivot = elements[(left + right) / 2];

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

                if (i <= j)
                {
                    // Swap
                    var tmp = elements[i];
                    elements[i] = elements[j];
                    elements[j] = tmp;

                    if (i != j)
                        file.WriteLine($"Swap elements at indices {i + 1} and {j + 1}.");

                    i++;
                    j--;
                }
            }

            // Recursive calls
            if (left < j)
            {
                this.Quicksort(elements, left, j, file);
            }

            if (i < right)
            {
                this.Quicksort(elements, i, right, file);
            }
        }

        private void DoWork(string[] args)
        {
            double[] array;

            using (var file = new System.IO.StreamReader("input.txt"))
            {
                var count = int.Parse(file.ReadLine());
                var input = file.ReadLine().Split(' ');
                array = new double[count];

                for (int i = 0; i < count; i++)
                {
                    array[i] = double.Parse(input[i]);
                }
            }


            using (var file = new System.IO.StreamWriter("output.txt"))
            {
                this.Quicksort(array, 0, array.Length - 1, file);
                file.WriteLine("No more swaps needed.");
                file.WriteLine(string.Join(" ", array));
            }
        }

        public static void Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            var app = new Lab1_5();
            app.DoWork(args);
        }
    }
}
