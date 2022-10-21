using System.Collections.Generic;

namespace TradeBotyoupin898.APIData
{
    public class ToDoJson
    {
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TodoDataItem> Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public class TodoDataItem
        {
            public int Type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string OrderNo { get; set; }
            /// <summary>
            /// 有承租方下单，等待确认报价
            /// </summary>
            public string Message { get; set; }

            public string CommodityName { get; set; }
        }


    }
}

