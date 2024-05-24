using Ordering.API.BuyerModel;
using Ordering.API.Models.BuyerModel;
using Ordering.API.Models.DTOs;
using Ordering.API.Models.OrderModel;

namespace Ordering.API.Models
{
    public static class OrderMapper

    {
        public static OrderDTO ToOrderDTO(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                AddressId = order.AddressId,
                BuyerId = order.BuyerId,
                OrderStatusId = order.OrderStatusId,
                Description = order.Description,
                PaymentMethodId = order.PaymentMethodId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                BuyerName = order.Buyer != null ? order.Buyer.Name : "",
                OrderStatusName = order.OrderStatus != null ? order.OrderStatus.Name : ""
            };
        }

        public static OrderDetailDTO ToOrderDetailDTO(Order order)
        {
            return new OrderDetailDTO
            {
                Id = order.Id,
                AddressId = order.AddressId,
                BuyerId = order.BuyerId,
                OrderStatusId = order.OrderStatusId,
                Description = order.Description,
                PaymentMethodId = order.PaymentMethodId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Street = order.Address.Street,
                Ward = order.Address.Ward,
                City = order.Address.City,
                Country = order.Address.Country,
                ZipCode = order.Address.ZipCode,
                BuyerName = order.Buyer != null ? order.Buyer.Name : "",
                OrderItems = order.OrderItems.Select(oi => ToOrderItemDTO(oi)),
                OrderStatusName = order.OrderStatus != null ? order.OrderStatus.Name : "",
                PaymentMethodName = order.PaymentMethod != null ? order.PaymentMethod.Alias : ""
            };
        }

        public static OrderItem ToOrderItem(OrderItemDTO orderItemDTO)
        {
            return new OrderItem
            {
                BookId = orderItemDTO.BookId,
                Title = orderItemDTO.Title,
                UnitPrice = orderItemDTO.UnitPrice,
                OldUnitPrice = orderItemDTO.OldUnitPrice,
                TotalUnitPrice = orderItemDTO.TotalUnitPrice,
                Quantity = orderItemDTO.Quantity,
                ImageUrl = orderItemDTO.ImageUrl,
            };
        }

        public static OrderItemDTO ToOrderItemDTO(OrderItem orderItemDTO)
        {
            return new OrderItemDTO
            {
                BookId = orderItemDTO.BookId,
                Title = orderItemDTO.Title,
                UnitPrice = orderItemDTO.UnitPrice,
                OldUnitPrice = orderItemDTO.OldUnitPrice,
                TotalUnitPrice = orderItemDTO.TotalUnitPrice,
                Quantity = orderItemDTO.Quantity,
                ImageUrl = orderItemDTO.ImageUrl,
            };
        }

        public static Transaction ToTransaction(CreateTransactionDTO transaction)
        {
            return new Transaction
            {
                BuyerId = transaction.BuyerId,
                TotalAmount = transaction.TotalAmount,
                PaymentMethodId = transaction.PaymentMethodId,
                Status = transaction.Status,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }

        public static Transaction ToTransactionFromDTO(TransactionDetailDTO transaction)
        {
            return new Transaction
            {
                Id = transaction.Id,
                BuyerId = transaction.BuyerId,
                TotalAmount = transaction.TotalAmount,
                PaymentMethodId = transaction.PaymentMethodId,
                Status = transaction.Status,
                CreatedAt = transaction.CreatedAt,
            };
        }

        public static TransactionDetailDTO ToTransactionDetailDTO(Transaction transaction)
        {
            return new TransactionDetailDTO
            {
                Id = transaction.Id,
                BuyerId = transaction.BuyerId,
                TotalAmount = transaction.TotalAmount,
                PaymentMethodId = transaction.PaymentMethodId,
                Status = transaction.Status,
                CreatedAt = transaction.CreatedAt,
                BuyerName = transaction.Buyer != null ? transaction.Buyer.Name : "",
            };
        }

        public static Address ToAddress(AddressDTO addressDTO)
        {
            return new Address
            {
                Street = addressDTO.Street,
                City = addressDTO.City,
                Ward = addressDTO.Ward,
                District = addressDTO.District,
                Country = addressDTO.Country,
                ZipCode = addressDTO.ZipCode,
                BuyerId = addressDTO.BuyerId
            };
        }

        public static AddressDTO ToAddressDTO(Address address)
        {
            return new AddressDTO
            {
                Id = (int)address.Id,
                Street = address.Street,
                City = address.City,
                Ward = address.Ward,
                District = address.District,
                Country = address.Country,
                ZipCode = address.ZipCode,
                BuyerId = address.BuyerId
            };
        }

        public static PaymentMethod ToPaymentMethod(PaymentMethodDTO paymentMethodDTO)
        {
            return new PaymentMethod
            {
                Alias = paymentMethodDTO.Alias,
                CardNumber = paymentMethodDTO.CardNumber,
                SecurityNumber = paymentMethodDTO.SecurityNumber,
                CardHoldername = paymentMethodDTO.CardHoldername,
                Expiration = paymentMethodDTO.Expiration,
                CardTypeId = paymentMethodDTO.CardTypeId,
            };
        }

        public static PaymentMethodDTO ToPaymentMethodDTO(PaymentMethod paymentMethodDTO)
        {
            return new PaymentMethodDTO
            {
                Id = paymentMethodDTO.Id,
                Alias = paymentMethodDTO.Alias,
                CardNumber = paymentMethodDTO.CardNumber,
                SecurityNumber = paymentMethodDTO.SecurityNumber,
                CardHoldername = paymentMethodDTO.CardHoldername,
                Expiration = paymentMethodDTO.Expiration,
                CardTypeId = paymentMethodDTO.CardTypeId,
                CardTypeName = paymentMethodDTO.CardType.Name ?? ""
            };
        }
    }
}
