namespace ViewModels
{
    public class ConfirmEmailViewModel : ComponentBaseViewModel
    {
        private string? _email = "";
        private string? _token = "";

        [Parameter] public string? Email { get; set; }
        [Parameter] public string? EmailConfirmationToken { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PropertyChanged += async (sender, e) => await InvokeAsync(StateHasChanged);
            await GetHttpParameters();
        }

        private async Task GetHttpParameters()
        {
            var uriBuilder = new UriBuilder(NavigationManager!.Uri);
            var httpQuery = System.Web.HttpUtility.ParseQueryString(uriBuilder.Query);
            _email = httpQuery["email"] ?? "";
            _token = httpQuery["token"] ?? "";
            await ConfirmEmail();
        }

        private async Task ConfirmEmail()
        {
            try
            {
                ConfirmEmailDto confirm = new()
                {
                    Email = _email!,
                    EmailConfirmationToken = _token!,
                };

                var response = await AuthenticationService!.ConfirmEmail(confirm);
                if (response)
                {
                    ConfigureSnackbar();
                    Snackbar!.Add("E-Mail erfolgreich bestätigt", Severity.Success);
                    await Task.Delay(1000);
                    NavigationManager!.NavigateTo("/");
                    StateHasChanged();
                }
                else
                {
                    Snackbar!.Add("E-Mail ist nicht verifiziert! Versuchen Sie es erneut.", Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private void ConfigureSnackbar()
        {
            Snackbar!.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
        }
    }
}