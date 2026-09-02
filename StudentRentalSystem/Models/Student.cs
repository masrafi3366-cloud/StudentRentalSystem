using System.ComponentModel.DataAnnotations;


namespace StudentRentalSystem.Models
{

    public class Student
    {


        [Key]
        public int StudentId { get; set; }





        [Required(ErrorMessage = "Full name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string FullName { get; set; } = string.Empty;






        [Required(ErrorMessage = "Mobile number is required")]
        [Phone(ErrorMessage = "Enter a valid mobile number")]
        public string Mobile { get; set; } = string.Empty;






        [Required(ErrorMessage = "Student ID card image is required")]
        public string StudentIdCardImage { get; set; } = string.Empty;






        [Required(ErrorMessage = "Father's number is required")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        public string FathersNumber { get; set; } = string.Empty;






        [Required(ErrorMessage = "Mother's number is required")]
        [Phone(ErrorMessage = "Enter a valid phone number")]
        public string MothersNumber { get; set; } = string.Empty;






        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; } = string.Empty;






        [Required(ErrorMessage = "Password is required")]
        [StringLength(
            20,
            MinimumLength = 6,
            ErrorMessage = "Password must be 6-20 characters"
        )]
        public string Password { get; set; } = string.Empty;






        public bool IsApproved { get; set; }






        public DateTime RegistrationDate { get; set; }



    }

}