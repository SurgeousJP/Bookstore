using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.BuyerAggregate
{
    internal class CardType : Enumeration
    {
        public static CardType Paypal = new(1, nameof(Paypal));
        public static CardType Visa = new(2, nameof(Visa));
        public static CardType MasterCard = new(3, nameof(MasterCard)); 

        public CardType(int id, string name) : base(id, name)
        {
        }
    }
}
