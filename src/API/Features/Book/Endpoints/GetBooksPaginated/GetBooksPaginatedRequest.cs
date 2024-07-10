namespace API.Features.Book.Endpoints.GetBooksPaginated
{
    public class GetBooksPaginatedRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
