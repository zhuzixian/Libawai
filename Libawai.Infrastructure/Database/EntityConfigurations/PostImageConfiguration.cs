using Libawai.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Libawai.Infrastructure.Database.EntityConfigurations
{
    public class PostImageConfiguration:IEntityTypeConfiguration<PostImage>
    {
        public void Configure(EntityTypeBuilder<PostImage> builder)
        {
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(100);
        }
    }
}
