namespace Tourism.Model
{
    public class Activity
    {
        public Guid Id { get; set; } // Unique identifier for the activity

        public string Name { get; set; } // Name of the activity

        public string Description { get; set; } // Brief description of the activity
        public TourismPlace TourismPlace { get; set; }
        public Guid TourismPlaceId { get; set; }
    }
}
