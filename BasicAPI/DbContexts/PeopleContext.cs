using BasicAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace BasicAPI.DbContexts
{
    public class PeopleContext : DbContext
    {
        public DbSet<Person> People { get; set; } = null!;

        public PeopleContext(DbContextOptions<PeopleContext> options) : base(options) {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasData(
                new Person("Billy", "Jean")
                {
                    Id = 1,
                    Age = 26,
                    Profession = "Singer"
                },
                new Person("John", "Madden")
                {
                    Id = 2,
                    Age = 58,
                    Profession = "Broadcaster"
                },
                new Person("John", "Smith")
                {
                    Id = 3,
                    Age = 16,
                    Profession = "Programmer"
                },
                new Person("Nicholas", "Bolde")
                {
                    Id = 4,
                    Age = 12,
                    Profession = "Thief"
                },
                new Person("Mark", "Lawrence")
                {
                    Id = 5,
                    Age = 45,
                    Profession = "Writer"
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
