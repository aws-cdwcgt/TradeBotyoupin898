using System.Collections.Generic;

namespace TradeBotyoupin898.DataStruct
{
    internal interface IToDo
    {
        public List<IToDoDataItem> GetData();
    }

    public interface IToDoDataItem
    {
        public int Type { get; }

        /// <summary>
        /// 
        /// </summary>
        public string OrderNo { get; }

        /// <summary>
        /// 有承租方下单，等待确认报价
        /// </summary>
        public string Message { get; }

        public string CommodityName { get; }
    }

    public interface ICode
    {
        public int Code { get; }
    }
}
