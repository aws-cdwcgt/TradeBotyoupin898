namespace TradeBotyoupin898.APIData
{
    public class Buyer
    {
        public int Id { get; set; }

        public ulong? SteamId { get; set; }
    }

    public class TradeType
    {
        public int Type { get; set; }

        public string Name { get; set; }
    }

    public class OrderData
    {
        public ulong TradeOfferId { get; set; }

        public TradeType TradeType { get; set; }

        public Buyer Buyer { get; set; }
    }

    public class OrderJson
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public OrderData Data { get; set; }
    }
}
