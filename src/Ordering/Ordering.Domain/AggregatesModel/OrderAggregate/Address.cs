using Ordering.Domain.SeedWork;

namespace Ordering.Domain.AggregatesModel.OrderAggregate
{
    internal class Address : ValueObject
    {
        public string Street { get; private set; }
        public string Ward { get; private set; } // phuong
        public string District { get; private set; } 
        public string City { get; private set; } 
        public string Country { get; private set; } 
        public string ZipCode { get; private set; }

        public Address(string street, string ward, string district, string city, string country, string zipcode)
        {
            Street = street;
            Ward = ward;
            District = district;
            City = city;
            Country = country;
            ZipCode = zipcode;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street;
            yield return Ward;
            yield return District;
            yield return City;
            yield return Country;
            yield return ZipCode;
        }
    }
}
