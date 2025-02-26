namespace AssuredBid.DTOs
{
    public class CreateTenderDTO
    {
        public string TenderId { get; set; }
        public string TenderTitle { get; set; }
        public string Category { get; set; }
        public decimal Budget { get; set; }
        public string Type { get; set; }
        public DateTime Deadline { get; set; }
        public string Status { get; set; }
    }
}
