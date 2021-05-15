using Microsoft.EntityFrameworkCore;

namespace Mall.Models
{
    public partial class MallDbContext : DbContext
    {
        public MallDbContext () {
        }

        public MallDbContext(DbContextOptions<MallDbContext> options) : base(options) {
        }

        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<MallCenter> Mall { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Product_category> Product_category { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Store> Store { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<WorkingTime> WorkingTime { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=MallDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity => 
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryName).IsRequired();
                entity.Property(e => e.CategoryDescription);
            });

            modelBuilder.Entity<MallCenter>(entity =>
            {
                entity.HasKey(e => e.MallId);

                entity.Property(e => e.MallName).IsRequired();
                entity.Property(e => e.MallDescription);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.HasOne(a => a.StoreIdNavigation)
                    .WithMany(b => b.Product)
                    .HasForeignKey(c => c.StoreId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.ProductName).IsRequired();

                entity.Property(e => e.ProductDescription);
            });

            modelBuilder.Entity<Product_category>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.HasKey(e => e.CategoryId);

                entity.HasOne(a => a.CategoryIdNavigation)
                    .WithMany(b => b.Product_Category)
                    .HasForeignKey(c => c.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(a => a.ProductIdNavigation)
                    .WithMany(b => b.Product_Category)
                    .HasForeignKey(c => c.ProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Room>(entity => 
            {
                entity.HasKey(e => e.RoomId);

                entity.HasOne(a => a.MallIdNavigation)
                    .WithMany(b => b.Room)
                    .HasForeignKey(c => c.MallId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.Rent).IsRequired();
                entity.Property(e => e.IsAvailable).IsRequired();
            });

            modelBuilder.Entity<Store>(entity =>
            {
                entity.HasKey(e => e.StoreId);

                entity.HasOne(a => a.RoomIdNavigation)
                    .WithMany(b => b.Store)
                    .HasForeignKey(c => c.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.StoreName).IsRequired().HasMaxLength(50);

                entity.Property(e => e.RentDebt);
                entity.Property(e => e.StoreDescription);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.HasOne(a => a.StoreIdNavigation)
                    .WithMany(b => b.User)
                    .HasForeignKey(c => c.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(a => a.MallNavigation)
                    .WithMany(b => b.User)
                    .HasForeignKey(c => c.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.ConfirmedByAdmin).IsRequired();
                entity.Property(e => e.IsAdmin).IsRequired();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            modelBuilder.Entity<WorkingTime>(entity => 
            {
                entity.HasKey(e => e.WorkingTimeId);

                entity.HasOne(a => a.StoreIdNavigation)
                    .WithMany(b => b.WorkingTime)
                    .HasForeignKey(c => c.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(a => a.MallNavigation)
                    .WithMany(b => b.WorkingTime)
                    .HasForeignKey(c => c.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.Property(e => e.DayOfTheWeek);
                entity.Property(e => e.StartingHour).IsRequired();
                entity.Property(e => e.FinishingHour).IsRequired();
                entity.Property(e => e.MallWorkingTime).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
