namespace MediService.ASP.NET_Core.Models.Specialists
{
    public class SpecialistViewModel
    {
        public string FullName { get; init; }

        public string Description { get; init; }

        public string ImageUrl { get; init; }

        public string[] Services { get; init; }
    }
}
