namespace StockServer
{
    class Trader
    {
        private readonly long traderID;
        private int hasStock;
        public Trader(long traderId)
        {
            traderID = traderId;
        }

        public long TraderID
        {
            get { return traderID; }
        }

        public int HasStock
        {
            get { return hasStock; }
            set { hasStock = value; }
        }
    }
}
