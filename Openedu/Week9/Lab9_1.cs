using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Openedu.Week9
{
    class Lab9_1
    {
        public static void Main(string[] args)
        {
            var app = new Lab9_1();
            app.DoWork(args);
        }

        //Префикс-функция для КМП
        public static int[] PrefFunc(string x)
        {
            //Инициализация массива-результата длиной X
            int[] res = new int[x.Length];
            int i = 0;
            int j = -1;
            res[0] = -1;
            //Вычисление префикс-функции
            while (i < x.Length - 1)
            {
                while ((j >= 0) && (x[j] != x[i]))
                    j = res[j];
                i++;
                j++;
                if (x[i] == x[j])
                    res[i] = res[j];
                else
                    res[i] = j;
            }
            return res;//Возвращение префикс-функции
        }

        //Функция поиска алгоритмом КМП
        public static List<int> KMP(string x, string s)
        {
            List<int> numbers = new List<int>(); //Объявление строки с номерами позиций
            if (x.Length > s.Length) return numbers; //Возвращает 0 поиск если образец больше исходной строки
                                                 //Вызов префикс-функции
            int[] d = PrefFunc(x);
            int i = 0, j;
            var prev = -1;
            while (i < s.Length)
            {
                var t = i;

                for (j = 0; (i < s.Length) && (j < x.Length); i++, j++)
                    while ((j >= 0) && (x[j] != s[i]))
                        j = d[j];

                if (j == x.Length)
                {
                    var pos = i - j;
                    if (pos != prev)
                    {
                        prev = pos;
                        numbers.Add(pos);
                    }
                }

                i = t;
                i++;
            }

            return numbers; //Возвращение результата поиска
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                var numbers = KMP(stdin[0], stdin[1]);
                var count = numbers.Count;
                sw.WriteLine(count);
                for (int i = 0; i < count; i++)
                {
                    sw.Write(numbers[i] + 1 + " ");
                }
            }
        }
    }
}
