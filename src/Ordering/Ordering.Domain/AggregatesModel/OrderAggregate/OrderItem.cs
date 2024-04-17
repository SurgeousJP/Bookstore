using Ordering.Domain.Exceptions;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    internal class OrderItem
    {
        public int BookId { get; private set; }

        private string _title;
        private decimal _unitPrice;
        private int _units;
        private string _productImage;
        private decimal _discountPercentage;

        protected OrderItem() { }

        public OrderItem(int bookId, string title, decimal unitPrice, string productImage, decimal discountPercentage, int units = 1)
        {
            if (units <= 0)
            {
                throw new OrderingDomainException("Invalid number of units");
            }

            BookId = bookId;

            _title = title;
            _unitPrice = unitPrice;
            _units = units;
            _productImage = productImage;
            _discountPercentage = discountPercentage;
        }

        public string GetProductImage() => _productImage;
        public decimal GetDiscountPercentage() => _discountPercentage;
        public int GetUnits() => _units;
        public decimal GetUnitPrice() => _unitPrice;
        public string GetOrderItemBookName() => _title;

        public void SetNewDiscountPercentage(decimal discountPercentage)
        {
            if (discountPercentage < 0)
            {
                throw new OrderingDomainException("Discount percentage is not valid, must be non-negative");
            }
        }

        public void AddUnits(int units)
        {
            if (units < 0)
            {
                throw new OrderingDomainException("Invalid units");
            }
        }
    }
}
