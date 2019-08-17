using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TodoMVC.Utils
{
    public class ApiClient : IApiClient
    {
        public Uri FullUrl { get; set; }
        public string BaseUrl { get; set; }
        public string Route { get; set; }

        private HttpClient client = new HttpClient();
        public string BearerToken { get; set; }

        public async Task<HttpResponseMessage> Delete(string json)
        {
            GetFullUrl();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, FullUrl);
            SetBearerToken(request);
            request.Content = content;
            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Get()
        {
            GetFullUrl();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, FullUrl);
            SetBearerToken(request);
            return await client.SendAsync(request);
        }


        public async Task<HttpResponseMessage> Post(string json)
        {
            GetFullUrl();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, FullUrl);
            SetBearerToken(request);
            request.Content = content;
            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Put(string json)
        {
            GetFullUrl();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, FullUrl);
            SetBearerToken(request);
            request.Content = content;
            return await client.SendAsync(request);
        }

        private void GetFullUrl()
        {
            FullUrl = new Uri(Path.Combine(BaseUrl, Route).Replace("\\", "/"));
        }
        private void SetBearerToken(HttpRequestMessage request)
        {
            if (!string.IsNullOrEmpty(BearerToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
        }

    }
}
