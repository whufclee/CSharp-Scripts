using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DescendingOrder
{
    class Program
    {
        static void Main(string[] args)
        {
            var myResult = Kata.OpenOrSenior(new[] { new[] { 45, 12 }, new[] { 55, 21 }, new[] { 19, 2 }, new[] { 104, 20 } });
            foreach (string item in myResult) Console.WriteLine(myResult);
            Console.ReadLine();
        }
    }

    public class Kata
    {
        public static IEnumerable<string> OpenOrSenior(int[][] data)
        {
            // Edit 1
            /* OPTION 1
              var category = new List<string>();
              foreach (int[] member in data)
              {
                if (member[0] >= 55 && member[1] > 7)
                {
                  category.Add("Senior");
                }
                else
                {
                  category.Add("Open");
                }
              }
              return category;
            */
            // Edit 2
            // CLEAN OPTION USING Enumerable tequique
            return data.Select(member => member[0] >= 55 && member[1] > 7 ? "Senior" : "Open").ToList();
        }
    }
}
