using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week2
{
    class Lab2_3
    {
        private int[] MakeAntiQuickSortArray(int n)
        {
            int[] array = new int[n];
            for (int i = 0; i < n; i++)
                array[i] = i + 1;

            for (int i = 2; i < n; i++)
            {
                int temp = array[i / 2];
                array[i / 2] = array[i];
                array[i] = temp;
            }
            return array;
        }

        private void DoWork(string[] args)
        {
            int n;
            int[] array;

            using (var file = new System.IO.StreamReader("input.txt"))
            {
                n = int.Parse(file.ReadLine());
            }

            array = this.MakeAntiQuickSortArray(n);

            using (var file = new System.IO.StreamWriter("output.txt"))
            {
                for (int i = 0; i < n; i++)
                {
                    file.Write($"{array[i]} ");
                }
            }
        }

        public static void Main(string[] args)
        {
            var app = new Lab2_3();
            app.DoWork(args);
        }
    }
}
