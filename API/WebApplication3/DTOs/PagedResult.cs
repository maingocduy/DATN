namespace WebApplication3.DTOs
{
    public class PagedResult<T>
    {
        public List<T> Projects { get; set; }
        public int TotalPages { get; set; }
    }
}
