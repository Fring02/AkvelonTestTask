using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Task = Domain.Entities.Task;
namespace Data.Contexts;

public class ApplicationContext : DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Task> Tasks { get; set; } = null!;
}