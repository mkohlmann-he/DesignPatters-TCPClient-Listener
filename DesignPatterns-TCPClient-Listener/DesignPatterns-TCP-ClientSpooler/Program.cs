using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignPatterns_TCP_ClientSpooler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write(">>> Enter your ID: ");
            string clientID = Console.ReadLine();
            MyClient ClientProxy = new MyClient(clientID);
            ClientProxy.Run();
           
            Console.WriteLine("\n Press Enter to Leave Client Spooler Main Loop...");
            Console.Read();
        }
    }
}
