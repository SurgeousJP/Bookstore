using MediatR;
using Ordering.Domain.AggregatesModel.BuyerAggregate;

namespace Ordering.Domain.Events
{
    internal class BuyerPaymentMethodVerifiedDomainEvent : INotification
    {
        public Buyer Buyer;
        public PaymentMethod PaymentMethod;
        public int OrderId;

        public BuyerPaymentMethodVerifiedDomainEvent(Buyer buyer, PaymentMethod paymentMethod, int orderId)
        {
            Buyer = buyer;
            PaymentMethod = paymentMethod;
            OrderId = orderId;
        }
    }
}
