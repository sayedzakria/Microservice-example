

using Ordering.Domain.Enums;

namespace Ordering.Infratstructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
           builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                .HasConversion(
                    id => id.Value,
                    value =>  OrderId.Of(value));
            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId).IsRequired();
            builder.HasMany<OrderItem>()
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);

             builder.ComplexProperty(o => o.OrderName,nameBuilder=>
             {
                 nameBuilder.Property(on => on.Value).HasColumnName(nameof(Order.OrderName))
                 .HasMaxLength(100)
                 .IsRequired();
             });
            builder.ComplexProperty(o => o.ShippingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FisrtName)
                .HasMaxLength(50)
                .IsRequired();
                addressBuilder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();
                addressBuilder.Property(a => a.EmailAddress)
                .HasMaxLength(50);
                addressBuilder.Property(a => a.AddressLine)
                .HasMaxLength(200)
                .IsRequired();
                addressBuilder.Property(a => a.Country)
                .HasMaxLength(50);
                addressBuilder.Property(a => a.State)
                .HasMaxLength(50);
                addressBuilder.Property(a => a.ZipCode)
                .HasMaxLength(20);

            });
            builder.ComplexProperty(o => o.BillingAddress, addressBuilder =>
            {
                addressBuilder.Property(a => a.FisrtName)
                .HasMaxLength(50)
                .IsRequired();
                addressBuilder.Property(a => a.LastName)
                .HasMaxLength(50)
                .IsRequired();
                addressBuilder.Property(a => a.EmailAddress)
                .HasMaxLength(50);
                addressBuilder.Property(a => a.AddressLine)
                .HasMaxLength(200)
                .IsRequired();
                addressBuilder.Property(a => a.Country)
                .HasMaxLength(50);
                addressBuilder.Property(a => a.State)
                .HasMaxLength(50);
                addressBuilder.Property(a => a.ZipCode)
                .HasMaxLength(20);
             });
             builder.ComplexProperty(o => o.Payment, paymentBuilder =>
             {
                 paymentBuilder.Property(p => p.CardName)
                 .HasMaxLength(50);
                  paymentBuilder.Property(p => p.CardNumber)
                 .HasMaxLength(24)
                 .IsRequired();
                 paymentBuilder.Property(p => p.Expiration)
                 .HasMaxLength(10)
                 .IsRequired();
                 paymentBuilder.Property(p => p.CVV)
               .HasMaxLength(3);
                 paymentBuilder.Property(p => p.PaymentMethod);
             });
            builder.Property(o => o.Status)
            .HasDefaultValue(OrderStatus.Draft)
                .HasConversion(
                s => s.ToString(),
                dbstatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbstatus)
                );
            builder.Property(o => o.TotalPrice);


        }
    }
}
