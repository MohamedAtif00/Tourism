using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tourism.Controllers.Contracts.TourismPlaces;
using Tourism.Model;
using Tourism.Services.TourismPlaceServices;
using static System.Net.Mime.MediaTypeNames;

namespace Tourism.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TourismPlaceController : ControllerBase
    {
        private readonly ITourismPlaceService _tourismPlaceService;
        private readonly ImageHelper _imageHelper;

        public TourismPlaceController(ITourismPlaceService tourismPlaceService, ImageHelper imageHelper)
        {
            _tourismPlaceService = tourismPlaceService;
            _imageHelper = imageHelper;
        }

        // GET: api/tourismplace
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            { 
                var tourismPlaces = await _tourismPlaceService.GetAllTourismPlacesAsync();
                return Ok(tourismPlaces);
            }

            var tourismPlacesForCategory = await _tourismPlaceService.GetTourismPlacesByCategoryId(categoryId);

            return Ok(tourismPlacesForCategory);

        }

        // GET: api/tourismplace/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var tourismPlace = await _tourismPlaceService.GetTourismPlaceByIdAsync(id);
            if (tourismPlace == null)
            {
                return NotFound();
            }
            return Ok(tourismPlace);
        }

        // POST: api/tourismplace
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] TourismPlaceCreateDto model, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tourismPlace = new TourismPlace
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                City = model.City,
                Location = model.Location,
                EntryFee = model.EntryFee,
                Rating = model.Rating,
                OpeningHours = model.OpeningHours,
                ClosingHours = model.ClosingHours,
                IsPopular = model.IsPopular,
                CategoryId = model.CategoryId,
                Images = new List<string>()
            };

            // Handle image uploads
            if (images != null && images.Count > 0)
            {
                foreach (var image in images)
                {
                    // Save the image using the ImageHelper class
                    string imagePath = await _imageHelper.SaveImageAsync(image, "tourism-places");
                    tourismPlace.Images.Add(imagePath); // Add the image path to the list
                }
            }

            // Save the tourism place to the database (via your service layer)
            await _tourismPlaceService.CreateTourismPlaceAsync(tourismPlace);

            return Ok(new { Message = "Tourism place added successfully", PlaceId = tourismPlace.Id });
        }

        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return BadRequest("Please upload an image.");
            }

            try
            {
                // Save the image and get the relative path
                string imagePath = await _imageHelper.SaveImageAsync(imageFile, "tourism-places");

                // Here you can store imagePath in the database if needed
                return Ok(new { imagePath });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-image")]
        public IActionResult GetImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
            {
                return BadRequest("Image path must be provided.");
            }

            // Get the full path to the image
            string fullPath = _imageHelper.GetImagePath(imagePath);

            if (fullPath == null)
            {
                return NotFound("Image not found.");
            }

            // Return the image file to the client
            var image = System.IO.File.OpenRead(fullPath);
            return File(image, "image/jpeg"); // You can adjust the MIME type as needed
        }

        // PUT: api/tourismplace/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] TourismPlace tourismPlace)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _tourismPlaceService.UpdateTourismPlaceAsync(id, tourismPlace);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/tourismplace/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _tourismPlaceService.DeleteTourismPlaceAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }

}
