namespace TradeBotyoupin898.APIData.Legacy
{
    public class LegacyOrderData
    {
        public ulong? OtherSteamId { get; set; }

        public string SteamOfferId { get; set; }

        public string OrderNo { get; set; }

        public string CommodityName { get; set; }

        public int Seller { get; set; }

        public int LeaseStatus { get; set; }

        public int OfferStatus { get; set; }

        public int BusinessType { get; set; }
    }
    public class LegacyOrder
    {
        public int Code { get; set; }

        public LegacyOrderData Data { get; set; }
    }
}
