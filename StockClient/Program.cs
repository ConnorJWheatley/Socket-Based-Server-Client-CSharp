using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace BankClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                using (Client client = new Client())
                {
                    while (true)
                    {
                        string line = Console.ReadLine();
                        string[] substrings = line.Split(' ');
                        switch (substrings[0].ToLower())
                        {
                            case "traders":
                                client.getTraders();
                                break;
                            case "stock_check":
                                client.stockCheck();
                                break;
                            case "trade":
                                client.trade(substrings[1]);
                                break;
                            default:
                                throw new Exception("Unknown command: " + substrings[0]);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
