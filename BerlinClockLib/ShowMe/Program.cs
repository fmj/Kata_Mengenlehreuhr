using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BerlinClockLib;

namespace ShowMe
{
    class Program
    {
        static void Main(string[] args)
        {
            
            
            while (true)
            {
                var mengelHeur = DateTime.Now.ToString("HH:mm:ss").toMengelHeur();
                foreach (var c in mengelHeur)
                {
                    if (c == 'Y')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(c);
                    }
                    else if (c == 'R')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(c);
                    }
                    else if (c == 'O')
                    {
                        Console.ForegroundColor = ConsoleColor.Black ;
                        Console.Write(c);
                    }
                    else if(c == '\r')
                        Console.WriteLine();
                }
                
                Console.WriteLine();
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
