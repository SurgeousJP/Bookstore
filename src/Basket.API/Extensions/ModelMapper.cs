using Basket.API.gRPC;
using Basket.API.Model;

namespace Basket.API.Extensions
{
    public static class ModelMapper
    {
        public static CustomerBasket MapToCustomerBasket(string userId, List<Model.BasketItem> basketItems)
        {
            var customerBasket = new CustomerBasket
            {
                BuyerId = userId,
            };

            foreach (var item in basketItems)
            {
                customerBasket.Items.Add(new()
                {
                    BookId = item.BookId,
                    Title = item.Title,
                    UnitPrice = (decimal)item.UnitPrice,
                    OldUnitPrice = (decimal)item.OldUnitPrice,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl,
                });
            }

            return customerBasket;
        }
    }
}
