using System.ComponentModel.DataAnnotations;


namespace StudentRentalSystem.Models
{

    public class Item
    {


        [Key]
        public int ItemId { get; set; }


        public int StudentId { get; set; }


        [Required]
        public string ItemName { get; set; }


        public string Category { get; set; }


        public string Description { get; set; }


        public string Image { get; set; }


        public decimal PricePerDay { get; set; }


        public bool AdminApproved { get; set; }


        public DateTime CreatedDate { get; set; }


    }

}