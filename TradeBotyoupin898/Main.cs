using Newtonsoft.Json.Bson;
using SteamAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Specialized;

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

                foreach (var todo in todoList)
                {
                    var order = youpinAPI.GetOrder(todo);
                    BusinessType businessType;

                    try
                    {
                        businessType = (BusinessType)order.BusinessType;
                    }
                    catch
                    {
                        Console.WriteLine($"不支持的业务类型 {order.BusinessType}");
                        break;
                    }

                    // 仅处理租赁业务
                    if (businessType != BusinessType.Lease) break;

                    LeaseStatus leaseStatus;

                    try
                    {
                        leaseStatus = (LeaseStatus)order.Status;
                    }
                    catch
                    {
                        Console.WriteLine($"不支持的租赁订单状态 {order.Status}");
                        break;
                    }

                    bool needPhoneConfirm = true;
                    bool canHandle = true;

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
                            canHandle = false;
                            break;
                    }

                    if (!canHandle) break;

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

                Thread.Sleep(600000);
            }
        }
    }
}
