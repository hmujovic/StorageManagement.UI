namespace Services
{
    public interface IProductService
    {
        Task<ObservableCollection<ProductDto>> GetAll(CancellationToken cancellationToken = default);

        Task<ProductDto> GetById(int productId, CancellationToken cancellationToken = default);

        Task<ObservableCollection<ProductDto>> GetByCategoryId(int categoryId, CancellationToken cancellationToken = default);

        Task<GeneralResponseDto> Delete(int productId, CancellationToken cancellationToken = default);

        Task<GeneralResponseDto> Create(ProductCreateDto productDto, CancellationToken cancellationToken = default);

        Task<GeneralResponseDto> Update(int productId, ProductUpdateDto productDto, CancellationToken cancellationToken = default);
    }
}