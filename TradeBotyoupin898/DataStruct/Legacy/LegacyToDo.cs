using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TradeBotyoupin898.Client;

namespace TradeBotyoupin898.DataStruct.Legacy
{
    [Obsolete]
    public class LegacyToDo : IToDo, ICode
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        // FIXME: (Error Prone)需要指明为更具体的 LegacyTodoDataItem 而不是 IToDoDataItem.
        public List<LegacyToDoDataItem> Data { get; set; }

        public List<IToDoDataItem> GetData() => Data.Select(i => (IToDoDataItem)i).ToList();
    }

    [Obsolete]
    public class LegacyToDoDataItem : IToDoDataItem
    {
        public int Type { get; set; }

        public string OrderNo { get; set; }

        public string Message { get; set; }

        public string CommodityName { get; set; }
    }
}
