using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TodoApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TodoApp.Controllers
{
    public class HomeController : Controller
    {
        TodoContext db;

        public HomeController(TodoContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await db.TodoItems.ToListAsync());
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(TodoItem task)
        {
            if (String.IsNullOrEmpty(task.Title))
            {
                ModelState.AddModelError("", "Задача должна иметь название");
            }

            if (ModelState.IsValid)
            {
                db.TodoItems.Add(task);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                TodoItem item = new TodoItem { ID = id.Value };
                db.Entry(item).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Update(int? id, bool status)
        {
            if (id != null)
            {

                TodoItem? item = await db.TodoItems.FirstOrDefaultAsync(p => p.ID == id);

                if (item != null)
                {
                    item.IsDone = status;
                    db.TodoItems.Update(item);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                TodoItem? item = await db.TodoItems.FirstOrDefaultAsync(p => p.ID == id);

                if (item != null)
                {
                    return View(item);
                }

            }
            return NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(TodoItem task)
        {
            if (String.IsNullOrEmpty(task.Title))
            {
                ModelState.AddModelError("Title", "Задача должна иметь название");
            }


            db.TodoItems.Update(task);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}