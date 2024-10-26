namespace Services
{
    public static class ApiEndpoints
    {
        public const string BaseUrl = "https://localhost:5000/api";
        public const string AccountController = $"{BaseUrl}/accounts";
        public const string ProductController = $"{BaseUrl}/products";
        public const string CategoryController = $"{BaseUrl}/categories";
    }
}