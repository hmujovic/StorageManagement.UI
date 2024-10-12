using Components.Accounts;

namespace ViewModels
{
    public class AccountSettingsViewModel : ComponentBaseViewModel
    {
        public AccountDto Account { get; set; } = new();

        protected bool Loading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            PropertyChanged += async (sender, e) => { await InvokeAsync(StateHasChanged); };
            await GetAccountAsync();
            Loading = false;
        }

        public async Task OpenUpdateUser(AccountDto account)
        {
            var options = new DialogOptions()
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
            };

            var accountCreate = account.Adapt<AccountCreationDto>();

            var parameters = new DialogParameters { ["Account"] = accountCreate };
            var text = !string.IsNullOrEmpty(account.Id) ? "Benutzer bearbeiten" : "Neuen Benutzer erstellen";

            var dialog = await DialogService!.ShowAsync<AccountFormComponent>(text, parameters, options);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await GetAccountAsync();
                StateHasChanged();
            }
        }

        public void OpenChangePassword()
        {
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.Small };
            DialogService!.Show<AccountChangePasswordComponent>(null, new DialogParameters(), options);
        }

        private async Task GetAccountAsync()
        {
            var accountId = await LocalStorage!.GetItemAsync<string>("accountId");
            try
            {
                Account = await AccountService!.GetAccountById(accountId);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}