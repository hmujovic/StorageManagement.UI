namespace Services
{
    public static class ApiEndpoints
    {
        public const string BaseUrl = "https://localhost:5000/api";
        // public const string BaseUrl = "http://grief-api.institut-ida.de/api";
        //public const string BaseUrl = "http://birgit-api.institut-ida.de/api";

        public const string AccountController = $"{BaseUrl}/account";
        public const string TokenController = $"{BaseUrl}/token";
        public const string CompanyController = $"{BaseUrl}/company";
        public const string TeamController = $"{BaseUrl}/team";
        public const string CourseController = $"{BaseUrl}/course";
        public const string ChapterController = $"{BaseUrl}/chapter";
        public const string CommentController = $"{BaseUrl}/comment";
        public const string GroupController = $"{BaseUrl}/group";
        public const string PostController = $"{BaseUrl}/post";
        public const string PostLikeController = $"{BaseUrl}/postLike";
        public const string ReplyToCommentController = $"{BaseUrl}/replyToComment";
        public const string NotificationController = $"{BaseUrl}/notification";
        public const string NewsController = $"{BaseUrl}/news";
        public const string CompanyFlagController = $"{BaseUrl}/companyFlag";
        public const string FeatureFlagController = $"{BaseUrl}/featureFlag";
    }
}