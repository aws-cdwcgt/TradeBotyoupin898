using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
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
                string responseStr = HttpResponse($"{endpoint_url}user/Account/ToDoList");
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

        public OrderData GetOrder(TodoDataItem toDo)
        {
            try
            {
                string responseStr = HttpResponse($"{endpoint_url}trade/Order/OrderPagedDetail?OrderNo={toDo.OrderNo}");
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


        private string HttpResponse(string url)
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

        class APIErrorException : Exception
        {
        }
    }
}
