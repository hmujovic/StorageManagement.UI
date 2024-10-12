using AuthProviders;

namespace Services;

public class RefreshTokenService(TokenAuthenticationStateProvider authProvider, IAuthenticationService authService)
{
    public async Task<string> TryRefreshToken()
    {
        var authState = await authProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        var exp = user.FindFirst(c => c.Type.Equals("exp")).Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var timeUTC = DateTime.UtcNow;
        var diff = expTime - timeUTC;
        if (diff.TotalMinutes <= 2)
            return await authService.RefreshToken();
        return string.Empty;
    }
}