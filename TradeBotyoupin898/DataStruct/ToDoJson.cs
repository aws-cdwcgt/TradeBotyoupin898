using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TradeBotyoupin898.Client;

namespace TradeBotyoupin898.DataStruct
{
    public class ToDoJson : IToDo, ICode
    {
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        // FIXME: (Error Prone)需要指明为更具体的 ToDoDataItem 而不是 IToDoDataItem.
        public List<ToDoDataItem> Data { get; set; }

        public List<IToDoDataItem> GetData() => Data.Select(i => (IToDoDataItem)i).ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    public class ToDoDataItem : IToDoDataItem
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

