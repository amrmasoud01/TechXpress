using techXpress.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace techXpress.DataAccess.Data.EntityConfigurations
{
    public class OrderDetailsEntityTypeConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder
                .ToTable("OrderDetails");

            builder
                .HasKey(d => new {d.OrderId , d.ProductId});

            builder
                .HasOne(d => d.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(d => d.OrderId);

            builder
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId);
        }
    }
}
