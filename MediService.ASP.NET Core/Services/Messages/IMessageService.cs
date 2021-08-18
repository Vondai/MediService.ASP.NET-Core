using System.Threading.Tasks;

namespace MediService.ASP.NET_Core.Services.Messages
{
    public interface IMessageService
    {
        int GetCountNew(string userId);

        Task<string> Send(string recipientId, string sender, string title, string content);
    }
}
