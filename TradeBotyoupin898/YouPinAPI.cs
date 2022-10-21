using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;
using TradeBotyoupin898.APIData;
using TradeBotyoupin898.APIData.Legacy;
using static TradeBotyoupin898.APIData.Legacy.LegacyToDo;
using static TradeBotyoupin898.APIData.ToDoJson;

namespace TradeBotyoupin898
{
    internal class YouPinAPI
    {
        private Manifest manifest;
        private string authkey;
        private const string endpoint_url = "https://api.youpin898.com/api/";

        private const int time_out = 10000;

        public YouPinAPI()
        {
            manifest = Manifest.GetManifest();
            authkey = manifest.YouPinAPI;
        }

        public List<TodoDataItem> GetToDoList()
        {
            try
            {
                string responseStr = httpResponse($"{endpoint_url}youpin/bff/trade/todo/v1/orderTodo/list");
                ToDoJson todo = JsonConvert.DeserializeObject<ToDoJson>(responseStr);

                if (todo.Code != 0 || todo == null) throw new APIErrorException();
                return todo.Data;
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

        public List<LegacyToDoDataItem> GetLegecyToDoList()
        {
            try
            {
                string responseStr = httpResponse($"{endpoint_url}user/Account/ToDoList", string.Empty);
                LegacyToDo todo = JsonConvert.DeserializeObject<LegacyToDo>(responseStr);

                if (todo.Code != 0 || todo == null) throw new APIErrorException();
                return todo.Data;
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

        public OrderData GetLeaseReturnOrder(string orderNo)
        {
            try
            {
                string responseStr = httpResponse($"{endpoint_url}v2/commodity/Lease/GetDetail?OrderNo={orderNo}");
                LeaseOrder order = JsonConvert.DeserializeObject<LeaseOrder>(responseStr);

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

        public OrderData GetOrder(string orderNo)
        {
            try
            {
                string responseStr = httpResponse($"{endpoint_url}youpin/bff/trade/v1/order/query/detail", $"{{\"orderNo\": {orderNo}}}");
                OrderJson order = JsonConvert.DeserializeObject<OrderJson>(responseStr);

                if (order.Code != 0 || order == null) throw new APIErrorException();

                return order.Data;
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

        public LegacyOrderData GetLegacyOrder(string orderNo)
        {
            try
            {
                string responseStr = httpResponse($"{endpoint_url}youpin/bff/trade/v1/order/query/detail", $"{{\"orderNo\": {orderNo}}}");
                LegacyOrder order = JsonConvert.DeserializeObject<LegacyOrder>(responseStr);

                if (order.Code != 0 || order == null) throw new APIErrorException();

                return order.Data;
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

        private string httpResponse(string url) => httpResponse(url, "{}");

        private string httpResponse(string url, string content)
        {
            RestClientOptions clientOptions = new RestClientOptions(url)
            {
                MaxTimeout = time_out
            };
            var client = new RestClient(clientOptions);
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddHeader("Authorization", $"Bearer {authkey}");
            request.AddHeader("apptype", "3");
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(content);
            RestResponse response = client.Execute(request);
            return response.Content;
        }

        private class APIErrorException : Exception
        {
        }
    }
}
