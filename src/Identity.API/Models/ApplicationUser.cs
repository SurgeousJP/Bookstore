using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Identity supports Username, Email, PasswordHash, PhoneNumber
        // Card information
        public string? CardFullName { get; set; }

        [CreditCard]
        public string? CardNumber { get; set; }

        public string? CardCVC { get; set; }

        public DateOnly? ExpirationDate { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Address { get; set; }

        public string? ProfileImageLink { get; set; }

        public string? FullName { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Timezone { get; set; }

        public string? Role { get; set; }
    }
}
