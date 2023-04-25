using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [Route("api/TodoItems")]
    [ApiController]
    public class TodoItemsAPIController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsAPIController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItemsAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }

          return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItemsAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpGet]
        [Route("getCompleted")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetCompletedTodoItem(int id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            return await _context.TodoItems.Where(e => e.IsDone).ToListAsync();
        }


        [HttpGet]
        [Route("getUncompleted")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetUncompletedTodoItem(int id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            return await _context.TodoItems.Where(e => !e.IsDone).ToListAsync();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, TodoItem todoItem)
        {
            if (id != todoItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
          if (String.IsNullOrEmpty(todoItem.Title))
          {
                return Problem("Задача должна иметь название");
          }

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.ID }, todoItem);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(int id)
        {
            return (_context.TodoItems?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
