using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week2
{
    class Lab1_3
    {
        /**
         * Метод для определения элемента в последовательности периодов Пизано
         *
         * @param divider делитель
         * @return элемента последовательности периода Пизано
         */
        private static long CalcPisanoPeriods(long divider)
        {
            long a = 0;
            long b = 1;
            long c;

            for (long i = 0L; i < divider * divider; i++)
            {
                c = (a + b) % divider;
                a = b;
                b = c;

                if (a == 0 && b == 1)
                {
                    return i + 1;
                }
            }
            return a;
        }

        private long GetHugeFibonacci(long elementNumber, long divider)
        {
            long remainder = elementNumber % CalcPisanoPeriods(divider);

            long first = 0L;
            long second = 1L;

            long result = remainder;

            for (int i = 1; i < remainder; i++)
            {
                result = (first + second) % divider;
                first = second;
                second = result;
            }

            return result % divider;
        }

        private void DoWork(string[] args)
        {
            var input = Console.ReadLine().Split(' ').Select(long.Parse).ToArray();
            Console.WriteLine(this.GetHugeFibonacci(input[0], input[1]));
        }
        
        public static void Main(string[] args)
        {
            var app = new Lab1_3();
            app.DoWork(args);
        }
    }
}
