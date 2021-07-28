namespace MediService.ASP.NET_Core.Data.Models
{
    public class Address
    {
        public int Id { get; init; }

        public string FullAddress { get; set; }

        public string City { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}
