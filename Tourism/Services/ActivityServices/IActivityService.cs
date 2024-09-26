using Tourism.Controllers;
using Tourism.Model;

namespace Tourism.Services.ActivityServices
{
    public interface IActivityService
    {
        Task<IEnumerable<Activity>> GetAllActivitiesAsync(); // Get all activities
        Task<Activity> GetActivityByIdAsync(Guid id); // Get a specific activity by its ID
        Task<Activity> CreateActivityAsync(CreateActiviryDto activity); // Add a new activity
        Task<bool> UpdateActivityAsync(Guid id, Activity activity); // Update an existing activity
        Task<bool> DeleteActivityAsync(Guid id); // Delete an activity by its ID
    }

}