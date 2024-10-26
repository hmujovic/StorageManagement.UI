using Components.Accounts;
using Components.Dialog;

namespace ViewModels
{
    public class AccountViewModel : ComponentBaseViewModel
    {
        public ObservableCollection<AccountDto> Fachadmins { get; set; } = [];

        public ObservableCollection<AccountDto> Users { get; set; } = [];

        public ObservableCollection<AccountDto> Counsellors { get; set; } = [];

        public AccountDto Account { get; set; } = new();

        public const string InternType = "intern";
        public const string ExternType = "extern";
        protected bool Loading = false;

        public string AccountId { get; set; }

        public bool IsAdmin { get; set; }
        public bool IsOrganisation { get; set; }
        public bool IsCounsellor { get; set; }

        public Guid CompanyId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            AccountId = await LocalStorage!.GetItemAsync<string>("accountId");
            var state = await StateProvider!.GetAuthenticationStateAsync();
            var roles = state.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
            IsAdmin = roles.Contains("Superadmin");
            IsOrganisation = roles.Contains("Fachadmin");
            IsCounsellor = roles.Contains("Counsellor");
            if (!IsAdmin)
            {
                var company = await CompanyService!.GetByAccountIdAsync(AccountId);
                CompanyId = company.Id;
            }

            PropertyChanged += async (_, _) => { await InvokeAsync(StateHasChanged); };
            await GetUserAccounts();
        }

        private async Task LoadAccounts()
        {
            await GetFachAdminAccounts();
            await GetUserAccounts();
            await GetCounsellorAccounts();
        }

        public async Task OpenCreateOrUpdateUser(AccountDto account, string role)
        {
            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
            };

            var accountCreationDto = account.Adapt<AccountCreationDto>();
            accountCreationDto.Roles = new List<string> { role };
            var parameters = new DialogParameters
            {
                ["Account"] = accountCreationDto
            };

            var dialogTitle = string.IsNullOrEmpty(account.Id) ? "Neuer Nutzer" : "Nutzer bearbeiten";

            var dialog = await DialogService!.ShowAsync<AccountFormComponent>(dialogTitle, parameters, options);
            var result = await dialog.Result;

            if (!result.Canceled)
            {
                await LoadAccounts();
            }
        }

        public async Task Delete(string accountId, string role)
        {
            try
            {
                var dialogResult = await SetConfirmDialog();
                if (!dialogResult.Canceled)
                {
                    var response = await AccountService!.Delete(accountId);
                    if (response.IsSuccess)
                    {
                        switch (role)
                        {
                            case "Fachadmin":
                                var adminAccount = Fachadmins.FirstOrDefault(a => a.Id == accountId);
                                if (adminAccount != null)
                                    Fachadmins.Remove(adminAccount);
                                Snackbar!.Add("Nutzer erfolgreich gelöscht", Severity.Success);
                                break;

                            case "Kunde":
                                var userAccount = Users.FirstOrDefault(a => a.Id == accountId);
                                if (userAccount != null)
                                    Users.Remove(userAccount);
                                Snackbar!.Add("Nutzer erfolgreich gelöscht", Severity.Success);
                                break;
                        }
                    }
                    else
                    {
                        Snackbar!.Add("Ein Fehler ist aufgetreten", Severity.Error);
                    }
                    await LoadAccounts();
                    StateHasChanged();
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task GetFachAdminAccounts()
        {
            try
            {
                Loading = true;
                Fachadmins = IsAdmin ? await AccountService!.GetByRoleAsync("Fachadmin") :
                    await AccountService!.GetByRoleAndCompanyAsync("Fachadmin", CompanyId);

                Loading = false;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task GetUserAccounts()
        {
            try
            {
                Loading = true;
                Users = IsAdmin ? await AccountService!.GetByRoleAsync("Kunde") :
                await AccountService!.GetByRoleAndCompanyAsync("Kunde", CompanyId);
                Loading = false;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task GetCounsellorAccounts()
        {
            try
            {
                Loading = true;
                Counsellors = IsAdmin ? await AccountService!.GetByRoleAsync("Counsellor") :
                    await AccountService!.GetByRoleAndCompanyAsync("Counsellor", CompanyId);
                Loading = false;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task<DialogResult> SetConfirmDialog()
        {
            var settingsDialog = new SettingDialogParameters<ConfirmComponent>(DialogService!, "Löschen Sie den Nutzer",
                "Sind Sie sicher, dass Sie den Nutzer löschen möchten?", "Löschen", Color.Error);
            return await settingsDialog.SetConfirmDialog();
        }

        #region For Search

        public string? SearchString { get; set; }

        public bool FilterFunc(AccountDto element)
        {
            if (string.IsNullOrEmpty(SearchString))
                return true;
            if (element!.FirstName!.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element!.LastName!.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element!.MobileNumber! != null &&
                element!.MobileNumber!.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Email!.Contains(SearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

        #endregion For Search
    }
}