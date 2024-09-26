namespace Tourism.Model
{
    public class Review
    {
        public Guid Id { get; set; } // Unique identifier for the review

        public string ReviewerName { get; set; } // Name of the reviewer

        public string Comment { get; set; } // Review comment

        public double Rating { get; set; } // Rating given by the reviewer

        public DateTime DateReviewed { get; set; } // Date of review

        // Foreign key to ApplicationUser
        public Guid TourismPlaceId { get; set; } // Foreign key to TourismPlace
        public Guid ApplicationUserId { get; set; }
    }
}
