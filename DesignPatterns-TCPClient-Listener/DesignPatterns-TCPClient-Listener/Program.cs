using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesignPatterns_TCPClient_Listener.ServerSide;

namespace DesignPatterns_TCPClient_Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            MyServerProxy ServerProxy = new MyServerProxy();
            ServerProxy.run();



            Console.WriteLine("\n Press Enter to Leave Server Main Loop...");
            Console.Read();
        }
    }
}
