using Ordering.API.Models.DTOs;
using Ordering.API.Models.OrderModel;

namespace BookCatalog.API.Queries.Mappers
{
    public static class OrderMapper

    {
        public static OrderDTO ToOrderDTO(Order order) => new OrderDTO
        {
            Id = order.Id,
            AddressId = order.AddressId,
            BuyerId = order.BuyerId,
            OrderStatusId = order.OrderStatusId,
            Description = order.Description,
            PaymentMethodId = order.PaymentMethodId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            BuyerName = order.Buyer.Name,
            OrderStatusName = order.OrderStatus.Name
        };

        public static OrderDetailDTO ToOrderDetailDTO(Order order) => new OrderDetailDTO
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
            BuyerName = order.Buyer.Name,
            OrderItems = order.OrderItems,
            OrderStatusName = order.OrderStatus.Name,
            PaymentMethodName = order.PaymentMethod.Alias
        };

        public static OrderItem ToOrderItem(OrderItemDTO orderItemDTO) => new OrderItem
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
}
