using Ordering.API.BuyerModel;
using Ordering.API.Models.DTOs;
using Ordering.API.Models.OrderModel;
using System.Text.Json.Serialization;

namespace Ordering.API.Models.BuyerModel;

public partial class Buyer
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>();

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public PaymentMethod VerifyOrAddPaymentMethod(PaymentMethodDTO paymentMethod)
    {
        var existingPayment = PaymentMethods.SingleOrDefault(p =>
               p.CardTypeId == paymentMethod.CardTypeId
            && p.CardNumber == paymentMethod.CardNumber
            && p.Expiration == paymentMethod.Expiration);

        if (existingPayment != null)
        {
            return existingPayment;
        }

        var newPaymentMethod = new PaymentMethod
        {
            CardTypeId = paymentMethod.CardTypeId,
            Alias = paymentMethod.Alias,
            CardNumber = paymentMethod.CardNumber,
            SecurityNumber = paymentMethod.SecurityNumber,
            CardHoldername = paymentMethod.CardHoldername,
            Expiration = paymentMethod.Expiration,
        };

        PaymentMethods.Add(newPaymentMethod);

        return newPaymentMethod;
    }

    public Address VerifyOrAddAddress(AddressDTO address)
    {
        var existingAddress = Addresses.SingleOrDefault(a =>
                a.Street == address.Street
             && a.Ward == address.Ward
             && a.City == address.City
             && a.District == address.District
             && a.ZipCode == address.ZipCode
             && a.Country == address.Country
             );

        if (existingAddress != null)
        {
            return existingAddress;
        }

        var newAddress = new Address
        {
            Street = address.Street,
            Ward = address.Ward,
            City = address.City,
            District = address.District,
            ZipCode = address.ZipCode,
            Country = address.Country,
            BuyerId = address.BuyerId
        };

        Addresses.Add(newAddress);

        return newAddress;
    }
}
