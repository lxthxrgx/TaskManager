using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskManager.Database;
using TaskManager.Class;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Pages
{
    public class EmploymentModel : PageModel
    {
        private readonly TakClassDatabase _context;
        public EmploymentModel(TakClassDatabase context)
        {
            _context = context;
        }
        public List<User> Users { get; set; }
        public List<Status> Statuses { get; set; } = new List<Status>();
        public ILookup<string, TaskClass> GroupedForms { get; set; } = new List<TaskClass>().ToLookup(f => f.StatusS);
        public TaskClass SelectedForm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Handler { get; set; }
        [BindProperty]
        public string SelectedUserId { get; set; }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(Handler))
            {
                var employment = await _context.employments
                    .FirstOrDefaultAsync(e => e.EmploymentName == Handler);

                if (employment != null)
                {
                    Statuses = await _context.Statuses
                     .Where(s => s.EmpName == employment.EmploymentName)
                     .OrderBy(s => s.Status_position)
                     .ToListAsync();

                    var tasks = await _context.TaskClasses
                        .Where(tc => tc.EmploymentId == employment.Id)
                        .ToListAsync();
                    GroupedForms = tasks.ToLookup(t => t.StatusS);
                }
            }
            else
            {
                Statuses = await _context.Statuses.ToListAsync();
                var tasks = await _context.TaskClasses.ToListAsync();
                GroupedForms = tasks.ToLookup(t => t.StatusS);
            }
            Users = await _context.Users.ToListAsync();
            if (Users == null)
            {
                Users = new List<User>();
            }

            ViewData["Handler"] = Handler;
        }

        public async Task<IActionResult> OnPostOpenModalAsync(int id)
        {
            SelectedForm = await _context.TaskClasses
                .FirstOrDefaultAsync(t => t.Id == id);

            if (SelectedForm != null)
            {
                return Page();
            }
            return NotFound();
        }

        public async Task<IActionResult> OnPostAddTaskAsync()
        {
            string title = Request.Form["Title"];
            string description = Request.Form["Description"];
            string statusName = Request.Form["Status"];
            Handler = Request.Form["Handler"];

            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParse(Request.Form["StartDate"], out startDate) ||
                !DateTime.TryParse(Request.Form["EndDate"], out endDate))
            {
                return BadRequest("Invalid date format");
            }
            Console.WriteLine("HANDLER -------------------------->" + Handler);
            var employment = await _context.employments.FirstOrDefaultAsync(e => e.EmploymentName == Handler);
            if (employment == null)
            {
                return NotFound("Employment not found");
            }
            int employmentId = employment.Id;

            var status = await _context.Statuses.FirstOrDefaultAsync(s => s.Name == statusName);
            if (status == null)
            {
                return NotFound("Status not found");
            }

            var newTask = new TaskClass
            {
                Title = title,
                Task = description,
                StatusS = status.Name,
                EmploymentId = employmentId,
                StartDate = startDate,
                EndDate = endDate
            };

            _context.TaskClasses.Add(newTask);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Employment", new { Handler = Handler });
        }

        public async Task<IActionResult> OnPostAddTaskStatusAsync()
        {
            string title = Request.Form["Title"];
            Handler = Request.Form["Handler"];
            var newStatus = new Status
            {
                Name = title,
                EmpName = Handler
            };

            _context.Statuses.Add(newStatus);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Employment", new { Handler = Handler });
        }

        public async Task<IActionResult> OnPostChangeStatusAsync(string sortedStatusIds, string SelectedUsername)
        {
            if (string.IsNullOrEmpty(sortedStatusIds))
            {
                return BadRequest("Invalid status order");
            }

            var statusIds = sortedStatusIds.Split(',').Select(int.Parse).ToList();

            for (int i = 0; i < statusIds.Count; i++)
            {
                var status = await _context.Statuses.FindAsync(statusIds[i]);
                if (status != null)
                {
                    status.Status_position = i + 1;
                    _context.Statuses.Update(status);
                }
            }

            var user = await _context.Users.FirstOrDefaultAsync(e => e.Username == SelectedUsername);
            if (user == null)
            {
                ModelState.AddModelError("", "Невірний користувач.");
                return Page();
            }

            var empname = await _context.employments.FirstOrDefaultAsync(e => e.EmploymentName == Handler);
            if (empname == null)
            {
                ModelState.AddModelError("", "Employment not found.");
                return Page();
            }

            var existingUserEmployment = await _context.UserEmployments
                .FirstOrDefaultAsync(ue => ue.UserId == user.Id && ue.EmploymentId == empname.Id);

            if (existingUserEmployment == null)
            {
                var addToEmployment = new UserEmployment()
                {
                    UserId = user.Id,
                    EmploymentId = empname.Id
                };
                _context.UserEmployments.Add(addToEmployment);
                await _context.SaveChangesAsync();
            }
            else
            {
                
            }

            return RedirectToPage("/Employment", new { Handler });
        }

    }
}
