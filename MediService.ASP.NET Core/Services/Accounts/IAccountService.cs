using MediService.ASP.NET_Core.Data.Models;

namespace MediService.ASP.NET_Core.Services.Accounts
{
    public interface IAccountService
    {
        string GetIdByUsername(string username);

        string GetAddress(string userId);

        User CreateUser(
            string username,
            string email,
            string fullName,
            string phoneNumber,
            string city,
            string address);
    }
}
