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

        public List<Status> Statuses { get; set; } = new List<Status>();
        public ILookup<string, TaskClass> GroupedForms { get; set; } = new List<TaskClass>().ToLookup(f => f.StatusS);
        public TaskClass SelectedForm { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Handler { get; set; }

        public async Task OnGetAsync()
        {
            // Load statuses
            if (!string.IsNullOrEmpty(Handler))
            {
                var employment = await _context.employments
                    .FirstOrDefaultAsync(e => e.EmploymentName == Handler);

                if (employment != null)
                {
                    Statuses = await _context.Statuses
                        .Where(s => s.EmpName == employment.EmploymentName)
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
            string handler = Request.Form["Handler"];

            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParse(Request.Form["StartDate"], out startDate) ||
                !DateTime.TryParse(Request.Form["EndDate"], out endDate))
            {
                return BadRequest("Invalid date format");
            }
            Console.WriteLine("HANDLER -------------------------->" + handler);
            var employment = await _context.employments.FirstOrDefaultAsync(e => e.EmploymentName == handler);
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

            return RedirectToPage();
        }


    }
}
