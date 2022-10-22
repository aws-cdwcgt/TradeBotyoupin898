using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using TradeBotyoupin898.Client;
using TradeBotyoupin898.DataStruct;

namespace TradeBotyoupin898
{
    internal class Main
    {
        private YouPinAPI youpinAPI;
        private SteamAPI steamAPI;

        private const int kapi_call_interval = 1000;
        private const int kapi_update_interval = 60000;

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
                        updateSleep(apiCallCount);
                        continue;
                    }

                    try
                    {
                        ProcessAllToDoItem(youpinAPI.ToDoItem);
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

        private void ProcessAllToDoItem(List<IToDoDataItem> itemList)
        {
            foreach (var item in itemList)
            {
                string orderID = item.OrderNo;
                OrderData order = youpinAPI.GetOrder(orderID);
                BusinessType businessType;

                businessType = (BusinessType)order.TradeType.Type;

                /**
                switch (businessType)
                {
                    case BusinessType.Lease:
                        leaseHandle(order);
                        break;

                    case BusinessType.Sell:
                        sellHandle(order);
                        break;

                    default:
                        throw new InvalidEnumArgumentException("尚未支持的业务类型", order.BusinessType, typeof(BusinessType));
                }
                **/
                // 由于对于其他订单状态暂时未知，默认由sellHandle处理
                sellHandle(order);
            }
        }

        /**
        private void leaseHandle(OrderData order)
        {
            //LeaseStatus leaseStatus = (LeaseStatus)order.LeaseStatus;

            bool needPhoneConfirm;

            switch (leaseStatus)
            {
                case LeaseStatus.Paied:
                    needPhoneConfirm = true;
                    break;

                case LeaseStatus.Remand:
                    // 获取归还订单单号，代办所给单号为租赁用
                    order = youpinAPI.GetLeaseReturnOrder(order.OrderNo);
                    // 归还订单不需要手机确认
                    needPhoneConfirm = false;
                    break;

                default:
                    throw new InvalidEnumArgumentException("尚未支持的租赁订单状态", order.LeaseStatus, typeof(LeaseStatus));
            }

            steamConfrim(order, needPhoneConfirm);
        }
        **/


        /// <summary>
        /// 出售订单没有多余的状态
        /// </summary>
        /// <param name="order"></param>
        private void sellHandle(OrderData order)
        {
            steamConfrim(order);
        }

        private void steamConfrim(OrderData order, bool needPhoneConfirm = true)
        {
            steamAPI.AcceptOffer(order);

            if (needPhoneConfirm)
            {
                var confs = steamAPI.GetConfirmation();
                foreach (var conf in confs)
                {
                    if (conf.Creator != order.TradeOfferId) break;
                    while (steamAPI.AcceptConfirmation(conf)) ;
                }
            }
        }

        /// <summary>
        /// Increasing callCount by 1, and sleep api call invteval.
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
