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
                ToDoJson standard = getHttpResponse<ToDoJson>($"{endpoint_url}youpin/bff/trade/todo/v1/orderTodo/list", Method.Post);
                ToDoItem.AddRange(standard.GetData());

                // FIXME:新版和旧版间获取到的 List 需要去重.
                //LegacyToDo legacy = getHttpResponse<LegacyToDo>($"{endpoint_url}user/Account/ToDoList");
                //ToDoItem.AddRange(legacy.GetData);
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
            // 以 IToDoDataItem.OrderNo 作为新旧 api 的识别方式.
            switch (toDoDataItem.OrderNo.Length)
            {
                case 19:
                    OrderJson orderjson = getHttpResponse<OrderJson>(
                        $"{endpoint_url}youpin/bff/trade/v1/order/query/detail",
                        Method.Post,
                        $"{{\"orderNo\": {toDoDataItem.OrderNo}}}");
                    return orderjson.Data;
                case 26:
                    LegacyOrder legacyOrder = getHttpResponse<LegacyOrder>(
                        $"{endpoint_url}trade/Order/OrderPagedDetail?OrderNo={toDoDataItem.OrderNo}",
                        Method.Get,
                        $"{{\"orderNo\": {toDoDataItem.OrderNo}}}");
                    return legacyOrder.Data;
                default:
                    throw new NotImplementedException($"仅可识别 {nameof(toDoDataItem.OrderNo)} 长度为 19 或 26 的订单.");
            }
        }

        public LeaseOrderData GetLeaseReturnOrder(LegacyOrderData order)
        {
            LeaseOrder legacyOrder = getHttpResponse<LeaseOrder>($"{endpoint_url}v2/commodity/Lease/GetDetail?OrderNo={order.OrderNo}", Method.Get);
            return legacyOrder.Data;
        }

        private T getHttpResponse<T>(string url, Method method, string body = "{}") where T: ICode
        {
            Console.WriteLine(typeof(T));

            string responseStr = request.HttpResponse(url, body, method);
            T result = JsonConvert.DeserializeObject<T>(responseStr);
            if (result.Code != 0 || result == null) throw new APIErrorException();

            return result;
        }
    }
}
