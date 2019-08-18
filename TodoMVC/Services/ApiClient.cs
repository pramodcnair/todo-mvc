using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TodoMVC.Services
{
    public class ApiClient : IApiClient
    {
        private ITokenService tokenService;
        public Uri FullUrl { get; set; }
        public string BaseUrl { get; set; }
        public string Route { get; set; }

        private HttpClient client = new HttpClient();
        public string BearerToken { get; set; }
        public ApiClient(ITokenService _tokenService)
        {
            tokenService = _tokenService;
        }
        public async Task<HttpResponseMessage> Delete(string json)
        {
            GetFullUrl();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, FullUrl);
            await SetBearerTokenAsync(request);
            request.Content = content;
            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Get()
        {
            GetFullUrl();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, FullUrl);
            await SetBearerTokenAsync(request);
            return await client.SendAsync(request);
        }


        public async Task<HttpResponseMessage> Post(string json)
        {
            GetFullUrl();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, FullUrl);
            await SetBearerTokenAsync(request);
            request.Content = content;
            return await client.SendAsync(request);
        }

        public async Task<HttpResponseMessage> Put(string json)
        {
            GetFullUrl();
            HttpContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, FullUrl);
            await SetBearerTokenAsync(request);
            request.Content = content;
            return await client.SendAsync(request);
        }

        private void GetFullUrl()
        {
            FullUrl = new Uri(Path.Combine(BaseUrl, Route).Replace("\\", "/"));
        }
        private async Task SetBearerTokenAsync(HttpRequestMessage request)
        {
            var token = await tokenService.GetToken();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

    }
}
