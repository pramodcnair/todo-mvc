using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TodoMVC.Models;
using TodoMVC.Services;

namespace TodoMVC.Controllers
{
    public class TodoController : Controller
    {
        private readonly IOptions<AzureAD> azureAd;
        private readonly IApiClient apiClient;
        public TodoController(IOptions<AzureAD> _azureAd, IApiClient _apiClient)
        {
            azureAd = _azureAd;
            apiClient = _apiClient;
        }
        public async Task<IActionResult> Index()
        {
            List<TodoItem> itemList = new List<TodoItem>();
            apiClient.BaseUrl = azureAd.Value.ApiUrl;
            apiClient.Route = "api/todoquery";
            HttpResponseMessage response = await apiClient.Get();
            var responseString = await response.Content.ReadAsStringAsync();
            itemList = JsonConvert.DeserializeObject<List<TodoItem>>(responseString);
            return View(itemList);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string item)
        {
            apiClient.BaseUrl = azureAd.Value.ApiUrl;
            apiClient.Route = "api/todocommand/add";
            HttpResponseMessage response = await apiClient.Post(JsonConvert.SerializeObject(new { Description = item }));

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Edit(int id)
        {
            TodoItem item = new TodoItem();
            apiClient.BaseUrl = azureAd.Value.ApiUrl;
            apiClient.Route = "api/todoquery/" + id;
            HttpResponseMessage response = await apiClient.Get();
            var responseString = await response.Content.ReadAsStringAsync();
            item = JsonConvert.DeserializeObject<TodoItem>(responseString);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                var items = collection["Description"];
                apiClient.BaseUrl = azureAd.Value.ApiUrl;
                apiClient.Route = "api/todocommand/update";
                HttpResponseMessage response = await apiClient.Put(JsonConvert.SerializeObject(new { Id = id, Description = items[0] }));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            TodoItem item = new TodoItem();
            apiClient.BaseUrl = azureAd.Value.ApiUrl;
            apiClient.Route = "api/todoquery/" + id;
            HttpResponseMessage response = await apiClient.Get();
            var responseString = await response.Content.ReadAsStringAsync();
            item = JsonConvert.DeserializeObject<TodoItem>(responseString);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            try
            {
                apiClient.BaseUrl = azureAd.Value.ApiUrl;
                apiClient.Route = "api/todocommand/delete";
                HttpResponseMessage response = await apiClient.Delete(JsonConvert.SerializeObject(new { Id = id }));

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch
            {
                return View();
            }
        }
    }
}