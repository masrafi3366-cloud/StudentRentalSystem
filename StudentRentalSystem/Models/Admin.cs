using System.ComponentModel.DataAnnotations;


namespace StudentRentalSystem.Models
{

    public class Admin
    {


        [Key]
        public int AdminId { get; set; }





        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;






        [Required]
        public string Password { get; set; } = string.Empty;



    }

}