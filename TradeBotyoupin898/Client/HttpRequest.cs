using System;
using System.Runtime.Serialization.Formatters;
using RestSharp;

namespace TradeBotyoupin898.Client
{
    internal class HttpRequest
    {
        private const int time_out = 10000;

        private string authkey;

        public HttpRequest(string authkey)
        {
            this.authkey = authkey;
        }

        internal string HttpResponse(string url) => HttpResponse(url, "{}");

        internal string HttpResponse(string url, string content, Method method = Method.Get)
        {
            var clientOptions = new RestClientOptions(url)
            {
                MaxTimeout = time_out
            };
            var client = new RestClient(clientOptions);
            var request = new RestRequest
            {
                Method = method,
            };
            request.AddHeader("Authorization", $"Bearer {authkey}");
            request.AddHeader("apptype", "3");
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(content);
            var response = client.Execute(request);
            return response.Content;
        }
    }
}
