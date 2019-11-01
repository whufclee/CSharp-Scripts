using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDuplicatesInString
{
    class Program
    {
        static void Main(string[] args)
        {
            bool myReturn = Kata.IsIsogram("Leigh");
            Console.WriteLine($"Result: {myReturn}");
            Console.ReadLine();
        }

        public class Kata
        {
            public static bool IsIsogram(string str)
            {
                return str.Length == str.ToUpper().Distinct().Count();
            }
        }
    }
}