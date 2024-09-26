using Tourism.Controllers;
using Tourism.Model;

namespace Tourism.Services.ReviewServices
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetAllReviewsAsync(); // Get all reviews
        Task<IEnumerable<Review>> GetReviewsByTourismPlaceIdAsync(Guid tourismPlaceId); // Get reviews for a specific tourism place
        Task<Review> GetReviewByIdAsync(Guid id); // Get a specific review by its ID
        Task<Review> CreateReviewAsync(CreateReviewDto review); // Add a new review
        Task<bool> DeleteReviewAsync(Guid id); // Delete a review by its ID
    }

}