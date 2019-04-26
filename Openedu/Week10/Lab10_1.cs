using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Openedu.Week10
{
    class Lab10_1
    {
        public static void Main(string[] args)
        {
            var app = new Lab10_1();
            app.DoWork(args);
        }
        
        public static int[] PrefFunc(string s)
        {
            int i, j, n = s.Length;
            int[] pi = new int[n];
            for (i = 1; i < n; ++i)
            {
                j = pi[i - 1];
                while (j > 0 && s[i] != s[j]) j = pi[j - 1];
                if (s[i] == s[j]) ++j;
                pi[i] = j;
            }
            return pi;
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                var numbers = PrefFunc(stdin[0]);
                for (int i = 0; i < numbers.Length; i++)
                {
                    sw.Write(numbers[i] + " ");
                }
            }
        }
    }
}
