using System;
using System.Collections.Generic;

namespace StockServer
{
    class StockMarket
    {
        private readonly Dictionary<long, Trader> traders;
        private long traderWithStock;

        public StockMarket()
        {
            traders = new Dictionary<long, Trader>();
        }

        public void addTrader(long traderID, int hasStock)
        {
            Trader trader = new Trader(traderID);
            if(hasStock == 1)
            {
                trader.HasStock = 1;
            }
            else
            {
                trader.HasStock = 0;
            }
            traders.Add(traderID, trader);
        }

        public void removeTrader(long traderID)
        {
            traders.Remove(traderID);
        }

        public long[] Traders
        {
            get 
            {
                long[] traderIDs = new long[traders.Count];
                traders.Keys.CopyTo(traderIDs, 0);
                return traderIDs;
            } 
        }

        public Trader getTrader(long traderID)
        {
            return traders[traderID];
        }

        public long traderWithTheStock()
        {  
            foreach(KeyValuePair<long, Trader> entry in traders)
            {
                if(entry.Value.HasStock == 1)
                {
                    traderWithStock = entry.Key;
                }
            }
            return traderWithStock;
        }

        public bool moveStock(long fromTraderID, long toTraderID)
        {
            lock (this)
            {
                Trader fromTrader = getTrader(fromTraderID);
                if(fromTrader.HasStock == 0)
                {
                    return false;
                }
                else 
                {
                    Trader toTrader = getTrader(toTraderID);
                    fromTrader.HasStock = 0;
                    toTrader.HasStock = 1;
                    return true;
                }
            }
        }
    }
}
