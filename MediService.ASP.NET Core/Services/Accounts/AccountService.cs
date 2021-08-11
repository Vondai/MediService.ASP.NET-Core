using System.Linq;
using MediService.ASP.NET_Core.Data;

namespace MediService.ASP.NET_Core.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly MediServiceDbContext data;

        public AccountService(MediServiceDbContext data)
            => this.data = data;

        public string GetAddress(string userId)
            => this.data.
            Addresses
            .Where(x => x.UserId == userId)
            .Select(x => x.FullAddress)
            .FirstOrDefault();
    }
}
