namespace TradeBotyoupin898.DataStruct
{
    /// <summary>
    /// 业务类型，分为求购租赁与出售
    /// 由 <see cref="OrderData.BusinessType"/> 提供
    /// </summary>
    public enum OrderType
    {
        Supply = 1,
        Sell = 2,
        Lease = 20,
    }

    public enum LegacyOrderType
    {
        Sell = 3,
        Lease = 20,
    }
}
