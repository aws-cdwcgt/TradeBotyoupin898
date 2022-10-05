using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using RestSharp;
using static TradeBotyoupin898.ToDo;

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
                string responseStr = httpResponse($"{endpoint_url}user/Account/ToDoList");
                ToDo todo = JsonConvert.DeserializeObject<ToDo>(responseStr);

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
                string responseStr = httpResponse($"{endpoint_url}trade/Order/OrderPagedDetail?OrderNo={orderNo}");
                Order order = JsonConvert.DeserializeObject<Order>(responseStr);

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


        private string httpResponse(string url)
        {
            RestClientOptions clientOptions = new RestClientOptions(url)
            {
                MaxTimeout = time_out
            };
            var client = new RestClient(clientOptions);
            var request = new RestRequest();
            request.AddHeader("Authorization", $"Bearer {authkey}");
            request.AddHeader("apptype", "3");
            RestResponse response = client.Execute(request);
            return response.Content;
        }

        private class APIErrorException : Exception
        {
        }
    }
}
