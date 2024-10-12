namespace Services;

public interface IAuthenticationService
{
    Task<GeneralResponseDto> RegisterAsync(RegistrationDto registrationDto,
        CancellationToken cancellationToken = default);

    Task<AuthenticationDto> Login(LoginDto loginDto, CancellationToken cancellationToken = default);

    Task<GeneralResponseDto> ChangePassword(string id, ChangePasswordDto changePassword);

    Task<GeneralResponseDto> ForgotPassword(ForgotPasswordDto forgotPasswordDto,
        CancellationToken cancellationToken = default);

    Task<GeneralResponseDto> ResetPassword(ResetPasswordDto resetPasswordDto,
        CancellationToken cancellationToken = default);

    Task<bool> ConfirmEmail(ConfirmEmailDto confirmEmailDto, CancellationToken cancellationToken = default);

    Task<string> RefreshToken();
}