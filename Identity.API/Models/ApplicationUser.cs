using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Identity supports Username, Email, PasswordHash, PhoneNumber
        // Card information

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name can only contain letters")]
        public string? CardFullName;

        [CreditCard]
        public string? CardNumber;

        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid CVC")]
        public string? CardCVC;

        public DateOnly? ExpirationDate;

        // Shipping information
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name can only contain letters")]
        public string? FirstName;

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last name can only contain letters")]
        public string? LastName;

        [StringLength(100)]
        public string? Address;

        // Image
        public string? ProfileImageLink;

        // Basic information
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name can only contain letters")]
        public string FullName;
        // Constraint right at frontend
        [Required]
        public string Country;
        [Required]
        public string City;
        [Required]
        public string Timezone;
        [Required] 
        public string Role;
    }
}
