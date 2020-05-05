namespace Backend.Models.DTO
{
    public abstract class FiltroBase
    {
        public string SearchString { get; set; }
        public string SortOrder { get; set; }
        public int? PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}