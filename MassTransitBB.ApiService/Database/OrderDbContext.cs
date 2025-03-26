using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace MassTransitBB.ApiService.Database
{
    public class OrderDbContext : SagaDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public OrderDbContext(DbContextOptions<OrderDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>(b =>
            {
                b.ToTable("Orders");
                b.HasKey(p=>p.Id);

                b.Property(e => e.ConfirmationEmail).HasMaxLength(300);
                b.HasMany(e => e.OrderItems).WithOne().HasForeignKey("OrderId");
                b.OwnsOne(e => e.PaymentInfo, c =>
                {
                    c.Property(p => p.CVV).HasMaxLength(3);
                    c.Property(p => p.ExpiryDate).HasMaxLength(7);
                    c.Property(p => p.CardNumber).HasMaxLength(20);
                });
            });


            modelBuilder.Entity<OrderItem>(b =>
            {
                b.ToTable("OrderItems");
                b.HasKey("_id");

                b.Property(e => e.ProductId).HasMaxLength(300);
                b.Property(e => e.Quantity);
            });

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get { yield return new OrderStateMap(); }
        }
    }
}