using System;
using System.ComponentModel.DataAnnotations;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class Message
    {
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; init; }

        public string Content { get; init; }

        public DateTime Sent { get; init; }

        [Required]
        public string UserId { get; init; }

        public User User { get; init; }

        public string Recipient { get; init; }
    }
}
