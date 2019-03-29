using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week6
{

    class Lab6_1
    {
        static Dictionary<int, string> _results = new Dictionary<int, string>();

        public static void Main(string[] args)
        {
            var app = new Lab6_1();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (var sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");

                int[] array = stdin[1].Split(' ').Select(x => int.Parse(x)).ToArray();
                int[] requests = stdin[3].Split(' ').Select(x => int.Parse(x)).ToArray();

                for (int i = 0; i < requests.Length; i++)
                    if (_results.ContainsKey(requests[i]))
                        sw.WriteLine(_results[requests[i]]);
                    else
                    {
                        this.BinarySearch(array, requests[i]);
                        sw.WriteLine(_results[requests[i]]);
                    }
            }
        }

        void BinarySearch(int[] array, int value)
        {
            int l = -1, r = array.Length;
            while (r > l + 1 || (r >= array.Length && l < 0 && array[l] != value && array[r] != value))
            {
                int m = (l + r) / 2;
                if (array[m] < value)
                    l = m;
                else
                    r = m;
            }

            if (r < array.Length && array[r] == value)
            {
                while (r < array.Length && array[r] == value) r++;
                while (l >= 0 && array[l] == value) l--;
                l += 2;
                _results.Add(value, string.Format("{0} {1}", l, r));
            }
            else
            {
                _results.Add(value, string.Format("-1 -1"));
            }
        }
    }
}
