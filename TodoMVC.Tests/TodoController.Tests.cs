using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TodoMVC.Controllers;
using TodoMVC.Models;
using TodoMVC.Utils;

namespace Tests
{
    public class TodoControllerTests
    {
        private AzureAD azureAd = new AzureAD
        {
            ClientId = "ClientId",
            Domain = "Domain",
            Instance = "Instance",
            TenantId = "TenantId"
        };
        private Mock<IOptions<AzureAD>> mockOptions = new Mock<IOptions<AzureAD>>();
        private Mock<IApiClient> mockApiClient = new Mock<IApiClient>();
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestTodoIndexPage()
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(GetAllTodos()), Encoding.UTF8, "application/json");
            mockOptions.Setup(p => p.Value).Returns(azureAd);
            mockApiClient.Setup(x => x.Get()).Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content }));
            var controller = new TodoController(mockOptions.Object, mockApiClient.Object);
            var actionResult = await controller.Index() as ViewResult;
            Assert.IsInstanceOf<ViewResult>(actionResult);
        }

        [Test]
        public async Task TestTodoAddMethod()
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(GetTodo()), Encoding.UTF8, "application/json");
            mockOptions.Setup(p => p.Value).Returns(azureAd);
            mockApiClient.Setup(x => x.Post(It.IsAny<string>())).Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content }));
            var controller = new TodoController(mockOptions.Object, mockApiClient.Object);
            var actionResult = await controller.Index("item") as RedirectToActionResult;
            Assert.IsInstanceOf<RedirectToActionResult>(actionResult);
        }

        [Test]
        public async Task TestTodoEditIndexPage()
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(GetTodo()), Encoding.UTF8, "application/json");
            mockOptions.Setup(p => p.Value).Returns(azureAd);
            mockApiClient.Setup(x => x.Get()).Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content }));
            var controller = new TodoController(mockOptions.Object, mockApiClient.Object);
            var actionResult = await controller.Edit(1) as ViewResult;
            Assert.IsInstanceOf<ViewResult>(actionResult);
        }
        [Test]
        public async Task TestTodoDeleteIndexPage()
        {
            HttpContent content = new StringContent(JsonConvert.SerializeObject(GetTodo()), Encoding.UTF8, "application/json");
            mockOptions.Setup(p => p.Value).Returns(azureAd);
            mockApiClient.Setup(x => x.Get()).Returns(Task.FromResult(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = content }));
            var controller = new TodoController(mockOptions.Object, mockApiClient.Object);
            var actionResult = await controller.Delete(1) as ViewResult;
            Assert.IsInstanceOf<ViewResult>(actionResult);
        }

        private List<TodoItem> GetAllTodos()
        {
            var todoList = new List<TodoItem> {
                 new TodoItem { Description = "first todo" },
                 new TodoItem { Description = "second todo" }
               };
            return todoList;

        }
        private TodoItem GetTodo()
        {
            return new TodoItem { Description = "first todo" };

        }
    }
}