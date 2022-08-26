namespace ServerSidePagination.Models.ViewModel
{
    public class SearchVM
    {
        public int? Id { get; set; }
        public List<string>? Status { get; set; }
        public decimal? Total { get; set; }
        public DateTime? CreatedOnUtc { get; set; }
    }
}
