using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using TradeBotyoupin898.Client;
using TradeBotyoupin898.DataStruct;
using TradeBotyoupin898.DataStruct.Legacy;

namespace TradeBotyoupin898
{
    internal class Main
    {
        private YouPinAPI youpinAPI;
        private SteamAPI steamAPI;

        private const int kapi_call_interval = 1000;
        private const int kapi_update_interval = 600000;

        public Main()
        {
            Stream stream = Console.OpenStandardInput();
            Console.SetIn(new StreamReader(stream, Encoding.Default, false, 5000));
            steamAPI = new SteamAPI();
            youpinAPI = new YouPinAPI();
        }

        public void Start()
        {
            while (true)
            {
                ushort apiCallCount = 0;

                try
                {
                    youpinAPI.UpdateAllToDoItem();

                    if (youpinAPI.ToDoItem.Count == 0)
                    {
                        Console.WriteLine("当前没有报价");
                        continue;
                    }

                    try
                    {
                        processAllToDoItem(youpinAPI.ToDoItem);
                    }
                    catch (InvalidEnumArgumentException NotHandleException)
                    {
                        Console.WriteLine(NotHandleException);
                    }
                }
                catch (APIErrorException)
                {
                    Console.WriteLine("悠悠API寄了.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("出现了未预料的错误.");
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    Console.WriteLine(DateTime.UtcNow);
                    updateSleep(apiCallCount);
                }
            }
        }

        private void processAllToDoItem(List<IToDoDataItem> itemList)
        {
            foreach (var item in itemList)
            {
                switch (youpinAPI.GetOrder(item))
                {
                    case OrderData orderData:
                        dealOrderData(orderData);
                        break;
                    case LegacyOrderData legacyOrderData:
                        dealOrderData(legacyOrderData);
                        break;
                    default:
                        throw new NotImplementedException($"未支持的 {nameof(IOrderData)} 类型.");
                }
            }
        }

        private void dealOrderData(OrderData order)
        {
            switch ((OrderType)order.GetOrderType())
            {
                case OrderType.Supply:
                    steamAPI.AcceptOffer(order, false);
                    break;
                case OrderType.Sell:
                    steamAPI.AcceptOffer(order, true);
                    break;
                case OrderType.Lease:

                    break;
                default:
                    throw new NotImplementedException($"未支持的 {order.GetOrderType()} 类型.");
            }
        }

        private void dealOrderData(LegacyOrderData order)
        {
            switch ((LegacyOrderType)order.GetOrderType())
            {
                case LegacyOrderType.Sell:
                    steamAPI.AcceptOffer(order, true);
                    break;
                case LegacyOrderType.Lease:
                    dealLease(order);
                    break;
                default:
                    throw new NotImplementedException($"未支持的 {order.GetOrderType()} 类型.");
            }
        }

        private void dealSupply(IOrderData order)
        {
            steamAPI.AcceptOffer(order, false);
        }

        private void dealSell(IOrderData order)
        {
            steamAPI.AcceptOffer(order, true);
        }

        private void dealLease(LegacyOrderData order)
        {
            switch ((LeaseStatus)order.GetLeaseStatus())
            {
                case LeaseStatus.Paied:
                    steamAPI.AcceptOffer(order, true);
                    break;
                case LeaseStatus.Remand:
                    LeaseOrderData remand = youpinAPI.GetLeaseReturnOrder(order);
                    steamAPI.AcceptOffer(remand, false);
                    break;
                default:
                    throw new InvalidEnumArgumentException("尚未支持的租赁订单状态", order.GetLeaseStatus(), typeof(LeaseStatus));
            }
        }

        /// <summary>
        /// Increasing callCount by 1, and sleep kapi_call_invteval milliseconds.
        /// </summary>
        private void callSleep(ref ushort callCount)
        {
            callCount++;
            Thread.Sleep(kapi_call_interval);
        }

        private void callSleep()
        {
            Thread.Sleep(kapi_call_interval);
        }

        /// <summary>
        /// slp 一个刷新间隔，减去已经 slp 的时间.
        /// </summary>
        /// <param name="callCount">这次更新中总共刷新的次数.</param>
        private void updateSleep(ushort callCount)
        {
            int remainTime = kapi_update_interval - (callCount * kapi_call_interval);
            if (remainTime <= kapi_call_interval)
            {
#if DEBUG
                Console.WriteLine($"{DateTime.UtcNow}\t过多的API调用: {callCount}次!");
#endif
                callSleep();
            }
            else
            {
                Thread.Sleep(kapi_update_interval);
            }
        }
    }
}
