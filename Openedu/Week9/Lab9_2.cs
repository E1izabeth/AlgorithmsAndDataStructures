using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Openedu.Week9
{
    class Lab9_2
    {
        public static void Main(string[] args)
        {
            var app = new Lab9_2();
            app.DoWork(args);
        }

        class S
        {
            public int left, right;

            public ulong Muls { get => (ulong)this.left * (ulong)this.right; }

            public S(int left, int right)
            {
                this.left = left;
                this.right = right;
            }
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                foreach (var line in stdin)
                {
                    var text = Regex.Replace(line, @"\s", string.Empty);
                    if (text.Length < 3)
                    {
                        sw.WriteLine("0");
                        continue;
                    }

                    var right = text.GroupBy(c => c).ToDictionary(g => g.Key, g => new S(0, g.Count()));
                    var left = new Dictionary<char, S>();

                    var cc = text[0];
                    var entry = right[cc];
                    left[cc] = entry;
                    entry.left++;
                    entry.right--;

                    var count = text.Length - 1;
                    ulong s = 0;
                    for (int i = 1; i < count; i++)
                    {
                        cc = text[i];
                        entry = right[cc];
                        entry.right--;

                        foreach (var kv in left)
                            s += kv.Value.Muls;

                        entry.left++;
                        left[cc] = entry;
                    }

                    sw.WriteLine(s);
                }
            }
        }
    }
}
