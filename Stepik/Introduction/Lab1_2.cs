using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stepik.Introduction
{
    class Lab1_2
    {
        private void DoWork(string[] args)
        {
            var n = int.Parse(Console.ReadLine());
            Console.WriteLine(this.Fib(n));
        }

        private int Fib(int n)
        {
            int[] f = new int[2];
            f[0] = 1; f[1] = 1;
            for (int i = 2; i <= (n - 1); i++)
                f[i % 2] = (f[i % 2] + f[(i + 1) % 2]) % 10;
            return f[(n - 1) % 2];
        }

        public static void Main(string[] args)
        {
            var app = new Lab1_2();
            app.DoWork(args);
        }
    }
}
