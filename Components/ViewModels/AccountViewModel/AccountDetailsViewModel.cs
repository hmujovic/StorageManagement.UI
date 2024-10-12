namespace ViewModels
{
    public class AccountDetailsViewModel : ComponentBaseViewModel
    {
        [Parameter] public AccountDto Account { get; set; } = new();

        protected override Task OnInitializedAsync()
        {
            PropertyChanged += async (sender, e) => await InvokeAsync(StateHasChanged);
            return Task.CompletedTask;
        }
    }
}