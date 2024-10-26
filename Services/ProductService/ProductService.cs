namespace Services
{
    public class ProductService(IApiService apiService) : IProductService
    {
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public async Task<GeneralResponseDto> Create(ProductCreateDto productDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Post($"{ApiEndpoints.ProductController}/create", productDto);
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options,
                    cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<GeneralResponseDto> Delete(int productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Delete($"{ApiEndpoints.ProductController}/{productId}");
                return response.IsSuccessStatusCode
                    ? new GeneralResponseDto { IsSuccess = true }
                    : new GeneralResponseDto { IsSuccess = false };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return new GeneralResponseDto { IsSuccess = false };
            }
        }

        public async Task<GeneralResponseDto> Update(int productId, ProductUpdateDto productDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Put($"{ApiEndpoints.ProductController}/update/{productId}", productDto);
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options,
                    cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ObservableCollection<ProductDto>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Get($"{ApiEndpoints.ProductController}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<ObservableCollection<ProductDto>>(responseStream, _options,
                    cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ObservableCollection<ProductDto>> GetByCategoryId(int categoryId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Get($"{ApiEndpoints.ProductController}/byCategory/{categoryId}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<ObservableCollection<ProductDto>>(responseStream, _options,
                    cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<ProductDto> GetById(int productId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Get($"{ApiEndpoints.ProductController}/details/{productId}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<ProductDto>(responseStream, _options,
                    cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }
    }
}