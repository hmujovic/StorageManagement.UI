namespace Services
{
    public class CategoryService(IApiService apiService) : ICategoryService
    {
        private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

        public async Task<GeneralResponseDto> Create(CategoryCreateDto categoryDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Post($"{ApiEndpoints.CategoryController}/create", categoryDto);
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

        public async Task<GeneralResponseDto> Delete(int categoryId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Delete($"{ApiEndpoints.CategoryController}/{categoryId}");
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

        public async Task<GeneralResponseDto> Update(int categoryId, CategoryUpdateDto categoryDto, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Put($"{ApiEndpoints.CategoryController}/update/{categoryId}", categoryDto);
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

        public async Task<ObservableCollection<CategoryDto>> GetAll(CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Get($"{ApiEndpoints.CategoryController}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<ObservableCollection<CategoryDto>>(responseStream, _options,
                    cancellationToken);
                return res ?? null!;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                return null!;
            }
        }

        public async Task<CategoryDto> GetById(int categoryId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await apiService.Get($"{ApiEndpoints.CategoryController}/details/{categoryId}");
                if (!response.IsSuccessStatusCode) return null!;
                await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
                var res = await JsonSerializer.DeserializeAsync<CategoryDto>(responseStream, _options,
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