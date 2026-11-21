using GroupProject421_521.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GroupProject421_521.Data
{
    /// <summary>
    /// Entity Framework Core context. Inherits the ASP.NET Identity tables and
    /// adds the app's own wines, user profiles and saved prompts.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Wine> Wine { get; set; } = default!;

        // "new" because IdentityDbContext already has a Users set for Identity
        // accounts; this one holds the app's own user profiles.
        public new DbSet<Users> Users { get; set; } = default!;

        public DbSet<UserPrompt> UserPrompt { get; set; } = default!;
    }
}
