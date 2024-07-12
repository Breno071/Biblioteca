namespace API.Shared.Extensions
{
    public static class HttpContextExtensions
    {
        public static Uri CreatedUri(this HttpContext httpContext, Guid? id = null)
        {
            return new Uri($"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{(id is null ? "" : $"/{id}")}");
        }
    }
}
