namespace StudentRentalSystem.Models.ViewModels
{

    public class AdminPaymentViewModel
    {


        public int PaymentId { get; set; }


        public string StudentName { get; set; } = string.Empty;


        public string ItemName { get; set; } = string.Empty;


        public decimal Amount { get; set; }


        public string PaymentMethod { get; set; } = string.Empty;


        public string TransactionId { get; set; } = string.Empty;


        public string PaymentStatus { get; set; } = string.Empty;


        public DateTime PaymentDate { get; set; }


    }

}