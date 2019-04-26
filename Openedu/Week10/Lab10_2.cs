using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Openedu.Week10
{
    class Lab10_2
    {
        public static void Main(string[] args)
        {
            var app = new Lab10_2();
            app.DoWork(args);
        }

        
        public static int[] ZFunc(string s)
        {
            int n = s.Length;
            int[] z = new int[n];
            for (int i = 1, l = 0, r = 0; i < n; ++i)
            {
                if (i <= r)
                    z[i] = Math.Min(r - i + 1, z[i - l]);
                while (i + z[i] < n && s[z[i]] == s[i + z[i]])
                    ++z[i];
                if (i + z[i] - 1 > r)
                {
                    l = i;
                    r = i + z[i] - 1;
                }
            }
            return z;
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                var numbers = ZFunc(stdin[0]);
                for (int i = 1; i < numbers.Length; i++)
                {
                    sw.Write(numbers[i] + " ");
                }
            }
        }
    }
}
