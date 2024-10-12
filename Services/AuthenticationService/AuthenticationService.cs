namespace Services;

public class AuthenticationService(IApiService apiService,
        HttpClient client,
        ILocalStorageService localStorage) : IAuthenticationService
{
    private readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public async Task<string> RefreshToken()
    {
        var token = await localStorage.GetItemAsync<string>("accessToken");
        var refreshToken = await localStorage.GetItemAsync<string>("refreshToken");
        var tokenDto = JsonSerializer.Serialize(new RefreshTokenDto { Token = token, RefreshToken = refreshToken });
        var bodyContent = new StringContent(tokenDto, Encoding.UTF8, "application/json");
        var refreshResult = await client.PostAsync($"{ApiEndpoints.TokenController}/refresh", bodyContent);
        var refreshContent = await refreshResult.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<AuthenticationDto>(refreshContent, _options);
        if (!refreshResult.IsSuccessStatusCode)
            throw new ApplicationException("Something went wrong during the refresh token action");
        await localStorage.SetItemAsync("accessToken", result.AccessToken);
        await localStorage.SetItemAsync("refreshToken", result.RefreshToken);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.AccessToken);
        return result.AccessToken;
    }

    public async Task<GeneralResponseDto> ChangePassword(string id, ChangePasswordDto changePassword)
    {
        try
        {
            var response =
                await apiService.PutAsync($"{ApiEndpoints.AccountController}/change/password", id, changePassword);
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

    public async Task<bool> ConfirmEmail(ConfirmEmailDto confirmEmailDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await apiService.PostAsync($"{ApiEndpoints.AccountController}/confirmEmail", confirmEmailDto);
            if (!response.IsSuccessStatusCode) return false;
            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var res = await JsonSerializer.DeserializeAsync<GeneralResponseDto>(responseStream, _options,
                cancellationToken);
            return res != null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<GeneralResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await apiService.PostAsync($"{ApiEndpoints.AccountController}/forgotPassword", forgotPasswordDto);
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

    public async Task<AuthenticationDto> Login(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await apiService.PostAsync($"{ApiEndpoints.AccountController}", loginDto);
            if (!response.IsSuccessStatusCode) return null!;
            await using var responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var res = await JsonSerializer.DeserializeAsync<AuthenticationDto>(responseStream, _options,
                cancellationToken);
            return res ?? null!;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(ex.Message);
            return null!;
        }
    }

    public async Task<GeneralResponseDto> RegisterAsync(RegistrationDto registrationDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await apiService.PostAsync($"{ApiEndpoints.AccountController}/registration", registrationDto);
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

    public async Task<GeneralResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await apiService.PostAsync($"{ApiEndpoints.AccountController}/resetPassword", resetPasswordDto);
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
}