using System.ComponentModel.DataAnnotations;


namespace StudentRentalSystem.Models
{

    public class Student
    {

        [Key]
        public int StudentId { get; set; }


        [Required]
        public string FullName { get; set; }


        [Required]
        public string Mobile { get; set; }


        [Required]
        public string StudentIdCardImage { get; set; }


        [Required]
        public string FathersNumber { get; set; }


        [Required]
        public string MothersNumber { get; set; }


        [Required]
        public string Email { get; set; }


        [Required]
        public string Password { get; set; }


        public bool IsApproved { get; set; }


        public DateTime RegistrationDate { get; set; }


    }

}