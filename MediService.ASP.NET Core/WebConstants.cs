namespace MediService.ASP.NET_Core
{
    public class WebConstants
    {
        public class GlobalMessage
        {
            public const string ErrorKey = "Error";
            public const string SuccessKey = "Success";
        }

        public class Cache
        {
            public const string RecentReviewsKey = nameof(RecentReviewsKey);
            public const string AllSpecialistsKey = nameof(AllSpecialistsKey);
            public const string AllServicesKey = nameof(AllServicesKey);
            public const string AllSubscriptionsKey = nameof(AllSubscriptionsKey);
        }

        public class QueryInfo
        {
            public const string FreeAppointment = "free";
        }
    }
}
