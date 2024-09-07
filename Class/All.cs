namespace TaskManager.Class
{
    public class EmploymentClass
    {
        public int Id { get; set; }
        public string EmploymentName { get; set; }

        public ICollection<UserEmployment> UserEmployments { get; set; } = new List<UserEmployment>();
        public ICollection<TaskClass>? TaskClasses { get; set; } = new List<TaskClass>();
    }

    public class Status
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string EmpName { get; set; }

        public int? EmploymentId { get; set; }
        public EmploymentClass? Employment { get; set; }

        public ICollection<TaskClass> TaskClasses { get; set; } = new List<TaskClass>();
    }

    public class TaskClass
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Task { get; set; }

        private DateTime _startDate;
        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        private DateTime _endDate;
        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        public int? EmploymentId { get; set; }
        public EmploymentClass? Employment { get; set; }

        public string StatusS { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }

        public ICollection<UserEmployment> UserEmployments { get; set; } = new List<UserEmployment>();
    }

    public class UserEmployment
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public User User { get; set; }

        public int EmploymentId { get; set; }
        public EmploymentClass Employment { get; set; }
    }
}
