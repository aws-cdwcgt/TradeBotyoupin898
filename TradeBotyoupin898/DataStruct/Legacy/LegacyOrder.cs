using System;
using Newtonsoft.Json;
using TradeBotyoupin898.Client;

namespace TradeBotyoupin898.DataStruct.Legacy
{
    public class LegacyOrder : IOrder, ICode
    {
        public int Code { get; set; }

        public LegacyOrderData Data { get; set; }
    }

    public class LegacyOrderData : IOrderData, IOrderType
    {
        public ulong? OtherSteamId { get; set; }

        public string SteamOfferId { get; set; }

        public string OrderNo { get; set; }

        public string CommodityName { get; set; }

        public int Seller { get; set; }

        public int LeaseStatus { get; set; }

        public int OfferStatus { get; set; }

        public int BusinessType { get; set; }

        public int GetOrderType() => BusinessType;

        public int GetLeaseStatus() => LeaseStatus;

        public ulong GetBuyer() => (ulong)OtherSteamId;

        public string GetTradeOfferId() => SteamOfferId;
    }
}
