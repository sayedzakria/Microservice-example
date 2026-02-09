


namespace Ordering.Infratstructure.Data.Configurations
{
    public class orderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.HasKey(oi => oi.Id);
            builder.Property(oi => oi.Id)
                .HasConversion(oi => oi.Value,
                value => OrderItemId.Of(value));
            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);
            builder.Property(oi => oi.Price)
                            .IsRequired();
            builder.Property(oi => oi.Quantity)
                .IsRequired();
        }
    }
}
