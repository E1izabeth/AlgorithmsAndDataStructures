using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week4
{
    class Lab4_2
    {
        public static void Main(string[] args)
        {
            var app = new Lab4_2();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                
                int[] parameters = stdin[0].Split(' ').Select(t => int.Parse(t)).ToArray();

                var queue = new Queue<long>();
                for (int i = 1; i < stdin.Length; i++)
                {
                    string[] temp = stdin[i].Split(' ');
                    if (temp[0] == "-")
                        sw.WriteLine(queue.Dequeue());
                    else
                        queue.Enqueue(long.Parse(temp[1]));
                }
            }
        }
    }
}
