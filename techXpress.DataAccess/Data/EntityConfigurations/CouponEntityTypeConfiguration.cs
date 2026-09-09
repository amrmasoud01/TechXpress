using techXpress.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace techXpress.DataAccess.Data.EntityConfigurations
{
    public class CouponEntityTypeConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder
                .ToTable("Coupons");

            builder
                .HasKey(c => c.CouponId);

            builder
                .Property(c => c.Code)
                .IsRequired();
        }
    }
}
