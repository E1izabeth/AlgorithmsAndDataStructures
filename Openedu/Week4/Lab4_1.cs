using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week4
{
    class Lab4_1
    {
        public static void Main(string[] args)
        {
            var app = new Lab4_1();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");

                int[] parameters = stdin[0].Split(' ').Select(t => int.Parse(t)).ToArray();

                Stack<long> queue = new Stack<long>();
                for (int i = 1; i < stdin.Length; i++)
                {
                    string[] temp = stdin[i].Split(' ');
                    if (temp[0] == "-")
                        sw.WriteLine(queue.Pop());
                    else
                        queue.Push(long.Parse(temp[1]));
                }
            }
        }
    }
}
