namespace MediService.ASP.NET_Core.Models.Subscriptions
{
    public class SubscriptionFormModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public int AppointmentCount { get; init; }

        public string Price { get; init; }
    }
}
