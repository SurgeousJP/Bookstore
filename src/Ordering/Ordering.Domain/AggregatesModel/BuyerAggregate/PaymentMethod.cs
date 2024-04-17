using Ordering.Domain.Exceptions;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    internal class PaymentMethod
    {
        private string _alias;
        private string _cardNumber;
        private string _securityNumber;
        private string _cardHolderName;
        private DateTime _expiration;

        private int _cardTypeId;
        public CardType CardType { get; private set; }

        protected PaymentMethod() { }

        public PaymentMethod(string alias, string cardNumber, string securityNumber, 
            string cardHolderName, int cardTypeId, DateTime expiration)
        {
            
            _cardNumber = !string.IsNullOrWhiteSpace(cardNumber) ? cardNumber : throw new OrderingDomainException(nameof(cardNumber));

            _securityNumber = !string.IsNullOrWhiteSpace(securityNumber) ? securityNumber : throw new OrderingDomainException(nameof(securityNumber));

            _cardHolderName = !string.IsNullOrWhiteSpace(cardHolderName) ? cardHolderName : throw new OrderingDomainException(nameof(cardHolderName));

            if (expiration < DateTime.Now)
            {
                throw new OrderingDomainException(nameof(expiration));
            }

            _expiration = expiration;
            _cardTypeId = cardTypeId;
            _alias = alias;
        }

        public bool IsEqualTo(int cardTypeId, string cardNumber, DateTime expiration)
        {
            return _cardTypeId == cardTypeId 
                && cardNumber == _cardNumber
                && _expiration == expiration;
        }
    }
}
