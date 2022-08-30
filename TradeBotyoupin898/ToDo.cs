using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeBotyoupin898
{
    public class ToDo
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

