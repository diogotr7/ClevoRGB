using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClevoRGB;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Clevo.Initialize())
            {
                Console.WriteLine("Initialized!");
                Clevo.SetColorFull(Color.Blue);
                Clevo.SetKeyColor(Key.ESCAPE, Color.Red);
                Clevo.SetKeyColor(Key.W, Color.Red);
                Clevo.SetKeyColor(Key.A, Color.Red);
                Clevo.SetKeyColor(Key.S, Color.Red);
                Clevo.SetKeyColor(Key.D, Color.Red);
                Clevo.Update();
            }
            else
            {
                Console.WriteLine("NOT Initialized!");
            }


            Console.ReadLine();
        }
    }
}
