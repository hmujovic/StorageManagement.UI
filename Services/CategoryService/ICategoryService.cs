namespace Services
{
    public interface ICategoryService
    {
        Task<ObservableCollection<CategoryDto>> GetAll(CancellationToken cancellationToken = default);

        Task<CategoryDto> GetById(int categoryId, CancellationToken cancellationToken = default);

        Task<GeneralResponseDto> Delete(int categoryId, CancellationToken cancellationToken = default);

        Task<GeneralResponseDto> Create(CategoryCreateDto categoryDto, CancellationToken cancellationToken = default);

        Task<GeneralResponseDto> Update(int categoryId, CategoryUpdateDto categoryDto, CancellationToken cancellationToken = default);
    }
}