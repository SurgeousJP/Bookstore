using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models
{
    public class UserUpdateDTO
    {
        // Identity supports Username, Email, PasswordHash, PhoneNumber
        // Card information
        public string Id;

        public string? UserName { get; set; }

        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        [RegularExpression(@"^\+?\d{0,2}\-?\d{4,5}\-?\d{5,6}$", ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name can only contain letters")]
        public string? CardFullName { get; set; }

        [CreditCard]
        public string? CardNumber { get; set; }

        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid CVC")]
        public string? CardCVC { get; set; }

        public DateOnly? ExpirationDate { get; set; }

        // Shipping information
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "First name can only contain letters")]
        public string? FirstName { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Last name can only contain letters")]
        public string? LastName { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        // Image
        public string? ProfileImageLink { get; set; }

        // Basic information
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full name can only contain letters")]
        public string? FullName { get; set; }

        public string? Country { get; set; }

        public string? City;

        public string? Timezone;

        public void setUpdateInfoToApplicationUser(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (UserName != null)
            {
                user.UserName = UserName;
            }

            // Update identity information
            if (Email != null)
            {
                user.Email = Email;
            }
            if (PhoneNumber != null)
            {
                user.PhoneNumber = PhoneNumber;
            }

            // Update card information
            if (CardFullName != null)
            {
                user.CardFullName = CardFullName;
            }
            if (CardNumber != null)
            {
                user.CardNumber = CardNumber;
            }
            if (CardCVC != null)
            {
                user.CardCVC = CardCVC;
            }
            if (ExpirationDate != null)
            {
                user.ExpirationDate = ExpirationDate;
            }

            // Update shipping information
            if (FirstName != null)
            {
                user.FirstName = FirstName;
            }
            if (LastName != null)
            {
                user.LastName = LastName;
            }
            if (Address != null)
            {
                user.Address = Address;
            }

            // Update profile image link
            if (ProfileImageLink != null)
            {
                user.ProfileImageLink = ProfileImageLink;
            }

            // Update basic information
            if (FullName != null)
            {
                user.FullName = FullName;
            }
            if (Country != null)
            {
                user.Country = Country;
            }
            if (City != null)
            {
                user.City = City;
            }
            if (Timezone != null)
            {
                user.Timezone = Timezone;
            }
        }
    }
}
