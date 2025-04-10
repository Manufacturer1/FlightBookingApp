using BaseEntity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Plane> Planes { get; set; }
        public DbSet<BaggagePolicy> Baggages { get; set; }


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


            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Airline)
                .WithMany(a => a.Flights)
                .HasForeignKey(f => f.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Plane)
                .WithMany(P => P.Flights)
                .HasForeignKey(f => f.PlaneId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Airline>()
                .HasOne(a => a.BaggagePolicy)
                .WithOne(b => b.Airline)
                .HasForeignKey<Airline>(a => a.BaggagePolicyId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<BaggagePolicy>(entity =>
            {
                entity.Property(p => p.CheckedWeightLimitKg)
                   .HasPrecision(10, 2)
                   .IsRequired();

                entity.Property(p => p.ExtraCheckedBagFee)
                        .HasPrecision(10, 2)
                        .IsRequired();

                entity.Property(p => p.OverWeightFeePerKg)
                        .HasPrecision(10, 2)
                        .IsRequired();

                entity.Property(p => p.CabinWeightLimitKg)
                .HasPrecision(10, 2)
                .IsRequired();

                entity.Property(p => p.ExtraCabinBagFee)
                .HasPrecision(10, 2)
                .IsRequired();
                

            });

        }


    }
}
