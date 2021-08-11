namespace MediService.ASP.NET_Core.Services.Specialists
{
    public interface ISpecialistService
    {
        public bool IsSpecialist(string userId);

        public string IdByUser(string userId);

        public string GetIdFromService(int serviceId);
    }
}
