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
using SteamAuth;
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

                foreach(var todo in todoList)
                {
                    Console.WriteLine(todo.CommodityName);
                    var order = youpinAPI.GetOrder(todo);

                    Console.WriteLine(order.SteamOfferId, order.OtherSteamId);

                    steamAPI.AcceptOffer(order);
                    var confs = steamAPI.GetConfirmation();
                    foreach(var conf in confs)
                    {
                        if (conf.Creator != ulong.Parse(order.SteamOfferId)) break;
                        steamAPI.AcceptConfirmation(conf);
                    }
                }

                Thread.Sleep(600000);
            }
        }
    }
}
