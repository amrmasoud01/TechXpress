using techXpress.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace techXpress.DataAccess.Data.EntityConfigurations
{
    public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .ToTable("Orders");

            builder
                .HasKey(o =>  o.OrderId);

            builder
                .Property(o => o.OrderStatus)
                .HasConversion<string>();

            builder
                .HasOne(o => o.Payment)
                .WithOne()
                .HasForeignKey<Order>(o => o.PaymentId)
                .HasPrincipalKey<Payment>(p => p.PaymentId);
                

            builder
                .HasOne(o => o.Coupon)
                .WithOne()
                .HasForeignKey<Order>(o => o.CouponId)
                .HasPrincipalKey<Coupon>(c => c.CouponId);

            builder
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .HasPrincipalKey(u => u.Id);
        }
    }
}
