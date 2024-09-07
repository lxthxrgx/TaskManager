using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.JSInterop;
using TaskManager.Database;
using TaskManager.Class;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Sha512;

namespace TaskManager.Pages
{
    public class HomeModel : PageModel
    {
        private readonly TakClassDatabase _context;
        private readonly IJSRuntime _jsRuntime;

        public HomeModel(TakClassDatabase context, IJSRuntime jsRuntime)
        {
            _context = context;
            _jsRuntime = jsRuntime;
        }

        public List<EmploymentClass> UserEmployment { get; set; } = new List<EmploymentClass>();

        [BindProperty]
        public string NameEmployment { get; set; }

        public async Task OnGetAsync()
        {
            //await SeedTestDataAsync();
            var username = User.Identity.Name;

            var useridFromUser = await _context.Users
                .Where(x => x.Username == username)
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            UserEmployment = await _context.UserEmployments
                .Where(ue => ue.UserId == useridFromUser)
                .Select(ue => ue.Employment)
                .ToListAsync();
            Console.WriteLine(NameEmployment + "----------------------");
        }

        private async Task SeedTestDataAsync()
        {
            if (!await _context.Users.AnyAsync())
            {
                var user1 = new User { Username = "coldxrms", Password = HashHelper.ComputeSha512Hash("qwe") };
                var user2 = new User { Username = "testuser2", Password = HashHelper.ComputeSha512Hash("qwe") };
                _context.Users.AddRange(user1, user2);
                await _context.SaveChangesAsync();

                var employment1 = new EmploymentClass { EmploymentName = "Software Engineer" };
                var employment2 = new EmploymentClass { EmploymentName = "Project Manager" };
                _context.employments.AddRange(employment1, employment2);
                await _context.SaveChangesAsync();

                var status1 = new Status { Name = "To Do", EmpName = "Software Engineer" };
                var status2 = new Status { Name = "In Progress", EmpName = "Software Engineer" };
                var status3 = new Status { Name = "Completed", EmpName = "Project Manager" };
                _context.Statuses.AddRange(status1, status2, status3);
                await _context.SaveChangesAsync();

                var task1 = new TaskClass { Title = "Task 1", Task = "Description for Task 1", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1), StatusS = "To Do", Employment = employment1 };
                var task2 = new TaskClass { Title = "Task 2", Task = "Description for Task 2", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(2), StatusS = "In Progress", Employment = employment1 };
                var task3 = new TaskClass { Title = "Task 3", Task = "Description for Task 3", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(3), StatusS = "Completed", Employment = employment2 };
                _context.TaskClasses.AddRange(task1, task2, task3);
                await _context.SaveChangesAsync();

                var userEmployment1 = new UserEmployment { User = user1, Employment = employment1 };
                var userEmployment2 = new UserEmployment { User = user2, Employment = employment2 };
                _context.UserEmployments.AddRange(userEmployment1, userEmployment2);
                await _context.SaveChangesAsync();
            }
        }

        public string BuildEmploymentUrl(string name)
        {
            return $"/employment/{name}";
        }

        public async Task<IActionResult> OnPostSaveEmploymentAsync()
        {
           
            try
            {
                Console.WriteLine(NameEmployment+"----------------------");
                var employment = new EmploymentClass { EmploymentName = NameEmployment };
                _context.employments.Add(employment);
                await _context.SaveChangesAsync();

                var username = User.Identity.Name;

                var useridFromUser = await _context.Users
                    .Where(x => x.Username == username)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                var userEmployment = new UserEmployment
                {
                    UserId = useridFromUser,
                    EmploymentId = employment.Id
                };
                _context.UserEmployments.Add(userEmployment);
                await _context.SaveChangesAsync();
                var statuses = new List<Status>
            {
                new Status { Name = "Open", EmpName = employment.EmploymentName, EmploymentId = employment.Id },
                new Status { Name = "In Progress", EmpName = employment.EmploymentName, EmploymentId = employment.Id },
                new Status { Name = "Closed", EmpName = employment.EmploymentName, EmploymentId = employment.Id }
            };
                _context.Statuses.AddRange(statuses);

                await _context.SaveChangesAsync();
            }
            catch(Exception ex) { Console.WriteLine($"MESSAGE: {ex}"); }
           

            return RedirectToPage();
        }
    }
}
