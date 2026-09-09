using techXpress.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace techXpress.DataAccess.Data.EntityConfigurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .ToTable("Categories");

            builder
                .HasKey(c => c.CategoryId);

            builder
                .Property(c => c.Name)
                .IsRequired();
        }
    }
}
