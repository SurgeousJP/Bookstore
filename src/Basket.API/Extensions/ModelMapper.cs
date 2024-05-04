using Basket.API.Model;
using EventBus.Messaging.Events;
using BasketItem = EventBus.Messaging.Events.BasketItem;

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

        public static BasketCheckoutEvent MapToBasketCheckoutEvent(string buyerId, ICollection<Model.BasketItem> basketItems)
        {
            var basketData = new BasketCheckoutEvent
            {
                BuyerId = buyerId,
                BasketItems = new List<BasketItem>()
            };

            foreach (var item in basketItems)
            {
                basketData.BasketItems.Add(new()
                {
                    BookId = item.BookId,
                    Title = item.Title,
                    UnitPrice = (decimal)item.UnitPrice,
                    OldUnitPrice = (decimal)item.OldUnitPrice,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl,
                });
            }

            return basketData;
        }
    }
}
