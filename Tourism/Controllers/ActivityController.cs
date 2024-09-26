using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism.Model;
using Tourism.Services.ActivityServices;

namespace Tourism.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }

        // GET: api/activity
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var activities = await _activityService.GetAllActivitiesAsync();
            return Ok(activities);
        }

        // GET: api/activity/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var activity = await _activityService.GetActivityByIdAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            return Ok(activity);
        }

        // POST: api/activity
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateActiviryDto activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdActivity = await _activityService.CreateActivityAsync(activity);
            return CreatedAtAction(nameof(GetById), new { id = createdActivity.Id }, createdActivity);
        }

        // PUT: api/activity/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Activity activity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _activityService.UpdateActivityAsync(id, activity);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/activity/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _activityService.DeleteActivityAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

    public record CreateActiviryDto(string name ,string description,Guid TourismPlaceId);

}
