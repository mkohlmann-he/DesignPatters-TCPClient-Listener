using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;
using System.Net.Sockets;



namespace DesignPatterns_TCP_ClientSpooler
{
    class MyClient
    {
        TcpClient Client = new TcpClient();
        string ClientID;
        string ServerID = "";
        string StackID = "";
        
        // Server Details
        String RemoteServer = "127.0.0.1";
        Int32 port = 13000;

        // Stream Details
        NetworkStream Stream;
        


        public MyClient(string id)
        {
            this.ClientID = id;

        }

        public void Run()
        {
            ProcessCommandLine();

            Console.WriteLine("\n Press Enter to Leave Client {0} Loop...", ClientID);
            Console.Read();
        }







        public void ProcessCommandLine()
        {
            while (true)
            {
                // Process Client Side Commands
                string userInput = this.GetCommandLineInput();
                ProcessInput(userInput);
            }
        }

        public string GetCommandLineInput()
        {
            Console.Write("{0}>{1}>{2}> ", this.ClientID, this.ServerID, this.StackID);
            return Console.ReadLine();
        }

        public void ProcessInput(string inputString)
        {
            string[] inputParse = inputString.Split();
            switch (inputParse[0])
            {
                case "connect":
                    ConnectToServer();
                    break;

                case "disconnect":
                    DisconnectFromServer();
                    break;

                default:
                    SendDataPacket(inputString);
                    break;
            }
        }


        public void ConnectToServer()
        {
            this.Client = new TcpClient(this.RemoteServer, port);
            this.Stream = this.Client.GetStream();
            Task.Run(() => ListenForServerReturns());
        }


        public void DisconnectFromServer()
        {
            this.ServerID = "";
            this.StackID = "";
            if (Client.Connected)
            {
                this.Stream.Close();
                this.Client.Close();
            }

        }


        async public Task ListenForServerReturns()
        {
            while (Client.Connected)
            {
                while (Stream.DataAvailable)
                {
                    Console.WriteLine("\nListening");
                    String responseData = String.Empty;
                    Byte[] data = new Byte[256];
                    Int32 bytes = this.Stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("{0}<{1}<{2}< {3}\n{0}>{1}>{2}> ", this.ClientID, this.ServerID, this.StackID, responseData);
                }
                System.Threading.Thread.Sleep(500); 
            }
                           
        }


        public void SendDataPacket(string message)
        {
            if (this.Client.Connected)
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(this.ClientID + " " + message);

                // Send Message
                this.Stream.Write(data, 0, data.Length);
            }
        }


        public void Connect(String RemoteServer, String message)
        {
            try
            {
                // Create a TcpClient. 
                // Note, for this client to work you need to have a TcpServer  
                // connected to the same address as specified by the server, port 
                // combination.
                Int32 port = 13000;
                TcpClient client = new TcpClient(RemoteServer, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(this.ClientID + ":" + message);

                // Get a client stream for reading and writing. 
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("\n\n------\nCLIENT {0} Sent: {1}", this.ClientID, (message));

                // Receive the TcpServer.response. 

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("\n\n------\nCLIENT {0} Received: {1}", this.ClientID, responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to Leave Client {0} Loop...", ClientID);
            Console.Read();
        }




    }
}
