using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TodoMVC.Models;

namespace TodoMVC.Controllers
{
    public class TodoController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<TodoItem> itemList = new List<TodoItem>();
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:58746/api/todo");
            ////   request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);
            var responseString = await response.Content.ReadAsStringAsync();
            itemList = JsonConvert.DeserializeObject<List<TodoItem>>(responseString);
            return View(itemList);
        }

        [HttpPost]
        public async Task<ActionResult> Index(string item)
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(new { Description = item }), System.Text.Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:58746/api/todo");
         //   request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            request.Content = content;
            HttpResponseMessage response = await client.SendAsync(request);

            //
            // Return the To Do List in the view.
            //
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Edit(int id)
        {
           TodoItem item = new TodoItem();
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:58746/api/todo/"+id);
            ////   request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);
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
                HttpContent content = new StringContent(JsonConvert.SerializeObject(new { Description = items[0] }), System.Text.Encoding.UTF8, "application/json");
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:58746/api/todo/" + id);
                ////   request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                request.Content = content;
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return View("Error",new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            TodoItem item = new TodoItem();
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:58746/api/todo/" + id);
            ////   request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);
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
                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:58746/api/todo/" + id);
                HttpResponseMessage response = await client.SendAsync(request);

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