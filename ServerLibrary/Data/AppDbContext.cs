using BaseEntity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Flight> Flights { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Flight>()
            .Property(f => f.BasePrice)
            .HasPrecision(10, 2);

            modelBuilder.Entity<Flight>()
              .Property(f => f.DepartureDate)
              .HasColumnType("date")
              .IsRequired();

            modelBuilder.Entity<Flight>()
                .Property(f => f.ArrivalDate)
                .HasColumnType("date")
                .IsRequired();
        }


    }
}
