using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Openedu.Week10
{
    public class Lab10_3
    {
        private static string[] _input;
        private static int _currentLineIndex;

        private static string ReadLine()
        {
            return _input[_currentLineIndex++];
        }

        public static void Main(string[] args)
        {
            var app = new Lab10_3();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            _input = File.ReadAllLines("input.txt");

            string str = ReadLine();
            int[] dim = new int[str.Length + 1];
            int[] substrStart = new int[str.Length + 1];
            int[] length = new int[str.Length + 1];
            for (int i = 0; i < str.Length + 1; i++)
            {
                dim[i] = int.MaxValue;
            }
            dim[0] = 0;

            for (int i = 0; i < str.Length; i++)
            {
                int[] p = new int[str.Length + 1];
                p[1] = 0;

                if (dim[i + 1] > dim[i] + 1)
                {
                    dim[i + 1] = dim[i] + 1;
                    substrStart[i + 1] = i;
                    length[i + 1] = 1;
                }

                int k = 0;
                for (int j = 2; i + j - 1 < str.Length; j++)
                {
                    while (str[i + j - 1] != str[i + k] && k > 0)
                    {
                        k = p[k];
                    }

                    if (str[i + j - 1] == str[i + k])
                    {
                        k++;
                    }

                    p[j] = k;

                    if (j % (j - p[j]) == 0)
                    {
                        if (dim[i + j] > dim[i] + (j - p[j]))
                        {
                            dim[i + j] = dim[i] + (j - p[j]);
                            substrStart[i + j] = i;
                            length[i + j] = j - p[j];
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            string[] substring = new string[str.Length];
            int[] substringsCount = new int[str.Length];
            int align = 0;

            for (int i = str.Length; i > 0;)
            {
                substring[align] = str.Substring(substrStart[i], length[i]);
                substringsCount[align] = (i - substrStart[i]) / length[i];
                align++;
                i = substrStart[i];
            }

            int maxAlign = align - 1;
            bool isPreviousAppendable = true;


            for (align--; align >= 0; align--)
            {
                bool optimized = (substring[align].Length > 2 || substringsCount[align] != 2) && (substring[align].Length != 1 || substringsCount[align] != 3);
                if (optimized)
                {
                    bool isOldAppendable = isPreviousAppendable;
                    isPreviousAppendable = substringsCount[align] <= 1;
                    if ((align != maxAlign) && (!isOldAppendable || !isPreviousAppendable))
                    {
                        sb.Append("+");
                    }
                    sb.Append(substring[align]);
                    if (!isPreviousAppendable)
                    {
                        sb.Append("*");
                        sb.Append(substringsCount[align]);
                    }
                }
                else
                {
                    string o = substring[align] + substring[align];
                    if (substringsCount[align] == 3)
                    {
                        o += substring[align];
                    }

                    if (!isPreviousAppendable)
                    {
                        sb.Append("+");
                    }
                    sb.Append(o);

                    isPreviousAppendable = true;
                }
            }

            File.WriteAllText("output.txt", sb.ToString());
        }
    }
}
