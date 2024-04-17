using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    internal class Order : Entity, IAggregateRoot
    {
        private DateTime _orderDate;

        public Address Address { get; private set; }

        private int? _buyerId;
        public int? GetBuyerId => _buyerId;

        public OrderStatus OrderStatus { get; private set; }
        private int _orderStatusId;

        private string _description;

        private bool _isDraft;

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        private int? _paymentMethodId;

        public static Order NewDraft()
        {
            var order = new Order();
            order._isDraft = true;
            return order;
        }

        protected Order()
        {
            _orderItems = new List<OrderItem>();
            _isDraft = false;
        }

        public Order(string userId, string userName, Address address, int cardTypeId, string cardNumber, string cardSecurityNumber, string cardHolderName, DateTime cardExpiration, int? buyerId = null, int? paymentMethodId = null) : this()
        {
            Address = address;
            _buyerId = buyerId;
            _paymentMethodId = paymentMethodId;
            _orderStatusId = OrderStatus.Submitted.Id;
            _orderDate = DateTime.UtcNow;

            AddDomainEvent(new OrderStartedDomainEvent(userId, userName, cardTypeId, cardNumber, cardSecurityNumber, cardHolderName, cardExpiration, this));
        }

        public void AddOrderItem(int bookId, string title, decimal unitPrice, string productImage, decimal discountPercentage, int units)
        {
            var existingOrderForProduct = _orderItems
                .Where(o => o.BookId == bookId)
                .SingleOrDefault();

            if (existingOrderForProduct != null)
            {
                if (discountPercentage > existingOrderForProduct.GetDiscountPercentage())
                {
                    existingOrderForProduct.SetNewDiscountPercentage(discountPercentage);
                }
                existingOrderForProduct.AddUnits(units);
            }
            else
            {
                var orderItem = new OrderItem(bookId, title, unitPrice, productImage, discountPercentage, units);

                _orderItems.Add(orderItem);
            }
        }
        public void SetPaymentId(int id)
        {
            this._paymentMethodId = id;
        }
        public void SetBuyerId(int id)
        {
            this._buyerId = id;
        }
        public void SetAwaitingValidationStatus()
        {
            if (_orderStatusId == OrderStatus.Submitted.Id)
            {
                AddDomainEvent(new OrderStatusChangedToAwaitingValidationDomainEvent(Id, _orderItems));
                _orderStatusId = OrderStatus.AwaitingValidation.Id;
            }
        }
        public void SetStockConfirmedStatus()
        {
            if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
            {
                AddDomainEvent(new OrderStatusChangedToStockConfirmedDomainEvent(Id));
                _orderStatusId = OrderStatus.StockConfirmed.Id;
                _description = "All items were confirmed with available stock.";
            }
        }

        public void SetPaidStatus()
        {
            if (_orderStatusId == OrderStatus.StockConfirmed.Id)
            {
                AddDomainEvent(new OrderStatusChangedToPaidDomainEvent(Id));
                _orderStatusId = OrderStatus.Paid.Id;
                _description = "The order has been paid and ready for shipping";
            }
        }
        public void SetShippedStatus()
        {
            if (_orderStatusId != OrderStatus.Paid.Id)
            {
                StatusChangeException(OrderStatus.Shipped);
            }
            AddDomainEvent(new OrderShippedDomainEvent(Id));
            _orderStatusId = OrderStatus.Shipped.Id;
            _description = "The order has been shipped successfully";
        }
        public void SetCancelledStatus()
        {
            if (_orderStatusId == OrderStatus.Paid.Id || _orderStatusId == OrderStatus.Shipped.Id)
            {
                StatusChangeException(OrderStatus.Cancelled);
            }
            _orderStatusId = OrderStatus.Cancelled.Id;
            _description = "The order was cancelled";
            AddDomainEvent(new OrderCancelledDomainEvent(Id)); 
        }
        public void SetCancelledStatusWhenStockIsRejected(IEnumerable<int> orderStockRejectedItems)
        {
            if (_orderStatusId == OrderStatus.AwaitingValidation.Id)
            {
                _orderStatusId = OrderStatus.Cancelled.Id;

                var itemsStockRejectedProductName = OrderItems
                    .Where(c => orderStockRejectedItems.Contains(c.BookId))
                    .Select(c => c.GetOrderItemBookName());

                var itemsStockRejectedProductDescription = string.Join(", ", itemsStockRejectedProductName);

                _description = $"The book items don't have stock ({itemsStockRejectedProductDescription})";
            }
        }

        private void StatusChangeException(OrderStatus statusToChange)
        {
            throw new OrderingDomainException($"It is not possible to change order status from {OrderStatus.Name} to {statusToChange.Name}");
        }
        public decimal GetTotal()
        {
            return _orderItems.Sum(o => o.GetUnits() * o.GetUnitPrice());  
        }
    }
}
