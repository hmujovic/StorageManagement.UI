namespace Services
{
    public interface IApiService
    {
        public Task<HttpResponseMessage> CreateAsync(string url, object obj);

        public Task<HttpResponseMessage> DeleteByGuidAsync(string url, Guid id);

        public Task<HttpResponseMessage> DeleteByIdAsync(string url, string id);

        public Task<HttpResponseMessage> GetAsync(string url);

        public Task<HttpResponseMessage> GetByGuidAsync(string url, Guid id);

        public Task<HttpResponseMessage> GetByIdAsync(string url, string id);

        public Task<HttpResponseMessage> PostAsync(string url, StringContent bodyContent);

        public Task<HttpResponseMessage> PostUnauthorizedAsync(string url, StringContent bodyContent);

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent bodyContent);

        public Task<HttpResponseMessage> PostAsync(string url, object obj);

        public Task<HttpResponseMessage> PutAsync(string url, Guid id, object obj);

        public Task<HttpResponseMessage> PutAsync(string url, string id, object obj);

        public Task<HttpResponseMessage> PatchAsync(string url, Guid id, object obj);

        public Task<HttpResponseMessage> UploadFileAsync(string url, MultipartFormDataContent content);
    }
}