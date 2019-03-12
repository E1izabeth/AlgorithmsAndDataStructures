using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week4
{
    class Lab4_4
    {
        public static void Main(string[] args)
        {
            var app = new Lab4_4();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");

                int[] parameters = stdin[0].Split(' ').Select(t => int.Parse(t)).ToArray();
                var queue = new Queue<int>();
                var minimums = new LinkedList<int>();

                for (var i = 0; i < stdin.Length; i++)
                {
                    var temp = stdin[i].Split(' ');
                    switch (temp[0])
                    {
                        case "+":
                            {
                                var r = int.Parse(temp[1]);
                                queue.Enqueue(r);

                                while (minimums.Count > 0 && minimums.First() > r)
                                {
                                    minimums.RemoveFirst();
                                }

                                minimums.AddFirst(r);
                                break;
                            }
                        case "-":
                            {
                                var b = queue.Dequeue();

                                if (minimums.Last() == b)
                                {
                                    minimums.RemoveLast();
                                }

                                break;
                            }
                        case "?":
                            {
                                sw.WriteLine(minimums.Last());
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
        }
    }
}
