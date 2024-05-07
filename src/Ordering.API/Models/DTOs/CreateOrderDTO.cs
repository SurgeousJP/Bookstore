using Ordering.API.Models.OrderModel;

namespace Ordering.API.Models.DTOs
{
    public class CreateOrderDTO
    {
        public Guid userId { get; set; }

        public string userName { get; set; }
         
        public string description { get; set; }
         
        public AddressDTO address { get; set; }

        public PaymentMethodDTO paymentMethod { get; set; }

        public List<OrderItem> orderItems { get; set; }
    }
}
