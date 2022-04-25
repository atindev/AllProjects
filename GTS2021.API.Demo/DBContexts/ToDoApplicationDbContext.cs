using GTS2021.API.Demo.Models;
using Microsoft.EntityFrameworkCore;

namespace GTS2021.API.Demo.DbContexts
{
    public class ToDoApplicationDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for this context.</param>
        public ToDoApplicationDbContext(DbContextOptions options) : base(options) { }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>
        /// The user.
        /// </value>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// Gets or sets the user task.
        /// </summary>
        /// <value>
        /// The user task.
        /// </value>
        public DbSet<UserTask> UserTask { get; set; }

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTask>()
            .HasCheckConstraint("CK_UserTask_status", "status <= 5 and status >= 1");
        }
    }
}
