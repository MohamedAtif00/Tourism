using Microsoft.EntityFrameworkCore;
using Tourism.Data;
using Tourism.Model;

namespace Tourism.Services.CategoryServices
{
    public class CategoryServices : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _context.categories.FindAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(Category Category)
        {
            _context.categories.Add(Category);
            await _context.SaveChangesAsync();
            return Category;
        }

        public async Task<bool> UpdateCategoryAsync(Guid id, Category Category)
        {
            var existingCategory = await _context.categories.FindAsync(id);
            if (existingCategory == null) return false;

            existingCategory.Name = Category.Name;
            existingCategory.Description = Category.Description;
            // Add any other fields to update

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var Category = await _context.categories.FindAsync(id);
            if (Category == null) return false;

            _context.categories.Remove(Category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
