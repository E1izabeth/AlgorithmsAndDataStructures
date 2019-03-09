using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Openedu
{
    class Program
    {
        static T CreateDelegate<T>(MethodInfo methodInfo)
        {
            return (T)(object)Delegate.CreateDelegate(typeof(T), methodInfo);
        }

        static void Main(string[] args)
        {
            var arr = typeof(Program).Assembly.GetTypes();
            Array.Sort(arr, (a, b) => b.FullName.CompareTo(a.FullName));

            MethodInfo doWorkMethod = null;

            if (args.Any())
                arr = arr.Where(t => t.Name == args[0]).ToArray();

            for (int i = 0; i < arr.Length && doWorkMethod == null; i++)
                if (arr[i] != typeof(Program))
                    doWorkMethod = arr[i].GetMethod("Main", BindingFlags.Public | BindingFlags.Static);

            //doWorkMethod = arr[3].GetMethod("Main", BindingFlags.Public | BindingFlags.Static);

            if (doWorkMethod != null)
            {
                var doWorkToCall = CreateDelegate<Action<string[]>>(doWorkMethod);
                doWorkToCall(args);
            }
            else
            {
                throw new ApplicationException("Nothing to do");
            }
        }
    }
}
