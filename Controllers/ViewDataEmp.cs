using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Class;
using TaskManager.Database;

namespace TaskManager.Controllers
{
    public class ViewDataEmp : Controller
    {
        private readonly TakClassDatabase _context;

        public ViewDataEmp(TakClassDatabase context)
        {
            _context = context;
        }

        [Route("Tasks/GetTaskById/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetDataById(int id)
        {
            var testviewTest = await _context.TaskClasses
                .FirstOrDefaultAsync(x => x.Id == id);

            if (testviewTest == null)
            {
                return Json(new { success = false, message = $"Запись с TaskClasses = {id} не найдена." });
            }

            return Json(new
            {
                success = true,
                task = new
                {
                    id = testviewTest.Id,
                    title = testviewTest.Title,
                    description = testviewTest.Task,
                    startDate = testviewTest.StartDate.ToString("yyyy-MM-dd"),
                    endDate = testviewTest.EndDate.ToString("yyyy-MM-dd"),
                    status = testviewTest.StatusS
                }
            });
        }

        [Route("Tasks/AddTaskData")]
        [HttpPost]
        public async Task<IActionResult> AddRentData([FromBody] TaskClass model)
        {
            if (model == null)
            {
                return BadRequest(new { success = false, message = "No data provided" });
            }

            if (string.IsNullOrEmpty(model.Title) || model.StartDate == null || model.EndDate == null ||
                string.IsNullOrEmpty(model.StatusS))
            {
                return BadRequest(new { success = false, message = "Incomplete data provided" });
            }
            var existingTaskClass = await _context.TaskClasses
       .FirstOrDefaultAsync(x => x.Id == model.Id);

            if (existingTaskClass == null)
            {
                var newTaskClass = new TaskClass
                {
                    Title = model.Title,
                    Task = model.Task,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    StatusS = model.StatusS
                };

                _context.TaskClasses.Add(newTaskClass);
                await _context.SaveChangesAsync();
            }
            else
            {
                existingTaskClass.Title = model.Title;
                existingTaskClass.Task = model.Task;
                existingTaskClass.StartDate = model.StartDate;
                existingTaskClass.EndDate = model.EndDate;
                existingTaskClass.StatusS = model.StatusS;

                _context.TaskClasses.Update(existingTaskClass);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true });
        }


    }
}
