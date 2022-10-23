using System;
using Newtonsoft.Json;
using TradeBotyoupin898.Client;

namespace TradeBotyoupin898.DataStruct
{
    public class OrderJson : IOrder, ICode
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public OrderData Data { get; set; }
    }

    public class OrderData : IOrderData, IOrderType
    {
        public ulong TradeOfferId { get; set; }

        public TradeType TradeType { get; set; }

        public Buyer Buyer { get; set; }

        public int GetOrderType() => TradeType.Type;

        public int GetLeaseStatus() => throw new NotImplementedException("新 API 的租借订单尚未遇见，无法处理");

        public ulong GetBuyer() => (ulong)Buyer.SteamId;

        public string GetTradeOfferId() => TradeOfferId.ToString();
    }

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
}
