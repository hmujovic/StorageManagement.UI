namespace ViewModels
{
    public abstract class BaseViewModel : ComponentBase, INotifyPropertyChanged
    {
        [Inject] public IDialogService? DialogService { get; set; }

        public static MudTheme DefaultTheme = new()
        {
            Palette = new Palette()
            {
                Primary = new MudBlazor.Utilities.MudColor("#6AACBD"),
                Secondary = new MudBlazor.Utilities.MudColor("#6AACBD"),
                Tertiary = new MudBlazor.Utilities.MudColor("#FE6625"),
                AppbarBackground = new MudBlazor.Utilities.MudColor("#6AACBD"),
                WarningContrastText = Colors.Amber.Lighten5,
                Error = Colors.Red.Darken4,
                Success = Colors.Green.Darken1,
                Warning = Colors.Amber.Default,
                //TextSecondary = new MudBlazor.Utilities.MudColor("#FE6625"),
            },
            Typography = new Typography()
            {
                Default = new Default()
                {
                    FontSize = "14px",
                    FontFamily = ["Roboto", "Helvetica", "Arial", "sans-serif"]
                },
                H6 = new H6()
                {
                    FontFamily = ["Roboto", "Helvetica", "Arial", "sans-serif"],
                    FontSize = "1rem",
                    FontWeight = 500,
                    LineHeight = 1.2,
                    LetterSpacing = ".0075em"
                }
            },
            LayoutProperties = new LayoutProperties()
            {
                DrawerWidthLeft = "350px",
                DrawerWidthRight = "300px",
                DefaultBorderRadius = "8px",
            }
        };

        private bool _isBusy = false;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetValue(ref _isBusy, value);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SetValue<T>(ref T backingFiled, T value, [CallerMemberName] string propertyName = null!)
        {
            if (EqualityComparer<T>.Default.Equals(backingFiled, value)) return;
            backingFiled = value;
            OnPropertyChanged(propertyName);
        }
    }
}