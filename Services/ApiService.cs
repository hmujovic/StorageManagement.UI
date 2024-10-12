namespace Services
{
    public class ApiService(HttpClient httpClient, ILocalStorageService localStorage) : IApiService
    {
        private async Task AddAuthorizationHeader()
        {
            var accessToken = await localStorage!.GetItemAsync<string>("accessToken");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<HttpResponseMessage> CreateAsync(string url, object obj)
        {
            var json = new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                Application.Json);
            return await httpClient.PostAsync(url, json);
        }

        public async Task<HttpResponseMessage> DeleteByGuidAsync(string url, Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url + "/" + id);
            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteByIdAsync(string url, string id)
        {
            var req = url + "/" + id;
            var request = new HttpRequestMessage(HttpMethod.Delete, req);
            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            await AddAuthorizationHeader();
            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> GetByIdAsync(string url, string id)
        {
            var req = url + "/" + id;
            var request = new HttpRequestMessage(HttpMethod.Get, req);
            await AddAuthorizationHeader();
            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, StringContent bodyContent)
        {
            return await httpClient.PostAsync(url + "/", bodyContent);
        }

        public async Task<HttpResponseMessage> PostUnauthorizedAsync(string url, StringContent bodyContent)
        {
            return await httpClient.PostAsync(url + "/", bodyContent);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, Guid id, object obj)
        {
            var req = url + "/" + id;
            var json = new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                Application.Json);
            return await httpClient.PutAsync(req + "/", json);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, string id, object obj)
        {
            var req = url + "/" + id;
            var json = new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                Application.Json);
            return await httpClient.PutAsync(req + "/", json);
        }

        public async Task<HttpResponseMessage> GetByGuidAsync(string url, Guid id)
        {
            var req = url + "/" + id;
            var request = new HttpRequestMessage(HttpMethod.Get, req);
            await AddAuthorizationHeader();
            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent bodyContent)
        {
            await AddAuthorizationHeader();
            return await httpClient.PostAsync(url + "/", bodyContent);
        }

        public async Task<HttpResponseMessage> GetUnauthorizedAsync(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            return await httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object obj)
        {
            var json = new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                Application.Json);
            return await httpClient.PostAsync(url, json);
        }

        public async Task<HttpResponseMessage> PatchAsync(string url, Guid id, object obj)
        {
            var req = url + "/" + id;
            var json = new StringContent(
                JsonSerializer.Serialize(obj),
                Encoding.UTF8,
                Application.Json);
            return await httpClient.PatchAsync(req + "/", json);
        }

        public async Task<HttpResponseMessage> UploadFileAsync(string url, MultipartFormDataContent content)
        {
            return await httpClient.PostAsync(url, content);
        }
    }
}