namespace Identity.API.Models
{
    public class UpdatePasswordDTO
    {
        public string CurrentPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;

        public bool isAnyAttributeNullOrEmpty()
        {
            return string.IsNullOrEmpty(CurrentPassword)
                || string.IsNullOrEmpty(NewPassword);
        }
    }
}
