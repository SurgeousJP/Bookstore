namespace Ordering.API.Models.DTOs
{
    public class AddressDTO
    {
        public string Street { get; set; } = null!;

        public string? Ward { get; set; }

        public string? District { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public string? ZipCode { get; set; }

        public Guid? BuyerId { get; set; }
    }
}
