namespace Tourism.Controllers.Contracts.TourismPlaces
{
    public class TourismPlaceCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public decimal EntryFee { get; set; }
        public double Rating { get; set; }
        public DateTime OpeningHours { get; set; }
        public DateTime ClosingHours { get; set; }
        public List<string> Activities { get; set; }
        public bool IsPopular { get; set; }
        public Guid CategoryId { get; set; }
    }

}
