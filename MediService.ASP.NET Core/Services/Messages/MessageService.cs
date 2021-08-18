using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediService.ASP.NET_Core.Data;
using MediService.ASP.NET_Core.Data.Models;
using MediService.ASP.NET_Core.Models.Messages;

namespace MediService.ASP.NET_Core.Services.Messages
{
    public class MessageService : IMessageService
    {
        private readonly MediServiceDbContext data;

        public MessageService(MediServiceDbContext data)
            => this.data = data;

        public MessageDetailsViewModel GetById(string id)
            => this.data
                .Messages
                .Where(m => m.Id == id)
                .Select(x => new MessageDetailsViewModel()
                {
                    Id = x.Id,
                    Content = x.Content,
                    Sent = x.Sent.ToLocalTime().ToString("MM-dd-yyyy HH:MM"),
                    Title = x.Title,
                    Sender = x.Sender
                })
                .FirstOrDefault();

        public async Task<bool> DeleteById(string id)
        {
            var message = this.data
                    .Messages
                    .Where(m => m.Id == id)
                    .FirstOrDefault();
            if (message == null)
            {
                return false;
            }
            this.data.Messages.Remove(message);
            await this.data.SaveChangesAsync();

            return true;
        }

        public int GetCountNew(string userId)
            => this.data
                .Messages
                .Where
                    (m => m.RecipientId == userId
                    && m.IsSeen == false)
                .Count();

        public async Task<bool> MarkAsSeen(string id)
        {
            var message = this.data
                    .Messages
                    .Where(m => m.Id == id)
                    .FirstOrDefault();
            if (message == null)
            {
                return false;
            }
            if (message.IsSeen != true)
            {
                message.IsSeen = true;
                await this.data.SaveChangesAsync();
            }

            return true;
        }

        public ICollection<MessageListingViewModel> GetListing(string userId)
            => this.data
                .Messages
                .Where(m => m.RecipientId == userId)
                .OrderByDescending(m => m.Sent)
                .Select(x => new MessageListingViewModel()
                {
                    Id = x.Id,
                    IsSeen = x.IsSeen,
                    Sender = x.Sender,
                    Title = x.Title,
                    Sent = x.Sent.ToLocalTime().ToString("MM-dd-yyyy HH:mm")
                })
                .ToList();

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
