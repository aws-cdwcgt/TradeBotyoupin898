using System.Collections.Generic;

namespace TradeBotyoupin898.APIData.Legacy
{
    public class LegacyToDo
    {
        public int Code { get; set; }

        public string Msg { get; set; }

        public List<LegacyToDoDataItem> Data { get; set; }

        public class LegacyToDoDataItem
        {
            public int Type { get; set; }

            public string OrderNo { get; set; }

            public string Message { get; set; }

            public string CommodityName { get; set; }
        }

    }
}
