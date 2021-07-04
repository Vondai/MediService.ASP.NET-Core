namespace MediService.ASP.NET_Core.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);
    }
}
