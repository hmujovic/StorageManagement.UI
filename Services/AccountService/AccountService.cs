namespace Services;

public class AccountService(IApiService apiService) : IAccountService
{
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public async Task<GeneralResponseDto> CreateAccount(AccountCreateDto accountDto, string type)
    {
        try
        {
            var response = await apiService.PostAsync($"{ApiEndpoints.AccountController}/create/{type}", accountDto);
            if (!response.IsSuccessStatusCode) return null!;
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options);
            return res ?? null!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<GeneralResponseDto> DeleteAccount(string accountId)
    {
        try
        {
            var response = await apiService.DeleteByIdAsync($"{ApiEndpoints.AccountController}", accountId);
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

    public async Task<AccountDto> GetAccountById(string accountId)
    {
        try
        {
            var response = await apiService.GetAsync($"{ApiEndpoints.AccountController}/details/{accountId}");
            if (!response.IsSuccessStatusCode) return null!;
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var res = await JsonSerializer.DeserializeAsync<AccountDto>(responseStream, _options);
            return res ?? null!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<ObservableCollection<AccountDto>> GetAllAccounts()
    {
        try
        {
            var response = await apiService.GetAsync($"{ApiEndpoints.AccountController}");
            if (!response.IsSuccessStatusCode) return null!;
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var res = await JsonSerializer.DeserializeAsync<ObservableCollection<AccountDto>>(responseStream, _options);
            return res ?? null!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return null!;
        }
    }


    public async Task<ObservableCollection<AccountDto>> GetByRoleAsync(string role)
    {
        try
        {
            var response = await apiService.GetAsync($"{ApiEndpoints.AccountController}/byRole/{role}");
            if (!response.IsSuccessStatusCode) return [];
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var res = await JsonSerializer.DeserializeAsync<ObservableCollection<AccountDto>>(responseStream,
                _options);
            return res ?? null!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return null!;
        }
    }


    public async Task<ObservableCollection<string>> GetAllRolesAccAsync(string roleId)
    {
        try
        {
            var response = await apiService.GetAsync($"{ApiEndpoints.AccountController}/roles/role/{roleId}");
            if (!response.IsSuccessStatusCode) return [];
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var res = await JsonSerializer.DeserializeAsync<ObservableCollection<string>>(responseStream, _options);
            return res ?? null!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<GeneralResponseDto> UpdateAsync(string accountId, AccountUpdateDto account)
    {
        try
        {
            var response = await apiService.PutAsync($"{ApiEndpoints.AccountController}", accountId, account);
            if (!response.IsSuccessStatusCode) return null!;
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options);
            return res ?? null!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return null!;
        }
    }
}