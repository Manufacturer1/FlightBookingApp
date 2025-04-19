using BaseEntity.Entities;
using BaseEntity.Responses;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Data;
using ServerLibrary.Repositories.Interfaces;
using System.Data.Common;

namespace ServerLibrary.Repositories.Implementations
{
    public class FlightAmenityRepository : IFlightAmenityRepository
    {
        private AppDbContext db;

        public FlightAmenityRepository(AppDbContext db)
        {
            this.db = db;
        }

        public async Task<GeneralReponse> CreateAsync(FlightAmenity amenity)
        {
            if (amenity == null) return new GeneralReponse(false, "Amenity was not found");
            try
            {
                var getExistingAmenity = await db.FlightAmenities.FirstOrDefaultAsync(x => x.FlightNumber == amenity.FlightNumber && x.AmenityId == amenity.AmenityId);
                if(getExistingAmenity == null)
                {
                    await db.FlightAmenities.AddAsync(amenity);

                }
                else
                {
                    return new GeneralReponse(false, "This flight number already was associated with this amenity");
                }
            }
            catch (DbException dbEx)
            {
                return new GeneralReponse(false, $"Database error: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return new GeneralReponse(false, $"Something went wrong {ex.Message}");
            }
            return new GeneralReponse(true, $"Amenity {amenity.Id} was added successfully");
        }

        public async Task<GeneralReponse> UpdateAsync(FlightAmenity amenity)
        {
            var flightAmenity = await db.FlightAmenities.FirstOrDefaultAsync(x => x.AmenityId == amenity.AmenityId);
            if (flightAmenity == null) return new GeneralReponse(false, "Flight amenity was not found");

   
            flightAmenity.AmenityId = amenity.AmenityId;
            flightAmenity.FlightNumber = amenity.FlightNumber;

            return new GeneralReponse(true, $"Flight amenity updated successfully");

        }
        public async Task SaveChangesAsync()
        {

            await db.SaveChangesAsync();

        }

    }
}
