global using AuthProviders;
global using Blazored.LocalStorage;
global using Components;
global using Contract;
global using Mapster;
global using Microsoft.AspNetCore.Components;
global using Microsoft.JSInterop;
global using MudBlazor;
global using Services;
global using System.Collections.ObjectModel;
global using System.ComponentModel;
global using System.Globalization;
global using System.Runtime.CompilerServices;
global using System.Security.Claims;
global using System.Text.Json;

namespace ViewModels
{
    public class ComponentBaseViewModel : ComponentBase, INotifyPropertyChanged, IDisposable
    {
        [Inject] public IApiService? ApiService { get; set; }

        [Inject] public HttpInterceptorService? Interceptor { get; set; }

        [Inject] public IAccountService? AccountService { get; set; }

        [Inject] public ITeamService? TeamService { get; set; }

        [Inject] public ICourseService? CourseService { get; set; }

        [Inject] public ICompanyFlagService? CompanyFlagService { get; set; }

        [Inject] public IGroupService? GroupService { get; set; }

        [Inject] public IPostService? PostService { get; set; }

        [Inject] public IPostLikeService? PostLikeService { get; set; }

        [Inject] public ICommentService? CommentService { get; set; }

        [Inject] public IReplyToCommentService? ReplyToCommentService { get; set; }

        [Inject] public INotificationService? NotificationService { get; set; }

        [Inject] public IChapterService? ChapterService { get; set; }

        [Inject] public ICompanyService? CompanyService { get; set; }

        [Inject] public ILocalStorageService? LocalStorageService { get; set; }

        [Inject] public IAuthenticationService? AuthenticationService { get; set; }

        [Inject] public NavigationManager? NavigationManager { get; set; }

        [Inject] public TokenAuthenticationStateProvider? StateProvider { get; set; }

        [Inject] public IDialogService? DialogService { get; set; }

        [Inject] public ISnackbar? Snackbar { get; set; }

        [Inject] public ILocalStorageService? LocalStorage { get; set; }

        [Inject] public INewsService? NewsService { get; set; }

        public JsonSerializerOptions? Options { get; set; } =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        protected JsonSerializerOptions? _options { get; set; } =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        [Inject] public IJSRuntime? Js { get; set; }

        private bool _isBusy = false;

        protected override void OnInitialized()
        {
            //Interceptor.RegisterEvent();
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetValue(ref _isBusy, value);
        }

        protected const string InfoFormat = "{first_item}-{last_item} von {all_items}";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetValue<T>(ref T backingFiled, T value, [CallerMemberName] string propertyName = null!)
        {
            if (EqualityComparer<T>.Default.Equals(backingFiled, value)) return;
            backingFiled = value;
            OnPropertyChanged(propertyName);
        }

        public async Task<Guid> GetCompanyIdFromStorage()
        {
            try
            {
                var companyIdRes = await LocalStorageService!.GetItemAsync<string>("companyId");
                return Guid.TryParse(companyIdRes, out var companyId) ? companyId : Guid.Empty;
            }
            catch
            {
                throw;
            }
        }

        public void Dispose() => Interceptor.DisposeEvent();
    }
}