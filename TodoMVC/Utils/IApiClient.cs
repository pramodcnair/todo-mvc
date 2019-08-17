using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TodoMVC.Utils
{
    public interface IApiClient
    {
        Uri FullUrl { get; set; }
        string BaseUrl { get; set; }
        string Route { get; set; }
        string BearerToken { get; set; }

        Task<HttpResponseMessage> Get();
        Task<HttpResponseMessage> Post(string json);
        Task<HttpResponseMessage> Put(string json);
        Task<HttpResponseMessage> Delete(string json);
    }
}
