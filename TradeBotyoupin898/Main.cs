using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;

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
                    break;
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

        private void toDoListHandle(List<ToDo.TodoDataItem> todoList)
        {
            foreach (var todo in todoList)
            {
                var order = youpinAPI.GetOrder(todo);
                BusinessType businessType;

                businessType = (BusinessType)order.BusinessType;

                // 仅处理租赁业务
                if (businessType != BusinessType.Lease) break;

                LeaseStatus leaseStatus = (LeaseStatus)order.LeaseStatus;

                bool needPhoneConfirm = true;

                switch (leaseStatus)
                {
                    case LeaseStatus.Paied:
                        needPhoneConfirm = true;
                        break;

                    case LeaseStatus.Remand:
                        // 归还订单不需要手机确认
                        needPhoneConfirm = false;
                        break;

                    default:
                        throw new InvalidEnumArgumentException(nameof(leaseStatus), (int)leaseStatus, typeof(LeaseStatus));
                }

                Console.WriteLine(todo.CommodityName);

                Console.WriteLine(order.SteamOfferId, order.OtherSteamId);

                steamAPI.AcceptOffer(order);

                if (needPhoneConfirm)
                {
                    var confs = steamAPI.GetConfirmation();
                    foreach (var conf in confs)
                    {
                        if (conf.Creator != ulong.Parse(order.SteamOfferId)) break;
                        steamAPI.AcceptConfirmation(conf);
                    }
                }
            }
        }

    }
}
