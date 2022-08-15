using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using ProjectManager.Models;

namespace ProjectManager.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project>   Projects   { get; set; } = default;
    public DbSet<NTask>     NTasks     { get; set; } = default;
    public DbSet<Event>     Events     { get; set; } = default;
    public DbSet<EventType> EventTypes { get; set; } = default;
}

