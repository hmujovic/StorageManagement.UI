namespace ViewModels
{
    public abstract class BaseViewModel : ComponentBase
    {
        [Inject] public IDialogService? DialogService { get; set; }

        public static MudTheme DefaultTheme = new()
        {
            PaletteLight = new PaletteLight()
            {
                Primary = new MudBlazor.Utilities.MudColor("#6AACBD"),
                Secondary = new MudBlazor.Utilities.MudColor("#6AACBD"),
                Tertiary = new MudBlazor.Utilities.MudColor("#FE6625"),
                AppbarBackground = new MudBlazor.Utilities.MudColor("#6AACBD"),
                WarningContrastText = Colors.Amber.Lighten5,
                Error = Colors.Red.Darken4,
                Success = Colors.Green.Darken1,
                Warning = Colors.Amber.Default
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
    }
}