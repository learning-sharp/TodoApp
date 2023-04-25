using Microsoft.EntityFrameworkCore;


namespace TodoApp.Models
{
    public class TodoContext : DbContext
    {
        public DbSet<TodoItem> TodoItems { get; set; } = null!;

        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>().HasData(
                    new TodoItem { ID = 1, Title = "Позлиться", Description = "Text text text text text text text text text text text text text" },
                    new TodoItem { ID = 2,  Title = "Собраться с силами", Description = "Text text text text text text text text text text text text text" },
                    new TodoItem { ID = 3, Title = "Начать заново", Description = "Text text text text text text text text text text text text text" }
            );
        }
    }
}
