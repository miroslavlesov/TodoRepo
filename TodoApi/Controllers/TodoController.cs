using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext context;

        public TodoController(TodoContext context)
        {
            this.context = context;

            if (this.context.TodoItems.Count() == 0) {
                this.context.TodoItems.Add(new TodoItem { Name = "Item1" });
                this.context.SaveChanges();
            }
        }

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return this.context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(long id) {
            var item = this.context.TodoItems.Find(id);

            if (item == null) {
                return NotFound();
            }

            return item;
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, TodoItem item) {
            var todo = this.context.TodoItems.Find(id);
            if (todo == null) {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            this.context.TodoItems.Update(todo);
            this.context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id) {
            var todo = this.context.TodoItems.Find(id);

            if (todo == null) {
                return NotFound();
            }

            this.context.Remove(todo);
        }  
    }
}