using System.Linq;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;

namespace MediService.ASP.NET_Core.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly MediServiceDbContext data;

        public AccountService(MediServiceDbContext data)
            => this.data = data;

        public string GetIdByUsername(string username)
            => this.data.Users
                .Where(u => u.UserName == username)
                .Select(x => x.Id)
                .FirstOrDefault();

        public User CreateUser(
            string username, 
            string email, 
            string fullName, 
            string phoneNumber, 
            string city, 
            string address)
        {
            var useraddress = new Address()
            {
                City = city,
                FullAddress = address,
            };
            var user = new User()
            {
                UserName = username,
                Email = email,
                FullName = fullName,
                PhoneNumber = phoneNumber,
            };
            user.Addresses.Add(useraddress);
            return user;
        }

        public string GetAddress(string userId)
            => this.data.
            Addresses
            .Where(x => x.UserId == userId)
            .Select(x => x.FullAddress)
            .FirstOrDefault();
    }
}
