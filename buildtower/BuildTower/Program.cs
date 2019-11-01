using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildTower
{
    class program
    {
        static void Main(string[] args)
        {
            string[] result = Kata.TowerBuilder(10);
            foreach (string n in result) { Console.WriteLine(n); }
            Console.WriteLine(result);
            Console.ReadLine();
        }

        public class Kata
        {
            public static string[] TowerBuilder(int nFloors)
            {
                string[] tower = new string[nFloors];
                int width = (nFloors * 2) - 1;
                int spaces = width / 2;
                for (int x = 0; x < nFloors; x++)
                {
                    string stars;
                    string spacing = new string(' ', spaces - x);
                    stars = new string('*', width - (spacing.Length * 2));
                    tower[x] = spacing+stars+spacing;
                }
                return tower;
            }
        }
    }
}
