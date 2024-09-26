using System.ComponentModel.DataAnnotations;

namespace Tourism.Model
{
    public class TourismPlace
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Location { get; set; } // Location or address of the place

        public decimal EntryFee { get; set; } // Entry fee (if applicable)

        [Range(0,5,ErrorMessage ="Rate should be between 0 and 5")]
        public double Rating { get; set; } // Average rating from visitors

        public List<string> Images { get; set; } // List of image URLs

        public DateTime OpeningHours { get; set; } // Opening time of the place

        public DateTime ClosingHours { get; set; } // Closing time of the place


        public bool IsPopular { get; set; } // If the place is marked as a popular tourist destination
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }
        public ICollection<Activity> Activities { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
