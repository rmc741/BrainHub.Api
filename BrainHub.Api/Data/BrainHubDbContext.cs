using Microsoft.EntityFrameworkCore;

namespace BrainHub.Api.Data
{
    public class BrainHubDbContext : DbContext
    {
        public BrainHubDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(BrainHubDbContext).Assembly);
        }
    }
}
