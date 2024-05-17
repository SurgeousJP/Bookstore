using Ordering.API.BuyerModel;
using Ordering.API.Models.BuyerModel;

namespace Ordering.API.Models.OrderModel
{
    public class Transaction
    {
        public static string TRANSACTION_SUCCESS = "Success";
        public static string TRANSACTION_FAILED = "Failed";

        public long Id { get; set; }
        public Guid BuyerId { get; set; }
        public decimal TotalAmount { get; set; }
        public long PaymentMethodId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Buyer? Buyer { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }
}
