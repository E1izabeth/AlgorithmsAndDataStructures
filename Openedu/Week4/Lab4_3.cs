using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week4
{
    class Lab4_3
    {
        public static void Main(string[] args)
        {
            var app = new Lab4_3();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");

                for (int i = 1; i < stdin.Length; i++)
                {
                    var isCorrect = true;
                    Stack<char> brackets = new Stack<char>();
                    for (int j = 0; j < stdin[i].Length; j++)
                        switch (stdin[i][j])
                        {
                            case '(':
                                brackets.Push(stdin[i][j]);
                                break;
                            case '[':
                                brackets.Push(stdin[i][j]);
                                break;
                            case ')':
                                if (brackets.Count == 0 || brackets.Peek() != '(')
                                    isCorrect = false;
                                if (brackets.Count != 0)
                                    brackets.Pop();
                                break;
                            case ']':
                                if (brackets.Count == 0 || brackets.Peek() != '[')
                                    isCorrect = false;
                                if (brackets.Count != 0)
                                    brackets.Pop();
                                break;
                        }
                    if (isCorrect && brackets.Count == 0)
                        sw.WriteLine("YES");
                    else
                        sw.WriteLine("NO");
                }

                sw.Dispose();
            }
        }
    }
}
