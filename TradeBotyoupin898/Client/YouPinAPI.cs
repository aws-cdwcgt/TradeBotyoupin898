using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using TradeBotyoupin898.DataStruct;
using TradeBotyoupin898.DataStruct.Legacy;

namespace TradeBotyoupin898.Client
{
    internal class YouPinAPI
    {
        private Manifest manifest;
        private string authkey;
        private HttpRequest request;
        private const string endpoint_url = "https://api.youpin898.com/api/";

        public List<IToDoDataItem> ToDoItem { get; private set; } = new List<IToDoDataItem>();


        public YouPinAPI()
        {
            manifest = Manifest.GetManifest();
            request = new HttpRequest(manifest.YouPinAPI);
        }

        public void UpdateAllToDoItem()
        {
            try
            {
                ToDoItem.Clear();
                var standard = new ToDoJson(
                    request.HttpResponse,
                    $"{endpoint_url}youpin/bff/trade/todo/v1/orderTodo/list",
                    string.Empty);
                ToDoItem.AddRange(standard.Data);

                var legacy = new LegacyToDo(
                    request.HttpResponse,
                    $"{endpoint_url}user/Account/ToDoList");
                ToDoItem.AddRange(legacy.Data);
            }
            catch (APIErrorException)
            {
                Console.WriteLine("悠悠API寄了");
            }
            catch (Exception ex)
            {
                Console.WriteLine("出现了未预料的错误");
                Console.WriteLine(ex.ToString());
                return;
            }
        }

        public IOrderData GetOrder(IToDoDataItem toDoDataItem)
        {
            switch (toDoDataItem)
            {
                case ToDoDataItem todoDataItem:
                    OrderJson orderjson = new OrderJson(
                        request.HttpResponse,
                        $"{endpoint_url}youpin/bff/trade/v1/order/query/detail",
                        toDoDataItem.OrderNo);
                    return orderjson.Data;
                case LegacyToDoDataItem legacyToDoDataItem:
                    LegacyOrder legacyOrder = new LegacyOrder(
                        request.HttpResponse,
                        $"{endpoint_url}youpin/bff/trade/v1/order/query/detail",
                        legacyToDoDataItem.OrderNo);
                    return legacyOrder.Data;
                default:
                    throw new NotImplementedException("不支持的类型");
            }

        }

        public OrderData GetLeaseReturnOrder(string orderNo)
        {
            try
            {
                string responseStr = HttpResponse($"{endpoint_url}v2/commodity/Lease/GetDetail?OrderNo={orderNo}");
                var order = JsonConvert.DeserializeObject<LeaseOrder>(responseStr);

                if (order.Code != 0 || order == null) throw new APIErrorException();

                return GetOrder(order.Data.ReturnOrderNo);
            }
            catch (APIErrorException)
            {
                Console.WriteLine("悠悠API寄了");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("出现了未预料的错误");
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
