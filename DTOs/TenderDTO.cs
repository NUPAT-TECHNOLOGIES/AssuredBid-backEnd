namespace AssuredBid.DTOs
{
    public class TenderDTO
    {
        public string TenderId { get; set; }
        public string TenderTitle { get; set; }
        public string Category { get; set; }
        public decimal? Budget { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateTime DeadLine { get; set; }
        public string Status { get; set; }
        public string ClassificationScheme { get; set; }
        public string ClassificationId { get; set; }


    }
}
