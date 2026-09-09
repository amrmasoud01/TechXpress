using techXpress.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace techXpress.DataAccess.Data.EntityConfigurations
{
    public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder
                .ToTable("Payments");

            builder
                .HasKey(p => p.PaymentId);

        }
    }
}
