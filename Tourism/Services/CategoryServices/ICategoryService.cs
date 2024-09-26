using Tourism.Model;

namespace Tourism.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync(); // Get all categories
        Task<Category> GetCategoryByIdAsync(Guid id); // Get a specific category by its ID
        Task<Category> CreateCategoryAsync(Category category); // Add a new category
        Task<bool> UpdateCategoryAsync(Guid id, Category category); // Update an existing category
        Task<bool> DeleteCategoryAsync(Guid id); // Delete a category by its ID
    }

}