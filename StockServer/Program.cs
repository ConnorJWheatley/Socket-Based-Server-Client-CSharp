using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace StockServer
{
    class Program
    {
        private const int port = 8888;
        private static long traderID = 0L;
        private static readonly StockMarket stockMarket = new StockMarket();
        private static readonly List<ClientHandler> clients = new List<ClientHandler>();
        
        static void Main(string[] args)
        {
            RunServer();
        }

        private static void RunServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, port);
            listener.Start();
            Console.WriteLine("Waiting for incoming connections...");
            while (true)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();
                ClientHandler clientHandler = new ClientHandler(tcpClient, traderID, stockMarket);
                clients.Add(clientHandler);
                traderID++;
                ThreadStart starter = () => ClientHandler.run(tcpClient, traderID, stockMarket);
                Thread thread = new Thread(starter);
                thread.Start();
            }
        }
    }
}