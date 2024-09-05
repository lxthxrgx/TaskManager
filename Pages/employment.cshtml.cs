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
        public ILookup<string, TaskClass> GroupedForms { get; set; } = new List<TaskClass>().ToLookup(f => f.Status);
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
                    GroupedForms = tasks.ToLookup(t => t.Status);
                }
            }
            else
            {
                Statuses = await _context.Statuses.ToListAsync();
                var tasks = await _context.TaskClasses.ToListAsync();
                GroupedForms = tasks.ToLookup(t => t.Status);
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
    }
}
