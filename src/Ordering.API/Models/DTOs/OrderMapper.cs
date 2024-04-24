using Ordering.API.Models.DTOs;
using Ordering.API.Models.Order;
using System;
using System.Linq;
using System.Xml.Linq;

namespace BookCatalog.API.Queries.Mappers
{
    public static class OrderMapper

    {
        public static CreateOrderDTO ToCreateOrderDTO(Order order) => new CreateOrderDTO
        {
            AddressId = order.AddressId,
            BuyerId = order.BuyerId,
            OrderStatusId = order.OrderStatusId,
            Description = order.Description,
            PaymentMethodId = order.PaymentMethodId,
            OrderDate = order.OrderDate,
            TotalAmount = order.TotalAmount,
            ShippingId = order.ShippingId
        };

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
            ShippingId = order.ShippingId,
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
            ShippingId= order.ShippingId,
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
    }
}
