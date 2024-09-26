using Microsoft.EntityFrameworkCore;
using Tourism.Controllers;
using Tourism.Data;
using Tourism.Model;

namespace Tourism.Services.ReviewServices
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Fetch all reviews
        public async Task<IEnumerable<Review>> GetAllReviewsAsync()
        {
            return await _context.reviews.ToListAsync();
        }

        // Fetch reviews for a specific tourism place
        public async Task<IEnumerable<Review>> GetReviewsByTourismPlaceIdAsync(Guid tourismPlaceId)
        {
            return await _context.reviews
                .Where(r => r.TourismPlaceId == tourismPlaceId)
                .ToListAsync();
        }
            
        // Fetch a review by its ID
        public async Task<Review> GetReviewByIdAsync(Guid id)
        {
            return await _context.reviews.FindAsync(id);
        }

        // Add a new review
        public async Task<Review> CreateReviewAsync(CreateReviewDto reviewDto)
        {
            // Map CreateReviewDto to Review entity
            var review = new Review
            {
                Id = reviewDto.userId,
                ReviewerName = reviewDto.ReviewerName,
                Comment = reviewDto.Comment,
                Rating = reviewDto.Rating,
                DateReviewed = DateTime.Now,
                TourismPlaceId = reviewDto.TourismPlaceId
            };

            // Add the new review to the context
            _context.reviews.Add(review);

            // Find the associated TourismPlace
            var tourismPlace = await _context.tourismPlaces
                .Where(tp => tp.Id == reviewDto.TourismPlaceId)
                .FirstOrDefaultAsync();

            if (tourismPlace == null)
            {
                throw new Exception("TourismPlace not found.");
            }

            // Get all the reviews for the TourismPlace including the new one
            var reviews = await _context.reviews
                .Where(r => r.TourismPlaceId == tourismPlace.Id)
                .ToListAsync();

            // Add the new review to the list for calculating average
            reviews.Add(review);

            // Calculate the new average rating
            if (reviews.Count == 0)
            {
                tourismPlace.Rating = 0; // Default rating if no reviews (just a fallback)
            }
            else
            {
                tourismPlace.Rating = reviews.Average(r => r.Rating);
                tourismPlace.Rating = Math.Clamp(tourismPlace.Rating, 0, 5); // Ensure rating is between 0 and 5
            }

            // Update the TourismPlace in the context
            _context.tourismPlaces.Update(tourismPlace);

            // Save all changes to the database
            await _context.SaveChangesAsync();

            return review;
        }



        // Delete a review by its ID
        public async Task<bool> DeleteReviewAsync(Guid id)
        {
            var review = await _context.reviews.FindAsync(id);
            if (review == null)
            {
                return false; // Return false if review is not found
            }

            _context.reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    

}
