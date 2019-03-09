using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepik.Introduction
{
    class Lab1_1
    {
        private void DoWork(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            Console.WriteLine(this.Fib(n));
        }

        private int Fib(int n)
        {
            if (n == 1 || n == 2)
            {
                return 1;
            }
            else
            {
                return this.Fib(n - 1) + this.Fib(n - 2);
            }

        }

        public static void Main(string[] args)
        {
            var app = new Lab1_1();
            app.DoWork(args);
        }
    }
}
