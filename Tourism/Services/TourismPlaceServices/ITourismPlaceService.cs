using Tourism.Model;

namespace Tourism.Services.TourismPlaceServices
{
    public interface ITourismPlaceService
    {
        Task<IEnumerable<TourismPlaceDto>> GetAllTourismPlacesAsync(); // Get all tourism places
        Task<GetSingleTourismPlace> GetTourismPlaceByIdAsync(Guid id); // Get a specific tourism place by its ID
        Task<TourismPlace> CreateTourismPlaceAsync(TourismPlace tourismPlace); // Add a new tourism place
        Task<bool> UpdateTourismPlaceAsync(Guid id, TourismPlace tourismPlace); // Update an existing tourism place
        Task<bool> DeleteTourismPlaceAsync(Guid id); // Delete a tourism place by its ID
        Task<List<TourismPlaceDto>> GetTourismPlacesByCategoryId(Guid categoryId);
    }

}