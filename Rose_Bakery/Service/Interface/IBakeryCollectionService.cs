using Rose_Bakery.Dto.Response;

namespace Rose_Bakery.Service.Interface
{
    public interface IBakeryCollectionService
    {
        Task<IList<BakeryResponseDto>> GetBakeryCollectionAsync();

    }
}
