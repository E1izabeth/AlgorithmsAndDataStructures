using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week3
{
    class Lab3_2
    {
        public static void Main(string[] args)
        {
            var app = new Lab3_2();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            FileStream fs = new FileStream("output.txt", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            string[] stdin = File.ReadAllLines("input.txt");

            int[] parameters = stdin[0].Split(' ').Select(x => int.Parse(x)).ToArray();

            int[] arrayIndexer = new int[parameters[0] + 1];
            for (int n = 0; n < parameters[0] + 1; n++)
                arrayIndexer[n] = n;

            RadixSort(ref stdin, ref arrayIndexer, parameters[0], parameters[1], parameters[2]);

            for (int i = 1; i < arrayIndexer.Length; i++)
                sw.Write("{0} ", arrayIndexer[i]);

            sw.Dispose();
        }

        static void RadixSort(ref string[] array, ref int[] arrayIndexer, int n, int m, int k)
        {
            for (int i = 0; i < k; i++)
                CountingSort(ref array, ref arrayIndexer, n, m, i);
        }

        static void CountingSort(ref string[] array, ref int[] arrayIndexer, int n, int m, int phase)
        {
            int k = m - phase;

            int[] resultIndexer = new int[arrayIndexer.Length];
            int[] countingArray = new int[130];

            for (int i = 0; i < array[k].Length; i++)
                countingArray[array[k][i]]++;
            for (int i = 98; i <= 125; i++)
                countingArray[i] += countingArray[i - 1];
            for (int i = array[k].Length; i > 0; i--)
            {
                int charValue = array[k][arrayIndexer[i] - 1];
                int countedIndex = countingArray[charValue];
                resultIndexer[countedIndex] = arrayIndexer[i];
                countingArray[charValue]--;
            }

            arrayIndexer = resultIndexer;

            if (GC.GetTotalMemory(false) / 1024 / 1024 > 200)
                GC.Collect();
        }
    }
}
