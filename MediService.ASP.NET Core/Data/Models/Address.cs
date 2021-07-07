namespace MediService.ASP.NET_Core.Data.Models
{
    public class Address
    {
        public int Id { get; init; }

        public string FullAddress { get; init; }

        public string City { get; init; }

        public string UserId { get; init; }

        public User User { get; init; }
    }
}
