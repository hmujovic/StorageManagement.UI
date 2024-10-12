using Components.Dialog;

namespace ViewModels
{
    public class DashboardViewModel : ComponentBaseViewModel
    {
        protected bool Loading;

        protected ObservableCollection<CourseDto> Courses { get; set; } = [];

        private bool IsAdmin { get; set; }

        protected double CourseCompletitionRate { get; set; }

        private string AccountId { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            AccountId = await LocalStorage!.GetItemAsync<string>("accountId");
            PropertyChanged += async (sender, e) => await InvokeAsync(StateHasChanged);
            var state = await StateProvider!.GetAuthenticationStateAsync();
            var roles = state.User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToArray();
            IsAdmin = roles.Contains("Superadmin");
            await LoadCourses();
            Loading = false;
        }

        protected async Task MarkCourseAsCompleted(CourseDto courseDto)
        {
            var course = courseDto.Adapt<CourseForCreationOrUpdateDto>();
            var parameters = new DialogParameters();
            var options = new DialogOptions() { CloseButton = true, MaxWidth = MaxWidth.ExtraSmall };
            var dialogTitle = "Kompletter Kurs";
            const string text = "Möchten Sie diesen Kurs als abgeschlossen markieren?";
            parameters.Add("ContentText", text);
            parameters.Add("ButtonText", "Akzeptieren");
            parameters.Add("Color", Color.Success);

            var dialog = await DialogService!.ShowAsync<ChangeStatusComponent>(dialogTitle, parameters, options);
            var result = await dialog.Result;
            var confirmation = result.Data as bool? ?? false;

            if (!result.Canceled)
            {
                course.Status = confirmation ? "completed" : course.Status;
                await CourseService!.UpdateCourseAsync(course.Id.Value, course);
            }

            await LoadCourses();
        }
        protected async Task OpenCourseChapters(CourseDto courseDto)
        {
            NavigationManager.NavigateTo($"/chapters/{courseDto.Id}");
        }

        private async Task LoadCourses()
        {
            try
            {
                Courses = IsAdmin ? await CourseService!.GetAllAsync() :
                    await CourseService!.GetByAccountIdAsync(AccountId);
                Courses = new ObservableCollection<CourseDto>(Courses);
                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}