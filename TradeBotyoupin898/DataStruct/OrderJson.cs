using System;
using Newtonsoft.Json;
using TradeBotyoupin898.Client;

namespace TradeBotyoupin898.DataStruct
{
    public class OrderJson : IOrder
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public OrderData Data { get; set; }

        public OrderJson(Func<string, string, string> httpResponse, string url, string orderNo)
        {
            string responseStr = httpResponse(url, $"{{\"orderNo\": {orderNo}}}");
            OrderJson result = JsonConvert.DeserializeObject<OrderJson>(responseStr);
            if (result.Code != 0 || result == null) throw new APIErrorException();

            Code = result.Code;
            Msg = result.Msg;
            Data = result.Data;
        }
    }

    public class OrderData : IOrderData
    {
        public ulong TradeOfferId { get; set; }

        public TradeType TradeType { get; set; }

        public Buyer Buyer { get; set; }
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
