// Controllers/TodoController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoApp.MVC.Models.Domain.Concrete;
using TodoApp.MVC.Models.ViewModels;
using TodoApp.MVC.Services.Abstract;

namespace TodoApp.MVC.Controllers;

/*
 * TodoController, yapılacaklar listesi ile ilgili işlemleri gerçekleştirir.
 *
 * Metotlar:
 * - Index: Tüm yapılacakları, tamamlanan veya tamamlanmamış olanları döner.
 * - Create (GET): Yeni bir görev oluşturma sayfasını döner.
 * - Create (POST): Yeni bir görev oluşturur.
 * - Edit (GET): Mevcut bir görevi düzenleme sayfasını döner.
 * - Edit (POST): Düzenlenen görevi günceller.
 * - Delete: Mevcut bir görevi siler.
 * - ToggleComplete: Bir görevin tamamlandığını veya tamamlanmadığını değiştirir.
 */
[Authorize] // Tüm işlemler için giriş zorunlu
public class TodoController : Controller
{
    private readonly IToDoService _todoService;

    public TodoController(IToDoService todoService)
    {
        _todoService = todoService;
    }

    // GET: /Todo
    public async Task<IActionResult> Index(bool? isCompleted = null)
    {
        try
        {
            var todoItems = isCompleted.HasValue
                ? (isCompleted.Value 
                    ? await _todoService.GetCompletedTodosAsync() 
                    : await _todoService.GetIncompleteTodosAsync())
                : await _todoService.GetAllTodosAsync();

            var viewModel = new TodoListViewModel
            {
                AllTodos = todoItems.Select(todo => new TodoViewModel
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    IsCompleted = todo.IsCompleted
                })
            };

            // ViewBag'i güncelle
            ViewBag.IsCompleted = isCompleted;

            return View(viewModel);
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: /Todo/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Todo/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TodoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await _todoService.AddTodoAsync(model.Title, model.Description);
            TempData["SuccessMessage"] = "Görev başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    // GET: /Todo/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var todos = await _todoService.GetAllTodosAsync();
        var todo = todos.FirstOrDefault(t => t.Id == id);

        if (todo == null)
        {
            return NotFound();
        }

        var model = new TodoViewModel
        {
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted
        };

        return View(model);
    }

    // POST: /Todo/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TodoViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var todo = new ToDoItem
            {
                Id = id,
                Title = model.Title,
                Description = model.Description,
                IsCompleted = model.IsCompleted
            };

            await _todoService.UpdateTodoAsync(todo);
            TempData["SuccessMessage"] = "Görev başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    // POST: /Todo/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var todos = await _todoService.GetAllTodosAsync();
            var todo = todos.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            await _todoService.DeleteTodoAsync(todo);
            TempData["SuccessMessage"] = "Görev başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // POST: /Todo/ToggleComplete/5
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        try
        {
            var todoItems = await _todoService.GetAllTodosAsync();
            var todo = todoItems.FirstOrDefault(t => t.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            todo.IsCompleted = !todo.IsCompleted;
            await _todoService.UpdateTodoAsync(todo);

            string message = todo.IsCompleted 
                ? "Görev tamamlandı olarak işaretlendi." 
                : "Görev tamamlanmamış olarak işaretlendi.";
            
            TempData["SuccessMessage"] = message;

            // Görev durumuna göre yönlendirme yap
            return RedirectToAction(nameof(Index), new { isCompleted = todo.IsCompleted });
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}