using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeBotyoupin898
{
    public class OrderData
    {
        public ulong OtherSteamId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string SteamOfferId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// MAG-7 | 正义 (略有磨损)
        /// </summary>
        public string CommodityName { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public int Seller { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int OfferStatus { get; set; }
    }

    public class Order
    {
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 请求成功
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int TipType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public OrderData Data { get; set; }
    }
}
