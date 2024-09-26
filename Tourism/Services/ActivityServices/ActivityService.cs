using Microsoft.EntityFrameworkCore;
using Tourism.Controllers;
using Tourism.Data;
using Tourism.Model;

namespace Tourism.Services.ActivityServices
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;

        public ActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Fetch all activities
        public async Task<IEnumerable<Activity>> GetAllActivitiesAsync()
        {
            return await _context.activities.ToListAsync();
        }

        // Fetch a specific activity by its ID
        public async Task<Activity> GetActivityByIdAsync(Guid id)
        {
            return await _context.activities.FindAsync(id);
        }

        // Add a new activity
        public async Task<Activity> CreateActivityAsync(CreateActiviryDto activity)
        {
            Activity newActivity = new() { 
                Id= Guid.NewGuid(),
                Name=activity.name,
                Description=activity.description,
                TourismPlaceId = activity.TourismPlaceId
            };
            _context.activities.Add(newActivity);
            await _context.SaveChangesAsync();
            return newActivity;
        }

        // Update an existing activity
        public async Task<bool> UpdateActivityAsync(Guid id, Activity activity)
        {
            var existingActivity = await _context.activities.FindAsync(id);
            if (existingActivity == null) return false;

            existingActivity.Name = activity.Name;
            existingActivity.Description = activity.Description;

            await _context.SaveChangesAsync();
            return true;
        }

        // Delete an activity by its ID
        public async Task<bool> DeleteActivityAsync(Guid id)
        {
            var activity = await _context.activities.FindAsync(id);
            if (activity == null) return false;

            _context.activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
