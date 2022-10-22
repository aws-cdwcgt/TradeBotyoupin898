using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TradeBotyoupin898.Client;

namespace TradeBotyoupin898.DataStruct.Legacy
{
    public class LegacyToDo : IToDo
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        // FIXME: (Error Prone)需要指明为更具体的 LegacyTodoDataItem 而不是 IToDoDataItem.
        public List<IToDoDataItem> Data { get; set; }

        public LegacyToDo(Func<string, string> httpResponse, string url)
        {
            string responseStr = httpResponse(url);
            LegacyToDo result = JsonConvert.DeserializeObject<LegacyToDo>(responseStr);
            if (result.Code != 0 || result == null) throw new APIErrorException();

            Code = result.Code;
            Msg = result.Msg;
        }
    }

    public class LegacyToDoDataItem : IToDoDataItem
    {
        public int Type { get; set; }

        public string OrderNo { get; set; }

        public string Message { get; set; }

        public string CommodityName { get; set; }
    }
}
