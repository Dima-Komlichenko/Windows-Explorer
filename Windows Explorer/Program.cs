using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows_Explorer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(100, 30);
            Console.OutputEncoding = Encoding.Unicode;


            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Windows_Explorer we = new Windows_Explorer();
            we.App();
            Console.ReadKey();
        }
    }
}
