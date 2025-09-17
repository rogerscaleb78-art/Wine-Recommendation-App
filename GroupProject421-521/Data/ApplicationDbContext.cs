using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GroupProject421_521.Models;

namespace GroupProject421_521.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GroupProject421_521.Models.Wine> Wine { get; set; } = default!;
        public DbSet<GroupProject421_521.Models.Users> Users { get; set; } = default!;
        public DbSet<GroupProject421_521.Models.UserPrompt> UserPrompt { get; set; } = default!;

    }
}