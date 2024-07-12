namespace API.Features.Client.Endpoints.CreateClient
{
    public class CreateClientResponse
    {
        public Guid ClientId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
