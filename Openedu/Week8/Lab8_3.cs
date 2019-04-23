using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Openedu.Week8
{
    public class HashTable
    {
        private long _tableSize;
        private long[] _table;

        public HashTable(long size)
        {
            _tableSize = size;
            _table = new long[size];
            for (int i = 0; i < size; i++)
                _table[i] = -1;
        }

        public bool TryInsert(long key)
        {
            long hash = this.GetHash(key);
            long hash2 = this.GetHash2(key);
            while (_table[hash] != -1 && _table[hash] != key)
            {
                hash = (hash + hash2) % _tableSize;
                hash2++;
            }
            if (_table[hash] == key)
                return false;
            _table[hash] = key;
            return true;
        }

        private long GetHash(long key)
        {
            return Math.Abs(unchecked((int)((long)(key * 47))) ^ (int)((key * 31) >> 32)) % _tableSize;
        }
        private long GetHash2(long key)
        {
            return Math.Abs(unchecked((int)((long)(key * 113))) ^ (int)((key * 97) >> 32)) % (_tableSize - 1) + 1;
        }
    }

    class Lab8_3
    {
        public static void Main(string[] args)
        {
            var app = new Lab8_3();
            app.DoWork(args);
        }

        private void DoWork(string[] args)
        {
            using (StreamWriter sw = new StreamWriter("output.txt"))
            {
                string[] stdin = File.ReadAllLines("input.txt");
                var arr1 = stdin[0].Split(' ').Select(s => long.Parse(s)).ToArray();
                var arr2 = stdin[1].Split(' ').Select(s => long.Parse(s)).ToArray();

                var n = arr1[0];
                var x = arr1[1];
                var a = arr1[2];
                var b = arr1[3];

                var Ac = arr2[0];
                var Bc = arr2[1];
                var Ad = arr2[2];
                var Bd = arr2[3];

                HashTable hashTable = new HashTable(n * 2);
                for (int i = 0; i < n; i++)
                {
                    if (hashTable.TryInsert(x))
                    {
                        a = (a + Ad) % 1000;
                        b = (b + Bd) % 1000000000000000;
                    }
                    else
                    {
                        a = (a + Ac) % 1000;
                        b = (b + Bc) % 1000000000000000;
                    }
                    x = (x * a + b) % 1000000000000000;
                }

                sw.WriteLine($"{x} {a} {b}");
            }
        }
    }
}