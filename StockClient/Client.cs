using System;
using System.IO;
using System.Net.Sockets;

namespace BankClient
{
    class Client : IDisposable
    {
        const int port = 8888;

        private readonly StreamReader reader;
        private readonly StreamWriter writer;
        private TcpClient tcpClient;
        private long traderID;

        public Client()
        {
            // Connecting to the server and creating objects for communications
            tcpClient = new TcpClient("localhost", port);
            NetworkStream stream = tcpClient.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);

            // Parsing the response
            string line = reader.ReadLine();
            if (line.Trim().ToLower() != "success")
                throw new Exception(line);
            traderID = long.Parse(reader.ReadLine());
            String listOfTraders = reader.ReadLine();
            String traderWithStock = reader.ReadLine();

            Console.WriteLine("Logged in successfully.");
            Console.WriteLine("Current traderID: " + traderID);
            Console.WriteLine("Current list of traders: " + listOfTraders);
            Console.WriteLine("Current trader with stock: " + traderWithStock);
            writer.Flush();
        }

        public void getTraders()
        {
            writer.WriteLine("TRADERS");
            writer.Flush();
            String line = reader.ReadLine();
            Console.WriteLine(line);
        }

        public void stockCheck()
        {
            writer.WriteLine("Stock_check");
            writer.Flush();
            String line = reader.ReadLine();
            Console.WriteLine(line);
        }

        public void trade(String toTraderID)
        {
            String line = ($"Trade {toTraderID}");
            writer.WriteLine(line);
            writer.Flush();
            String response = reader.ReadLine();
            Console.WriteLine(response);
        }


        public void Dispose()
        {
            reader.Close();
            writer.Close();
        }
    }
}