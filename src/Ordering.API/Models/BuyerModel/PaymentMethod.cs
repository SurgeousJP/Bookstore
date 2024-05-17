using Ordering.API.Extensions;
using Ordering.API.Models.BuyerModel;
using Ordering.API.Models.OrderModel;
using System.Text.Json.Serialization;

namespace Ordering.API.BuyerModel;

public partial class PaymentMethod
{
    public long Id { get; set; }

    public string Alias { get; set; } = null!;

    public string? CardNumber { get; set; }

    public string? SecurityNumber { get; set; }

    public string? CardHoldername { get; set; }

    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly? Expiration { get; set; }

    public long? CardTypeId { get; set; }

    public Guid? BuyerId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual Buyer? Buyer { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual Cardtype? CardType { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public PaymentMethod() { }

    public PaymentMethod(
        long cardTypeId, string alias, string cardNumber,
        string securityNumber, string cardHolderName, DateOnly expiration)
    {
        this.CardTypeId = cardTypeId;
        this.Alias = alias;
        this.CardNumber = cardNumber;
        this.Expiration = expiration;
        this.SecurityNumber = securityNumber;
        this.CardHoldername = cardHolderName;
    }
}
