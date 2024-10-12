namespace Services;

public interface IAccountService
{
    Task<ObservableCollection<AccountDto>> GetAllAccounts();

    Task<AccountDto> GetAccountById(string accountId);

    Task<ObservableCollection<AccountDto>> GetByRoleAsync(string role);

    Task<ObservableCollection<string>> GetAllRolesAccAsync(string roleId);

    Task<GeneralResponseDto> CreateAccount(AccountCreateDto accountDto, string type);

    Task<GeneralResponseDto> UpdateAsync(string accountId, AccountUpdateDto account);

    Task<GeneralResponseDto> DeleteAccount(string accountId);
}