using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;
using ToDoList.DataAPI.Models;

namespace ToDoList.DataAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ToDoListController : ApiController
    {
        private static readonly Dictionary<int, ToDoItem> MockData = new Dictionary<int, ToDoItem>();

        static ToDoListController()
        {
            MockData.Add(0, new ToDoItem { ID = 0, Owner = "*", Description = "Application map" });
            MockData.Add(1, new ToDoItem { ID = 1, Owner = "*", Description = "Live Metrics Stream" });
            MockData.Add(2, new ToDoItem { ID = 2, Owner = "*", Description = "Failures" });
            MockData.Add(3, new ToDoItem { ID = 3, Owner = "*", Description = "Performance" });
            MockData.Add(4, new ToDoItem { ID = 4, Owner = "*", Description = "Browser" });
            MockData.Add(5, new ToDoItem { ID = 5, Owner = "*", Description = "Usage" });
        }

        public IEnumerable<ToDoItem> Get(string owner)
        {
            Thread.Sleep(3000);
            return MockData.Values.Where(m => m.Owner == owner || owner == "*");
        }

        public ToDoItem GetById(string owner, int id)
        {
            Thread.Sleep(3000);
            return MockData.Values.First(m => (m.Owner == owner || owner == "*" ) && m.ID == id);
        }

        public void Post(ToDoItem todo)
        {
            Thread.Sleep(3000);
            todo.ID = MockData.Count > 0 ? MockData.Keys.Max() + 1 : 1;
            MockData.Add(todo.ID, todo);
        }

        public void Put(ToDoItem todo)
        {
            Thread.Sleep(3000);
            var xtodo = MockData.Values.First(a => (a.Owner == todo.Owner || todo.Owner == "*") && a.ID == todo.ID);
            if (todo != null && xtodo != null)
            {
                xtodo.Description = todo.Description;
            }
        }

        public void Delete(string owner, int id)
        {
            Thread.Sleep(3000);
            var todo = MockData.Values.First(a => (a.Owner == owner || owner == "*") && a.ID == id);
            if (todo != null)
            {
                MockData.Remove(todo.ID);
            }
        }
    }
}

