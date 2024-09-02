using Evaluation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Evaluation.DB
{
    /// <summary>
    /// The database context for the application, inheriting from IdentityDbContext.
    /// This context manages the application's identity and sales data.
    /// </summary>
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        /// <summary>
        /// Initializes a new instance of the AppDbContext class with the given options.
        /// </summary>
        /// <param name="options">Options for configuring the database context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /// <summary>
        /// This represents the Sales table in the database.
        /// </summary>
        public DbSet<Sale> Sales { get; set; }
    }
}