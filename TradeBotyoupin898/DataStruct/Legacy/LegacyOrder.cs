using Newtonsoft.Json;
using System;
using TradeBotyoupin898.Client;

namespace TradeBotyoupin898.DataStruct.Legacy
{
    public class LegacyOrder : IOrder
    {
        public int Code { get; set; }

        public LegacyOrderData Data { get; set; }

        public LegacyOrder(Func<string, string, string> httpResponse, string url, string orderNo)
        {
            string responseStr = httpResponse(url, $"{{\"orderNo\": {orderNo}}}");
            LegacyOrder result = JsonConvert.DeserializeObject<LegacyOrder>(responseStr);
            if (result.Code != 0 || result == null) throw new APIErrorException();

            Code = result.Code;
            Data = result.Data;
        }
    }

    public class LegacyOrderData : IOrderData
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
}
