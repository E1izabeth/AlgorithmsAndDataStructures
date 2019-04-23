using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week8
{
    class Lab8_4
    {
        public static void Main(string[] args)
        {
            var app = new Lab8_4();
            app.DoWork(args);
        }

        char[] Next(char[] s)
        {
            char[] t = (char[])s.Clone();
            int i = s.Length - 1;

            while (i >= 0 && s[i] == 'z')
            {
                s[i] = 'a';
                --i;
            }

            if (i >= 0)
                t[i]++;

            return t;
        }

        int Hash(char[] s, int x)
        {
            int h = 0, i, r = 1, l = s.Length;
            for (i = 0; i < l; ++i)
            {
                h = unchecked(h + unchecked((s[i] - 'a') * r));
                r = unchecked(r * x);
            }
            return h;
        }

        private void DoWork(string[] args)
        {
            int x, L, v, i;
            char[] s = new char[2500], t;
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                var N = int.Parse(stdin[0]);

                x = 700;
                L = 2500;
                v = 10;

                for (i = 0; i < L; ++i)
                    s[i] = 'a';

                var ss = new SortedSet<string>();
                t = s;
                for (int j = 0; j < N;)
                {
                    string xx;
                    if (this.Hash(t, x) == v && ss.Add(xx = new string(t)))
                    {
                        sw.WriteLine(xx);
                        j++;
                    }
                    t = this.Next(t);
                }
            }
        }
    }
}