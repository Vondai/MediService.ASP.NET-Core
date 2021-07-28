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

        public string Content { get; set; }

        public DateTime Sent { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; init; }

        public string Recipient { get; set; }
    }
}
