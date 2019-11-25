using Libawai.Core.Entities;
using Libawai.Infrastructure.Database.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Libawai.Infrastructure.Database
{
    public class LibawaiDbContext:DbContext
    {
        public LibawaiDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PostConfiguration());
            modelBuilder.ApplyConfiguration(new PostImageConfiguration());
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImage> PostImages { get; set; }
    }
}
