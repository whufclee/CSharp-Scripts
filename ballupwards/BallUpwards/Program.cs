using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BallUpwards
{
    class Program
    {
        static void Main(string[] args)
        {
            Ball.MaxBall(15);
            Console.ReadLine();
        }
    }
    public class Ball
    {

        public static int MaxBall(int v0)
        {
            double kmpm = (v0 * 1000) / 60;
            double mps = kmpm / 60;
            Console.WriteLine($"MPS: {mps}");
            double orig_t = 0.166666666666667;
            double g = Math.Pow(9.81, 2)*orig_t;
            double t = 0.0;
            for (int i = 0; i < v0; i++)
            {
                t += orig_t;
                double h = (mps*t) - (0.5*g*t*t);
                Console.WriteLine($"{i} : {h}");
            }
            return 8;
        }
    }
}
