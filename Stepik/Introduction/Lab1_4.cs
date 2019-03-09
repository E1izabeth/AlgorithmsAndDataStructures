using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepik.Introduction
{
    class Lab1_4
    {
        private int GetNOD(int firstValue, int secondValue)
        {
            if (firstValue == 0)
            {
                return secondValue;
            }
            else if (secondValue == 0)
            {
                return firstValue;
            }
            else if (firstValue >= secondValue)
            {
                return GetNOD(firstValue % secondValue, secondValue);
            }
            else
            {
                return GetNOD(firstValue, secondValue % firstValue);
            }
        }

        private void DoWork(string[] args)
        {
            var input = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            Console.WriteLine(this.GetNOD(input[0], input[1]));
        }

        public static void Main(string[] args)
        {
            var app = new Lab1_4();
            app.DoWork(args);
        }
    }
}
