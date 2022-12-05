using Microsoft.EntityFrameworkCore;

namespace auth_and_other
{
    internal class AppDBContext: DbContext
    {
        private const string NameServer = "WIN-GDUP4P8GKP6";
        private const string NameDb = "authAndOtherDB";

        public DbSet<User>? Users { get; set; } = null!;

        public AppDBContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@$"Server={NameServer};Database={NameDb};Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
