using System;
using Newtonsoft.Json;
using TradeBotyoupin898.Client;

namespace TradeBotyoupin898.DataStruct.Legacy
{
    public class LeaseOrder: ICode
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public LeaseOrderData Data { get; set; }
    }

    public class LeaseOrderData : IOrderData
    {
        public string ReturnOrderNo { get; set; }

        public ulong? OtherSteamId { get; }

        public ulong GetBuyer() => (ulong)OtherSteamId;

        public string GetTradeOfferId() => ReturnOrderNo;
    }
}
