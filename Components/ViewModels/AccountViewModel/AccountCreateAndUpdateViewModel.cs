namespace ViewModels
{
    public class AccountCreateAndUpdateViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected readonly CultureInfo De = CultureInfo.GetCultureInfo("de-DE");
        protected const string ValidationMessage = "Feld ist erforderlich.";
        [Parameter] public string? AccountId { get; set; }

        [Parameter]
        public AccountCreationDto? Account { get; set; }

        protected bool IsAdmin { get; set; }

        protected bool IsOrganisation { get; set; }

        public ObservableCollection<AccountRolesDto> AccountRoles { get; set; } = [];

        public ObservableCollection<CompanyDto> Companies { get; set; } = [];

        public CompanyDto? SelectedCompany { get; set; } 
       

        protected override async Task OnInitializedAsync()
        {
            PropertyChanged += (_, _) => StateHasChanged();
            AccountId = await LocalStorage!.GetItemAsync<string>("accountId");
            var state = await StateProvider!.GetAuthenticationStateAsync();
            var roles = state.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
            IsAdmin = roles.Contains("Superadmin");
            IsOrganisation = roles.Contains("Fachadmin");
            if (IsAdmin || IsOrganisation)
            {
                await GetAllRoles();
            }

            switch (IsAdmin)
            {
                case true:
                    Companies = await CompanyService!.GetAllAsync();
                    if (!string.IsNullOrEmpty(Account.Id))
                    {

                        SelectedCompany = Companies.FirstOrDefault(x => x.Id == Account.CompanyId);
                    }
                    break;

                case false:
                    SelectedCompany = await CompanyService!.GetByAccountIdAsync(AccountId);
                    break;
            }

            if (IsOrganisation)
            {
                var orgRoles = AccountRoles.Where(role => role.Name != "Superadmin" && role.Name != "Fachadmin").ToList();
                AccountRoles = new ObservableCollection<AccountRolesDto>(orgRoles);
            }

            
        }

        public async Task CreateOrUpdateAsync(AccountCreationDto account)
        {
            try
            {
                account.CompanyId = SelectedCompany.Id;
                var type = account.Roles.Any(x => x.Equals("Kunde")) ? "extern" : "intern";

                var response = string.IsNullOrEmpty(account.Id)
                    ? await AccountService!.CreateAccount(account!, type)
                    : await AccountService!.UpdateAsync(account!.Id!, account);

                await HandleCreateOrUpdateResponse(response);
            }
            catch (HttpRequestException)
            {
                MudDialog?.Close(DialogResult.Ok(true));
            }
        }

        private async Task GetAllRoles()
        {
            try
            {
                AccountRoles = await AccountService!.GetAllRoles();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async Task HandleCreateOrUpdateResponse(GeneralResponseDto response)
        {
            if (response.IsSuccess)
            {
                await HandleSuccessResponse(response);
                StateHasChanged();
            }
            else
            {
                Snackbar!.Add("FEHLER vom Server", Severity.Error);
            }

            MudDialog?.Close(DialogResult.Ok(true));
        }

        private async Task HandleSuccessResponse(GeneralResponseDto response)
        {
            var isSuccess = response.IsSuccess;
            Snackbar!.Add(isSuccess ? "Erfolgreich!" : "Fehler!", isSuccess ? Severity.Success : Severity.Error);

            if (await LocalStorage!.GetItemAsync<string>("accountId") == null)
            {
                MudDialog?.Close(DialogResult.Ok(true));
            }
            else
            {
                MudDialog?.Close(DialogResult.Ok(true));
            }
        }

        public void Cancel() => MudDialog?.Cancel();

        public bool IsDisabled =>
            string.IsNullOrEmpty(Account!.FirstName) ||
            string.IsNullOrEmpty(Account.LastName) ||
            string.IsNullOrEmpty(Account.Email);
    }
}