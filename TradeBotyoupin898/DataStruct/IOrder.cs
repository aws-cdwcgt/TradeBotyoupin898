namespace TradeBotyoupin898.DataStruct
{
    public interface IOrder
    {
    }

    public interface IOrderData
    {
        /// <returns>买家的 64 位 steamID</returns>
        public ulong GetBuyer();

        public string GetTradeOfferId();
    }

    public interface IOrderType
    {
        public int GetOrderType();

        public int GetLeaseStatus();
    }
}
