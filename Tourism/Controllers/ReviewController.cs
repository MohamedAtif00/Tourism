using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Model;
using Tourism.Services.ReviewServices;
using Tourism.Services.TourismPlaceServices;

namespace Tourism.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        private readonly ITourismPlaceService _tourismPlaceService;
        public ReviewController(IReviewService reviewService, ITourismPlaceService tourismPlaceService)
        {
            _reviewService = reviewService;
            _tourismPlaceService = tourismPlaceService;
        }

        // GET: api/review
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewService.GetAllReviewsAsync();
            return Ok(reviews);
        }

        // GET: api/review/tourismPlace/1
        [HttpGet("tourismPlace/{tourismPlaceId}")]
        public async Task<IActionResult> GetByTourismPlace(Guid tourismPlaceId)
        {
            var reviews = await _reviewService.GetReviewsByTourismPlaceIdAsync(tourismPlaceId);
            return Ok(reviews);
        }

        // GET: api/review/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            return Ok(review);
        }

        // POST: api/review
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto review)
        {


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Call your service to create the review, passing the userId from token
            var createdReview = await _reviewService.CreateReviewAsync(review);

            // Return Created response with the newly created review
            return CreatedAtAction(nameof(GetById), new { id = createdReview.Id }, createdReview);
        }


        // DELETE: api/review/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _reviewService.DeleteReviewAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

    
    public record CreateReviewDto(string ReviewerName,string Comment,double Rating,DateTime? DateReviewed,Guid TourismPlaceId,Guid userId);

}
