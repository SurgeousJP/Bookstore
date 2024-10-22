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

            int idx = 1;

            foreach (var item in basketItems)
            {
                customerBasket.Items.Add(new()
                {
                    Id = idx++,
                    BookId = item.BookId,
                    Title = item.Title,
                    UnitPrice = (decimal)item.UnitPrice,
                    OldUnitPrice = (decimal)item.OldUnitPrice,
                    Quantity = item.Quantity,
                    ImageUrl = item.ImageUrl,
                    Selected = item.Selected
                });
            }

            return customerBasket;
        }
    }
}
