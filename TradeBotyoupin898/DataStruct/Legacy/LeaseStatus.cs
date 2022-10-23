namespace TradeBotyoupin898.DataStruct.Legacy
{
    /// <summary>
    /// 出租时会遇到的所有状态
    /// <see cref="LegacyOrderData.LeaseStatus"/> 提供状态
    /// </summary>
    public enum LeaseStatus
    {
        Paied = 15,
        Lease = 20,
        Remand = 30,

        // 客服介入
        Intervention = 80,
        Buyout = 610,
        Complete = 600,
        Failed = 1000,
    }
}
