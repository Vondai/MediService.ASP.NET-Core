using System;
using System.Linq;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;

namespace MediService.ASP.NET_Core.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly MediServiceDbContext data;

        public MessageService(MediServiceDbContext data)
            => this.data = data;

        public int GetCountNew(string userId)
            => this.data
                .Messages
                .Where
                    (m => m.RecipientId == userId
                    && m.IsSeen == false)
                .Count();

        public async Task<string> Send(string recipientId, string sender, string title, string content)
        {
            var message = new Message()
            {
                Title = title,
                Content = content,
                Sender = sender,
                RecipientId = recipientId,
                Sent = DateTime.UtcNow,
            };
            await this.data.Messages.AddAsync(message);
            await this.data.SaveChangesAsync();

            return message.Id;
        }
    }
}
