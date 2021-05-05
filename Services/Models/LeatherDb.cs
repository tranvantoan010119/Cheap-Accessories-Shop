namespace Services.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LeatherDb : DbContext
    {
        public LeatherDb()
            : base("name=LeatherDb")
        {
        }

        public virtual DbSet<CartItem> CartItem { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<CustomerInfor> CustomerInfor { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItem> OrderItem { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<ProductCate> ProductCate { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<SubscriptionEmail> SubscriptionEmail { get; set; }
        public virtual DbSet<Wishlist> Wishlist { get; set; }
        public virtual DbSet<AddressDelivery> AddressDelivery { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .Property(e => e.TotalAmount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Order>()
                .HasOptional(e => e.AddressDelivery)
                .WithRequired(e => e.Order);

            modelBuilder.Entity<OrderItem>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<OrderItem>()
                .Property(e => e.LastPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Product>()
                .Property(e => e.SaleOff)
                .HasPrecision(18, 0);
        }
    }
}
