using Basket.API.Model;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string customerId);
        Task<CustomerBasket> UpdateBasketAsnc(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string id);
    }
}
