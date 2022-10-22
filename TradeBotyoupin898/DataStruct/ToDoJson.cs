using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TradeBotyoupin898.Client;

namespace TradeBotyoupin898.DataStruct
{
    public class ToDoJson : IToDo
    {
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        // FIXME: (Error Prone)需要指明为更具体的 ToDoDataItem 而不是 IToDoDataItem.
        public List<IToDoDataItem> Data { get; set; }

        /// <summary>
        /// 向 httpResponse 发送 url.
        /// </summary>
        /// <param name="httpResponse"></param>
        /// <param name="endpoint_url"></param>
        public ToDoJson(Func<string, string, string> httpResponse, string url, string content)
        {
            string responseStr = httpResponse(url, content);
            var result = JsonConvert.DeserializeObject<ToDoJson>(responseStr);
            if (result.Code != 0 || result == null) throw new APIErrorException();

            Code = result.Code;
            Data = result.Data;
        }
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

