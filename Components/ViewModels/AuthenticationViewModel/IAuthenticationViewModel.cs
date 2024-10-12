namespace ViewModels
{
    public interface IAuthenticationViewModel
    {
        public Task<AuthenticationDto> Login(LoginDto userForAuthentication);

        public Task<bool> Registration(RegistrationDto registrationAccount);

        public Task<GeneralResponseDto> ForgotPassword(ForgotPasswordDto forgotPassword);

        public Task<GeneralResponseDto> ResetPassword(ResetPasswordDto resetPassword);

        public Task<bool> ChangeUserPassword();

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public ChangePasswordDto ChangePassword { get; set; }
        public bool ShowAuthError { get; set; }

        public Task Logout();
    }
}