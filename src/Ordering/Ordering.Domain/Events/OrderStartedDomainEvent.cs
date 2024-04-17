using MediatR;
using Ordering.Domain.AggregatesModel.OrderAggregate;

namespace Ordering.Domain.Events
{
    internal class OrderStartedDomainEvent : INotification
    {
        public OrderStartedDomainEvent(string userId, string userName, int cardTypeId, string cardNumber, string cardSecurityNumber, string cardHolderName, DateTime cardExpiration, Order order)
        {
            UserId = userId;
            UserName = userName;
            CardTypeId = cardTypeId;
            CardNumber = cardNumber;
            CardSecurityNumber = cardSecurityNumber;
            CardHolderName = cardHolderName;
            CardExpiration = cardExpiration;
            Order = order;
        }

        ///<summary>
        ///Event used when an order is created
        /// </summary>

        public string UserId { get; }
        public string UserName { get; }
        public int CardTypeId { get; }
        public string CardNumber { get; }
        public string CardSecurityNumber { get; }
        public string CardHolderName { get; }
        public DateTime CardExpiration {  get; }
        public Order Order { get; }
    }
}
