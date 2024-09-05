using Microsoft.EntityFrameworkCore;
using TaskManager.Class;

namespace TaskManager.Database
{
    public class TakClassDatabase : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TaskClass> TaskClasses { get; set; }
        public DbSet<EmploymentClass> employments { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UserEmployment> UserEmployments { get; set; }

        public TakClassDatabase(DbContextOptions<TakClassDatabase> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EmploymentClass>()
                .HasMany(e => e.UserEmployments)
                .WithOne(ue => ue.Employment)
                .HasForeignKey(ue => ue.EmploymentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserEmployments)
                .WithOne(ue => ue.User)
                .HasForeignKey(ue => ue.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserEmployment>()
         .HasIndex(ue => new { ue.UserId, ue.EmploymentId })
         .IsUnique();
        }
    }
}
