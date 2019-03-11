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
        private int[] countingArray = new int[130];
        private int[] resultIndexer;

        public static void Main(string[] args)
        {
            var app = new Lab3_2();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");

                int[] parameters = stdin[0].Split(' ').Select(x => int.Parse(x)).ToArray();

                int[] arrayIndexer = new int[parameters[0] + 1];
                for (int n = 0; n < parameters[0] + 1; n++)
                    arrayIndexer[n] = n;

                arrayIndexer = this.RadixSort(stdin, arrayIndexer, parameters[0], parameters[1], parameters[2]);

                for (int i = 1; i < arrayIndexer.Length; i++)
                    sw.Write("{0} ", arrayIndexer[i]);
            }
        }

        private int[] RadixSort(string[] array, int[] arrayIndexer, int n, int m, int k)
        {
            for (int i = 0; i < k; i++)
            {
                resultIndexer = new int[arrayIndexer.Length];
                arrayIndexer = this.CountingSort(array, arrayIndexer, n, m, i);
            }
            return arrayIndexer;
        }

        private int[] CountingSort(string[] array, int[] arrayIndexer, int n, int m, int phase)
        {
            int k = m - phase;

            for (int i = 0; i < countingArray.Length; i++)
            {
                countingArray[i] = 0;
            }

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

            return resultIndexer;
        }
    }
}
