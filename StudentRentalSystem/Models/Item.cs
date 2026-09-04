using System.ComponentModel.DataAnnotations;


namespace StudentRentalSystem.Models
{

    public class Item
    {


        [Key]
        public int ItemId { get; set; }




        public int StudentId { get; set; }







        [Required(ErrorMessage = "Item name is required")]
        public string ItemName { get; set; } = string.Empty;







        public string Category { get; set; } = string.Empty;







        public string Description { get; set; } = string.Empty;







        public string Image { get; set; } = string.Empty;







        public decimal PricePerDay { get; set; }







        public bool AdminApproved { get; set; }







        public DateTime CreatedDate { get; set; }




        public bool IsRented { get; set; }



        public ICollection<PaymentMethod> PaymentMethods { get; set; }
= new List<PaymentMethod>();


    }

}