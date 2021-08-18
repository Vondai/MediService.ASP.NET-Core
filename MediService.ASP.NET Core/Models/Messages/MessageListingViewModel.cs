namespace MediService.ASP.NET_Core.Models.Messages
{
    public class MessageListingViewModel
    {
        public string Id { get; init; }

        public string Title { get; init; }

        public string Sender { get; init; }

        public bool IsSeen { get; init; }

        public string Sent { get; init; }
    }
}
