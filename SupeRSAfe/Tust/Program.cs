using System;

namespace Tust
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = "Hello World!";

            byte aa = 4;

            var b = System.Text.Encoding.UTF8.GetBytes(a);

            var c = System.Text.Encoding.UTF8.GetString(b);

            foreach(var d in b)
            {
                Console.Write($"{d} ");
            }

            Console.WriteLine();
            Console.WriteLine(c);

            Console.WriteLine(aa.ToString("D3"));
        }
    }
}
