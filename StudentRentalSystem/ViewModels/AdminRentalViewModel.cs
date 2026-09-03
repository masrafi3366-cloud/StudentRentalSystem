namespace StudentRentalSystem.Models.ViewModels
{


    public class AdminRentalViewModel
    {


        public int RentalId { get; set; }



        public string StudentName { get; set; }



        public string ItemName { get; set; }



        public DateTime StartDate { get; set; }



        public DateTime EndDate { get; set; }



        public int RentalDays { get; set; }



        public decimal TotalAmount { get; set; }



        public string RentalStatus { get; set; }



        public string PaymentStatus { get; set; }



        public string TransactionId { get; set; }



        public DateTime? PaymentDate { get; set; }



    }


}