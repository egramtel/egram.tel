using Microsoft.EntityFrameworkCore;
using Tel.Egram.Services.Persistance.Entities;

namespace Tel.Egram.Services.Persistance
{
    public class DatabaseContext : DbContext
    {
        public DbSet<KeyValueEntity> Values { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KeyValueEntity>(m =>
            {
                m.ToTable("key_value");
                
                m.Property(v => v.Key);
                m.Property(v => v.Value);
                
                m.HasKey(v => v.Key);
            });
        }
    }
}