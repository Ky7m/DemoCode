using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Filters;
using ToDoList.API.ToDoListAPIClient.Models;

namespace ToDoList.API.Controllers
{
    public class HttpOperationExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is Microsoft.Rest.HttpOperationException)
            {
                var ex = (Microsoft.Rest.HttpOperationException)context.Exception;
                context.Response = ex.Response;
            }
        }
    }

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [HttpOperationExceptionFilter]
    public class ToDoListController : ApiController
    {
        private string owner = "*";

        private static ToDoListAPIClient.ToDoListAPIClient NewDataAPIClient()
        {
            var client = new ToDoListAPIClient.ToDoListAPIClient(new Uri(ConfigurationManager.AppSettings["toDoListDataAPIURL"]));
            return client;
        }

        // GET: api/ToDoItemList
        public async Task<IEnumerable<ToDoItem>> Get()
        {
            using (var client = NewDataAPIClient())
            {
                var results = await client.ToDoList.GetWithOperationResponseAsync(owner);
                return results.Body.Select(m => new ToDoItem
                {
                    Description = m.Description,
                    ID = m.ID,
                    Owner = m.Owner
                });
            }
        }

        // GET: api/ToDoItemList/5
        public async Task<ToDoItem> GetByID(int id)
        {
            using (var client = NewDataAPIClient())
            {
                var result = await client.ToDoList.GetByIdWithOperationResponseAsync(owner, id);
                return new ToDoItem
                {
                    Description = result.Body.Description,
                    ID = result.Body.ID,
                    Owner = result.Body.Owner
                };
            }
        }

        // POST: api/ToDoItemList
        public async Task Post(ToDoItem todo)
        {
            todo.Owner = owner;
            using (var client = NewDataAPIClient())
            {
                await client.ToDoList.PostWithOperationResponseAsync(new ToDoItem
                {
                    Description = todo.Description,
                    ID = todo.ID,
                    Owner = todo.Owner
                });
            }
        }

        public async Task Put(ToDoItem todo)
        {
            todo.Owner = owner;
            using (var client = NewDataAPIClient())
            {
                await client.ToDoList.PutWithOperationResponseAsync(new ToDoItem
                {
                    Description = todo.Description,
                    ID = todo.ID,
                    Owner = todo.Owner
                });
            }
        }

        // DELETE: api/ToDoItemList/5
        public async Task Delete(int id)
        {
            using (var client = NewDataAPIClient())
            {
                await client.ToDoList.DeleteWithOperationResponseAsync(owner, id);
            }
        }
    }
}