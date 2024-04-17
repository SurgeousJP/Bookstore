using Ordering.Domain.Events;
using Ordering.Domain.Exceptions;
using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    internal class Buyer : Entity, IAggregateRoot
    {
        public string IdentityGuid { get; private set; }
        
        public string Name { get; private set; }

        private List<PaymentMethod> _paymentMethods;

        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        public Buyer (string identity, string name)
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new OrderingDomainException(nameof(identity));

            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new OrderingDomainException(nameof(name));
        }

        public PaymentMethod VerifyOrAddPaymentMethod(int cardTypeId, string alias, string cardNumber, string securityNumber, string cardHolderName, DateTime expiration, int orderId)
        {
            var existingPayment = _paymentMethods.SingleOrDefault(p => p.IsEqualTo(cardTypeId, cardNumber, expiration));

            if (existingPayment != null)
            {
                AddDomainEvent(new BuyerPaymentMethodVerifiedDomainEvent(this, existingPayment, orderId));

                return existingPayment;
            }

            var newPaymentMethod = new PaymentMethod(alias, cardNumber, securityNumber, cardHolderName, cardTypeId, expiration);

            _paymentMethods.Add(newPaymentMethod);
            AddDomainEvent(new BuyerPaymentMethodVerifiedDomainEvent(this, newPaymentMethod, orderId));

            return newPaymentMethod;
        }
    }
}
