using Microsoft.EntityFrameworkCore;
using System.Linq;
using Tourism.Data;
using Tourism.Model;

namespace Tourism.Services.TourismPlaceServices
{
    public class TourismPlaceService : ITourismPlaceService
    {
        private readonly ApplicationDbContext _context;

        public TourismPlaceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourismPlaceDto>> GetAllTourismPlacesAsync()
        {
            // Fetch the tourism places and include their activities
            var tourismPlaces = await _context.tourismPlaces
                .Include(x => x.Activities)
                .ToListAsync();

            // Map the tourism places to DTOs
            var tourismPlaceDtos = tourismPlaces.Select(tp => new TourismPlaceDto(
                tp.Id,
                tp.Name,
                tp.Description,
                tp.City,
                tp.Location,
                tp.EntryFee,
                tp.Rating,
                tp.Images, // Assuming it's already a List<string>
                tp.OpeningHours, // Assuming this is DateTime
                tp.ClosingHours, // Assuming this is DateTime
                tp.IsPopular,
                tp.CategoryId,
                tp.Activities.Select(a => new ActivityDto(
                    a.Id,
                    a.Name,
                    a.Description
                )).ToList() // Map the activities to ActivityDto
            )).ToList();

            return tourismPlaceDtos;
        }

        public async Task<List<TourismPlaceDto>> GetTourismPlacesByCategoryId(Guid categoryId)
        {
            // Fetch the tourism places and include their activities
            var tourismPlaces = await _context.tourismPlaces.Where(x => x.CategoryId == categoryId)
                .Include(x => x.Activities)
                .ToListAsync();

            // Map the tourism places to DTOs
            var tourismPlaceDtos = tourismPlaces.Select(tp => new TourismPlaceDto(
                tp.Id,
                tp.Name,
                tp.Description,
                tp.City,
                tp.Location,
                tp.EntryFee,
                tp.Rating,
                tp.Images, // Assuming it's already a List<string>
                tp.OpeningHours, // Assuming this is DateTime
                tp.ClosingHours, // Assuming this is DateTime
                tp.IsPopular,
                tp.CategoryId,
                tp.Activities.Select(a => new ActivityDto(
                    a.Id,
                    a.Name,
                    a.Description
                )).ToList() // Map the activities to ActivityDto
            )).ToList();

            return tourismPlaceDtos;
        }



        public async Task<GetSingleTourismPlace> GetTourismPlaceByIdAsync(Guid id)
        {
            var tourismPlace = await _context.tourismPlaces
                .Include(tp => tp.Activities)
                .Include(tp=> tp.Reviews)
                .FirstOrDefaultAsync(tp => tp.Id == id);

            if (tourismPlace == null)
            {
                // Handle the case where the tourism place is not found
                throw new KeyNotFoundException("Tourism place not found.");
            }

            var tourismPlaceDto = new GetSingleTourismPlace(
                tourismPlace.Id,
                tourismPlace.Name,
                tourismPlace.Description,
                tourismPlace.City,
                tourismPlace.Location,
                tourismPlace.EntryFee,
                tourismPlace.Rating,
                tourismPlace.Images, // Assuming this is already a List<string>
                tourismPlace.OpeningHours,
                tourismPlace.ClosingHours,
                tourismPlace.IsPopular,
                tourismPlace.CategoryId,
                tourismPlace.Activities.Select(a => new ActivityDto(
                    a.Id,
                    a.Name,
                    a.Description
                )).ToList(),
                tourismPlace.Reviews.Select(r => new ReviewDto(
                    r.Id,
                    r.ReviewerName,
                    r.Comment
                    )).ToList()

            );

            return tourismPlaceDto;
        }


        public async Task<TourismPlace> CreateTourismPlaceAsync(TourismPlace tourismPlace)
        {
            _context.tourismPlaces.Add(tourismPlace);
            await _context.SaveChangesAsync();
            return tourismPlace;
        }

        public async Task<bool> UpdateTourismPlaceAsync(Guid id, TourismPlace tourismPlace)
        {
            var existingTourismPlace = await _context.tourismPlaces.FindAsync(id);
            if (existingTourismPlace == null) return false;

            existingTourismPlace.Name = tourismPlace.Name;
            existingTourismPlace.Description = tourismPlace.Description;
            existingTourismPlace.Location = tourismPlace.Location;
            existingTourismPlace.EntryFee = tourismPlace.EntryFee;
            existingTourismPlace.Rating = tourismPlace.Rating;
            // Add any other fields to update

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteTourismPlaceAsync(Guid id)
        {
            var tourismPlace = await _context.tourismPlaces.FindAsync(id);
            if (tourismPlace == null) return false;

            _context.tourismPlaces.Remove(tourismPlace);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public record TourismPlaceDto(Guid id,
                                string name,
                                string description,
                                string city,
                                string location,
                                decimal entryFee,
                                double rating,
                                List<string> images,
                                DateTime openingTime,
                                DateTime closingTime,
                                bool isPopular,
                                Guid categoryId,
                                List<ActivityDto> activities);

    public record GetSingleTourismPlace(Guid id,
                                string name,
                                string description,
                                string city,
                                string location,
                                decimal entryFee,
                                double rating,
                                List<string> images,
                                DateTime openingTime,
                                DateTime closingTime,
                                bool isPopular,
                                Guid categoryId,
                                List<ActivityDto> activities,
                                List<ReviewDto> Reviews
        );
    public record ActivityDto(Guid Id, string Name, string Description);
    public record ReviewDto(Guid id,string reviewerName,string comment);

}
