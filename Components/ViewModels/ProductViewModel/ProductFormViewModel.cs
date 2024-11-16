using System.Collections.ObjectModel;

namespace ViewModels
{
    public class ProductFormViewModel : ComponentBaseViewModel
    {
        [CascadingParameter] private MudDialogInstance? MudDialog { get; set; }
        protected const string ValidationMessage = "Field is required.";

        [Parameter]
        public ProductCreateDto? ProductCreate { get; set; }
        
        [Parameter]
        public ProductUpdateDto? ProductUpdate { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; } = [];

        public CategoryDto? SelectedCategory { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Categories = await CategoryService!.GetAll();
            if (ProductUpdate != null) 
            {
                var category = Categories.FirstOrDefault(x => x.Id == ProductUpdate.CategoryId);
                SelectedCategory = category;
                StateHasChanged();
            }
        }

        public async Task CreateOrUpdate()
        {
            try
            {   
                var response = new GeneralResponseDto();
                if (ProductCreate != null && ProductCreate.Id == 0)
                {
                    ProductCreate.CategoryId = SelectedCategory.Id;
                    response = await ProductService!.Create(ProductCreate);
                }
                else
                {
                    ProductUpdate!.CategoryId = SelectedCategory.Id;
                    response = await ProductService!.Update(ProductUpdate!.Id!, ProductUpdate);
                }

                HandleResponse(response);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                MudDialog!.Close(DialogResult.Ok(true));
            }
        }

        private void HandleResponse(GeneralResponseDto response)
        {
            var isSuccess = response?.IsSuccess == true;
            Snackbar!.Add(isSuccess ? "Success!" : "Error!", isSuccess ? Severity.Success : Severity.Error);
            MudDialog!.Close(DialogResult.Ok(true));
        }

        public void Cancel() => MudDialog!.Cancel();

        public bool Disabled => 
            (
                (ProductCreate != null && (string.IsNullOrWhiteSpace(ProductCreate.Name)))  ||
                (ProductUpdate != null && (string.IsNullOrWhiteSpace(ProductUpdate.Name)))
            );
    }
}