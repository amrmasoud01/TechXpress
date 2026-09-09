using techXpress.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace techXpress.DataAccess.Data.EntityConfigurations
{
    public class SellerEntityTypeConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder
                .HasKey(s => s.SellerId);

            builder
                .ToTable("Sellers");

            builder
                .Property(s => s.SellerName)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
