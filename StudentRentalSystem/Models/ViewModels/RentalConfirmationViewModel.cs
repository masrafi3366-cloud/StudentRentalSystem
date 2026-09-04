using System;


namespace StudentRentalSystem.Models.ViewModels
{

    public class RentalConfirmationViewModel
    {


        public int RentalId { get; set; }


        public int ItemId { get; set; }



        public string ItemName { get; set; } = string.Empty;



        public string OwnerName { get; set; } = string.Empty;



        public string OwnerMobile { get; set; } = string.Empty;



        public DateTime StartDate { get; set; }



        public DateTime EndDate { get; set; }



        public int RentalDays { get; set; }



        public decimal TotalAmount { get; set; }



        public string Status { get; set; } = string.Empty;



        public string PaymentStatus { get; set; } = "Pending";


    }

}