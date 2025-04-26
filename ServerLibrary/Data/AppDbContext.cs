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
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<FlightAmenity> FlightAmenities { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        public DbSet<PassportIdentity> PassportIdentities { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

    

            modelBuilder.Entity<Flight>()
              .Property(f => f.DepartureDate)
              .HasColumnType("date")
              .IsRequired();

            modelBuilder.Entity<Flight>()
                .Property(f => f.ArrivalDate)
                .HasColumnType("date")
                .IsRequired();

            modelBuilder.Entity<Flight>()
                  .Property(f => f.BasePrice)
                  .HasPrecision(10, 2);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Plane)
                .WithMany(P => P.Flights)
                .HasForeignKey(f => f.PlaneId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.OriginAirport)
                .WithMany(a => a.DepartingFlights)
                .HasForeignKey(f => f.OriginAirportId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DestinationAirport)
                .WithMany(a => a.ArrivalFlights)
                .HasForeignKey(f => f.DestinationAirportId)
                .OnDelete(DeleteBehavior.NoAction);

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


            modelBuilder.Entity<FlightAmenity>()
                .HasOne(fa => fa.Flight)
                .WithMany(f => f.FlightAmenities)
                .HasForeignKey(fa => fa.FlightNumber);


            modelBuilder.Entity<FlightAmenity>()
                .HasOne(fa => fa.Amenity)
                .WithMany(a => a.FlightAmenities)
                .HasForeignKey(fa => fa.AmenityId);


            modelBuilder.Entity<Discount>(entity =>
            {
                entity.Property(p => p.Percentage)
                  .HasPrecision(5, 2);
            });

            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.PassportIdentity)
                .WithOne()
                .HasForeignKey<Passenger>(p => p.PassportIdentityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Passenger>()
                .HasOne(p => p.ContactDetails)
                .WithOne()
                .HasForeignKey<Passenger>(p => p.ContactDetailsId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(p => p.Passenger)
                .WithMany(b => b.Bookings)
                .HasForeignKey(b => b.PassengerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(i => i.Itinerary)
                .WithMany(b => b.Bookings)
                .HasForeignKey(b => b.ItineraryId)
                .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<Ticket>()
                 .HasOne(t => t.Booking)
                 .WithMany(b => b.Tickets)
                 .HasForeignKey(t => t.BookingId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(f => f.Flight)
                .WithMany(t => t.Tickets)
                .HasForeignKey(t => t.FlightNumber)
                .OnDelete(DeleteBehavior.Cascade);
        }



    }
}
