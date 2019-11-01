using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchingCarMileageNumbers
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Kata.IsInteresting(3, new List<int>() { 1337, 256 });
            Console.WriteLine(result);
            Console.ReadLine();
        }
        public static class Kata
        {
            // Check if all numbers are the same
            private static bool SameNumbers(int number)
            {
                int numCount = number.ToString().Where(c => c == number.ToString()[0]).Count();
                return numCount == number.ToString().Length;
            }

            // check if all numbers are sequentially incremental
            private static bool SequentialInc(int number)
            {
                int[] numArray = Array.ConvertAll(number.ToString().ToArray(), x => (int)x - 48);
                for (int i = 1; i < numArray.Length; i++)
                {
                    int current = numArray[i];
                    int prevNum = numArray[i-1];
                    int expected = prevNum < 9 ? prevNum + 1 : 0;
                    if (current != expected) return false;
                }
                return true;
            }

            // check if all numbers are sequentially decremental
            private static bool SequentialDec(int number)
            {
                int[] numArray = Array.ConvertAll(number.ToString().ToArray(), x => (int)x - 48);
                for (int i = 1; i < numArray.Length; i++) if (numArray[i] != numArray[i-1]-1)
                        return false;
                return true;
            }

            // check if numbers are in palindrome e.g. 1221 or 73837
            private static bool Palindrome(int number)
            {
                string numString = number.ToString();
                int splitPoint = numString.Length / 2;
                int oddNum = numString.Length % 2;
                string part1 = numString.Remove(splitPoint);
                char[] part2Array = numString.Remove(0,splitPoint+oddNum).ToCharArray();
                Array.Reverse(part2Array);
                string part2 = new string (part2Array);
                return part1 == part2;
            }

            public static int IsInteresting(int number, List<int> awesomePhrases)
            {
                // check for all interesting numbers
                if (number >= 98) {
                    int removed1stNum = Convert.ToInt32(number.ToString().Remove(0, 1));
                    if (removed1stNum == 0 && number > 99) return 2; //trailing zeros
                    if (SameNumbers(number) && number > 99) return 2; //all same numbers
                    if (SequentialInc(number) && number > 99) return 2; //all numbers are sequential increments
                    if (SequentialDec(number) && number > 99) return 2; //all numbers are sequential decrements
                    if (Palindrome(number) && number > 99) return 2; //number is palindrome
                    if (awesomePhrases.Contains(number) && number > 99) return 2; //number in awesomePhrases

                    // check if interesting occurs within next 2 miles
                    for (int i = 0; i < 2; i++)
                    {
                        number++;
                        removed1stNum = Convert.ToInt32(number.ToString().Remove(0, 1));
                        if (removed1stNum == 0 && number > 99) return 1; //trailing zeros
                        if (SameNumbers(number) && number > 99) return 1; //all same numbers
                        if (SequentialInc(number) && number > 99) return 1; //all numbers are sequential increments
                        if (SequentialDec(number) && number > 99) return 1; //all numbers are sequential decrements
                        if (Palindrome(number) && number > 99) return 1; //number is palindrome
                        if (awesomePhrases.Contains(number) && number > 99) return 1; //number in awesomePhrases
                    }
                }
                return 0;
            }
        }

        /* BEST PRACTICE
          public static int IsInteresting(int number, List<int> awesomePhrases)
          {
            return Enumerable.Range(number,3)
              .Where(x => Interesting(x, awesomePhrases))
              .Select(x => (number - x + 4)/2)
              .FirstOrDefault();
          }
    
          private static bool Interesting(int num, List<int> awesome)
          {
            if (num < 100) return false;
            var s = num.ToString();
            return awesome.Contains(num)
              || s.Skip(1).All(c => c == '0')
              || s.Skip(1).All(c => c == s[0])
              || "1234567890".Contains(s)
              || "9876543210".Contains(s)
              || s.SequenceEqual(s.Reverse());
          }
         */

    }
}
