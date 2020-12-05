using System;
using System.IO;
using System.Net.Sockets;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
/*
stock trading not working
need to update finally statement
*/
// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


namespace StockServer
{
    class ClientHandler
    {
        private readonly TcpClient tcpClient;
        private long traderID;
        private StockMarket stockMarket;
        public ClientHandler(TcpClient client, long traderId, StockMarket stockMarketInst)
        {
            tcpClient = client;
            traderID = traderId;
            stockMarket = stockMarketInst;
        }

        public static void run(TcpClient client, long traderID, StockMarket stockMarket)
        {
            using (Stream stream = client.GetStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                StreamReader reader = new StreamReader(stream);
                try
                {
                    Console.WriteLine($"New connection; trader ID {traderID}");
                    writer.WriteLine("SUCCESS");
                    if(stockMarket.Traders.Length == 0)
                    {
                        stockMarket.addTrader(traderID, 1);
                    }
                    else
                    {
                        stockMarket.addTrader(traderID, 0);
                    }
                    writer.WriteLine(traderID);
                    writer.WriteLine(string.Join(",", stockMarket.Traders));
                    writer.WriteLine(stockMarket.traderWithTheStock());
                    writer.Flush();

                    while (true)
                    {
                        string line = reader.ReadLine();
                        string[] substrings = line.Split(' ');
                        switch (substrings[0].ToLower())
                        {
                            case "hello":
                                writer.WriteLine("hello from the c# server " + traderID + "!");
                                writer.Flush();
                                break;

                            case "stock_check":
                                Trader trader = stockMarket.getTrader(traderID);
                                writer.WriteLine(trader.HasStock);
                                writer.Flush();
                                break;

                            case "trade":
                                long fromTrader = traderID;
                                long toTrader = long.Parse(substrings[1]);
                                if(stockMarket.getTrader(toTrader) == null)
                                {
                                    writer.WriteLine("That trader is not in the market.");
                                    break;
                                }
                                if(stockMarket.moveStock(fromTrader, toTrader) == true)
                                {
                                    writer.WriteLine("TRADE SUCCESSFUL");
                                    Console.WriteLine("Trader " + traderID + " gave the stock to " + toTrader);
                                }

                                writer.Flush();
                                break;
                            default:
                                throw new Exception($"Unknown command: {substrings[0]}.");
                        }
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        writer.WriteLine("ERROR " + e.Message);
                        writer.Flush();
                        client.Close();
                    }
                    catch
                    {
                        Console.WriteLine("Failed to send error message.");
                    }
                }
                finally
                {   
                    Trader trader = stockMarket.getTrader(traderID);
                    if(trader.HasStock == 1 && stockMarket.Traders.Length > 1)
                    {
                        stockMarket.removeTrader(traderID);
                        long[] traders = stockMarket.Traders;
                        int numOfTraders = stockMarket.Traders.Length;
                        Random r = new Random();
                        int rand = r.Next(1, numOfTraders);
                        Console.WriteLine();
                        long randomTraderID = (long)traders.GetValue(rand);
                        Trader traderToGetStock = stockMarket.getTrader(randomTraderID);
                        trader.HasStock = 1;
                        Console.WriteLine("Trader " + traderID + " disconnected and left the market with the stock. It has been given to trader " + randomTraderID);
                    }
                    else
                    {
                        Console.WriteLine("Trader " + traderID + " disconnected.");
                        stockMarket.removeTrader(traderID);
                    }
                    Console.WriteLine("Current list of traders: " + stockMarket.Traders);
                }
            }
        }
    }
}