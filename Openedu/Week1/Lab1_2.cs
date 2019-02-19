using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week1
{
    public class Lab1_2
    {
        public static void Main(string[] args)
        {
            long a = 0, b = 0, c = 0;

            using (var file = new System.IO.StreamReader("input.txt"))
            {
                var input = file.ReadLine().Split(' ');
                a = int.Parse(input[0]);
                b = int.Parse(input[1]);
            }

            using (var file = new System.IO.StreamWriter("output.txt"))
            {
                c = a + b * b;
                file.WriteLine(c);
            }
        }
    }
}
