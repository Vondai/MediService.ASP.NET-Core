using System;
using System.ComponentModel.DataAnnotations;

using static MediService.ASP.NET_Core.Data.DataConstraints;

namespace MediService.ASP.NET_Core.Data.Models
{
    public class Message
    {
        public Message()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        [MaxLength(MessageTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(MessageContentMaxLength)]
        public string Content { get; set; }

        public DateTime Sent { get; set; }

        public bool IsSeen { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public User Recipient { get; set; }
    }
}
