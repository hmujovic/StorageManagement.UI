namespace ViewModels
{
    public class AuthenticationViewModel(
        HttpClient client,
        TokenAuthenticationStateProvider authStateProvider,
        ISnackbar snackbar,
        ILocalStorageService localStorage,
        NavigationManager navigationManager,
        IAuthenticationService authenticationService) : IAuthenticationViewModel
    {
        private ChangePasswordDto _changePassword = new();

        public string SuccessMessage { get; set; } = string.Empty;
        public bool IsSuccess;
        public string ErrorMessage { get; set; } = string.Empty;
        public bool ShowAuthError { get; set; }

        public async Task<GeneralResponseDto> ForgotPassword(ForgotPasswordDto forgotPassword)
        {
            var authResult = await authenticationService.ForgotPassword(forgotPassword);
            return authResult.IsSuccess
                ? authResult
                : new GeneralResponseDto
                { IsSuccess = false, Message = "Serverfehler. Versuchen Sie es später erneut. Danke!" };
        }

        public async Task<GeneralResponseDto> ResetPassword(ResetPasswordDto resetPassword)
        {
            var authResult = await authenticationService.ResetPassword(resetPassword);
            return authResult.IsSuccess
                ? authResult
                : new GeneralResponseDto
                { IsSuccess = false, Message = "Serverfehler. Versuchen Sie es später erneut. Danke!" };
        }

        public async Task<bool> ChangeUserPassword()
        {
            var authState = await authStateProvider!.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user!.Identity!.IsAuthenticated)
            {
                var authMessage = $"{user.Identity.Name} is authenticated.";
                var claims = user.Claims;
                var email = user.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;

                var accountId = await localStorage!.GetItemAsync<string>("accountId");
                _changePassword.Email = email;

                var response = await authenticationService.ChangePassword(accountId.ToString(), _changePassword);

                return response.IsSuccess;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> Registration(RegistrationDto registrationAccount)
        {
            try
            {
                var authResult = await authenticationService.RegisterAsync(registrationAccount);
                if (authResult.IsSuccess)
                {
                    snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopLeft;
                    snackbar.Add("Sie haben sich erfolgreich registriert!", Severity.Success);
                    IsSuccess = true;
                }
                else
                {
                    if (authResult.Data == null) return authResult.IsSuccess;
                    snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopLeft;
                    snackbar.Add("Es existiert ein Benutzer mit dieser E-Mail", Severity.Error);
                    return authResult.IsSuccess;
                }

                return authResult.IsSuccess;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<AuthenticationDto> Login(LoginDto userForAuthentication)
        {
            var authResult = await authenticationService.Login(userForAuthentication);
            if (authResult.IsSuccessful)
            {
                await localStorage!.SetItemAsync("accountId", authResult!.AccountId!.ToString()!);
                await localStorage.SetItemAsStringAsync("accessToken", authResult.AccessToken!);
                await localStorage.SetItemAsync("refreshToken", authResult.RefreshToken);
                await localStorage!.SetItemAsync<string>("accountFirstName", authResult!.AccountFirstName!);

                authStateProvider!.StateChanged();

                return new AuthenticationDto { IsSuccessful = true, AccessToken = authResult.AccessToken };
            }
            else
            {
                return new AuthenticationDto { IsSuccessful = false, ErrorMessage = authResult.ErrorMessage };
            }
        }

        public async Task Logout()
        {
            await localStorage.RemoveItemAsync("accessToken");
            await localStorage.RemoveItemAsync("accountId");
            await localStorage.RemoveItemAsync("refreshToken");
            client.DefaultRequestHeaders.Authorization = null;
            navigationManager.NavigateTo("/", true);
        }

        public ChangePasswordDto ChangePassword
        {
            get => _changePassword;
            set => _changePassword = value;
        }
    }
}