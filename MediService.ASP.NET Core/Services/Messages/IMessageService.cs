using System.Collections.Generic;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Models.Messages;

namespace MediService.ASP.NET_Core.Services.Messages
{
    public interface IMessageService
    {
        int GetCountNew(string userId);

        MessageDetailsViewModel GetById(string id);

        Task<bool> DeleteById(string id);

        Task<bool> MarkAsSeen(string id);

        Task<string> Send(string recipientId, string sender, string title, string content);

        ICollection<MessageListingViewModel> GetListing(string userId);
    }
}
