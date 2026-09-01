using System.ComponentModel.DataAnnotations;


namespace StudentRentalSystem.Models
{

    public class Payment
    {


        [Key]
        public int PaymentId { get; set; }


        public int RentalId { get; set; }


        public decimal Amount { get; set; }


        public DateTime PaymentDate { get; set; }


        public string PaymentStatus { get; set; }


        public string TransactionId { get; set; }


    }

}