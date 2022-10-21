using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using TradeBotyoupin898.APIData;

namespace TradeBotyoupin898
{
    internal class Main
    {
        private YouPinAPI youpinAPI;
        private SteamAPI steamAPI;

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
                var todoList = youpinAPI.GetToDoList();
                if (todoList == null || todoList.Count == 0)
                {
                    Console.WriteLine("当前没有报价");
                    Console.WriteLine();
                    Thread.Sleep(600000);
                    continue;
                }

                try
                {
                    toDoListHandle(todoList);
                }
                catch (InvalidEnumArgumentException NotHandleException)
                {
                    Console.WriteLine(NotHandleException);
                }

                Thread.Sleep(600000);
            }
        }

        private void toDoListHandle(List<ToDoJson.TodoDataItem> todoList)
        {
            foreach (var todo in todoList)
            {
                string orderID = todo.OrderNo;
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
    }
}
