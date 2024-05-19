using Ordering.API.Models.OrderModel;
using System.Text.Json.Serialization;

namespace Ordering.API.Models.BuyerModel;

public partial class Address
{
    public long Id { get; set; }

    public string Street { get; set; } = null!;

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public string? ZipCode { get; set; }

    public Guid? BuyerId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<Order>? Orders { get; set; } = new List<Order>();

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual Buyer? Buyer { get; set; }

    public Address() { }

    public Address(string street, string? ward, string? district, string? city, string? country, string? zipCode, Guid? buyerId)
    {
        Street = street;
        Ward = ward;
        District = district;
        City = city;
        Country = country;
        ZipCode = zipCode;
        BuyerId = buyerId;
    }
}
