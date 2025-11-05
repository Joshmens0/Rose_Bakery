using Rose_Bakery.Dto.Request;
using Rose_Bakery.Dto.Response;
using System.Threading.Tasks;

namespace Rose_Bakery.Service.Interface
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(OrderRequestDto request);
    }
}
