using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week8
{
    class Lab8_2
    {
        public static void Main(string[] args)
        {
            var app = new Lab8_2();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                var list = new LinkedList<string>();
                var kv = new Dictionary<string, LinkedListNode<string>>();

                for (int i = 1; i < stdin.Length; i++)
                {
                    var arr = stdin[i].Split(' ');
                    var key = arr[1];
                    switch (arr[0])
                    {
                        case "put":
                            {
                                var value = arr[2];
                                if (kv.TryGetValue(key, out var node))
                                {
                                    node.Value = value;
                                }
                                else
                                {
                                    kv.Add(key, list.AddLast(value));
                                }
                            }
                            break;
                        case "get":
                            {
                                if (kv.TryGetValue(key, out var node))
                                    sw.WriteLine(node.Value);
                                else 
                                    sw.WriteLine("<none>");
                            } break;
                        case "prev":
                            {
                                if (kv.TryGetValue(key, out var node) && node.Previous != null)
                                    sw.WriteLine(node.Previous.Value);
                                else
                                    sw.WriteLine("<none>");
                            }
                            break;
                        case "next":
                            {
                                if (kv.TryGetValue(key, out var node) && node.Next != null)
                                    sw.WriteLine(node.Next.Value);
                                else
                                    sw.WriteLine("<none>");
                            }
                            break;
                        case "delete":
                            {
                                if (kv.TryGetValue(key, out var node))
                                {
                                    kv.Remove(key);
                                    list.Remove(node);
                                }
                            } break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }
    }
}