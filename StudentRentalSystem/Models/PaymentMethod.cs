using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace StudentRentalSystem.Models
{

    public class PaymentMethod
    {


        [Key]
        public int PaymentMethodId { get; set; }



        // Which Item this payment belongs to

        [ForeignKey("Item")]
        public int ItemId { get; set; }



        public Item? Item { get; set; }







        // Example:
        // bKash
        // Nagad
        // Bank

        [Required]
        public string MethodName { get; set; }







        // Account number

        [Required]
        public string AccountNumber { get; set; }



    }

}