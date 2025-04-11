using BaseEntity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ServerLibrary.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Flight> Flights { get; set; }
        public DbSet<Itinerary> Itineraries { get; set; }
        public DbSet<FlightSegment> Segments { get; set; }
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
                .HasOne(f => f.Plane)
                .WithMany(P => P.Flights)
                .HasForeignKey(f => f.PlaneId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Airline>()
                .HasOne(a => a.BaggagePolicy)
                .WithMany(bp => bp.Airlines)
                .HasForeignKey(a => a.BaggagePolicyId)
                .OnDelete(DeleteBehavior.SetNull);



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

            modelBuilder.Entity<FlightSegment>()
                .HasOne(fs => fs.Flight)
                .WithMany(f => f.Segments)
                .HasForeignKey(fs => fs.FlightNumber)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FlightSegment>()
                .HasOne(fs => fs.Itinerary)
                .WithMany(i => i.Segments)
                .HasForeignKey(fs => fs.ItineraryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Itinerary>()
                .HasOne(a => a.Airline)
                .WithMany(i => i.Itineraries)
                .HasForeignKey(i => i.AirlineId)
                .OnDelete(DeleteBehavior.Cascade);

        }


    }
}
