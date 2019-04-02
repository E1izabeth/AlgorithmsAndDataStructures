using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week6
{
    class Lab6_2
    {
        public static void Main(string[] args)
        {
            var app = new Lab6_2();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (var sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllText("input.txt").Split(' ');

                int n = int.Parse(stdin[0]);
                double a = double.Parse(stdin[1]);
                sw.WriteLine(findMinLastHeight(n, 0, a));
            }

            double findMinLastHeight(double n, double left, double right)
            {
                double last = -1; 
                double leftHeight = right;

                
                while ((right - left) > 0.00000000001)
                {
                    double mid = (left + right) / 2;
                    double prev = leftHeight;
                    double cur = mid;
                    bool aboveGround = cur > 0;

                    for (int i = 3; i <= n; i++)
                    {
                        double next = 2 * cur - prev + 2;
                        aboveGround &= next > 0;
                        prev = cur;
                        cur = next;
                    }

                    if (aboveGround)
                    {
                        right = mid;
                        last = cur;
                    }
                    else
                        left = mid;

                }

                return last;
            }
        }
    }
}
