using Basket.API.Model;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        Task<CustomerBasket> GetBasketAsync(string customerId);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string id);
        Task<List<CustomerBasket>> GetBasketsContainItemAsync(int itemId);
        Task UpdateItemPriceForBasket(int itemId, double newPrice);

    }
}
