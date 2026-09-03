using System.ComponentModel.DataAnnotations;


namespace StudentRentalSystem.Models
{

    public class Rental
    {


        [Key]
        public int RentalId { get; set; }







        public int StudentId { get; set; }







        public int ItemId { get; set; }







        public int RentalDays { get; set; }







        public DateTime StartDate { get; set; }







        public DateTime EndDate { get; set; }







        public decimal TotalAmount { get; set; }







        // Pending / Active / Completed / Returned

        public string Status { get; set; } = "Pending";







        // Actual return date

        public DateTime? ReturnDate { get; set; }






        public bool IsReturned { get; set; }




    }

}